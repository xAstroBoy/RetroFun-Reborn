using System.Reactive.Linq;
using Microsoft.Extensions.Logging;
using ReactiveUI;

using Xabbo.Extension;
using Xabbo.Core;
using Xabbo.Core.Game;
using Xabbo.Core.GameData;
using Xabbo.Core.Messages.Outgoing;
using Xabbo.Core.Messages.Incoming;
using Xabbo.Controllers;
using Xabbo.Utility;
using Xabbo.Services.Abstractions;

namespace Xabbo.Components;

[Intercept]
public partial class FurniActionsComponent : Component
{
    private readonly ILogger _logger;
    private readonly IHabboApi _api;
    private readonly IGameDataManager _gameDataManager;
    private readonly RoomRightsController _rightsController;
    private readonly RoomManager _roomManager;
    private readonly XabbotComponent _xabbot;

    private IDisposable? _requiredRights;

    private FurniData? FurniData => _gameDataManager.Furni;
    private ExternalTexts? Texts => _gameDataManager.Texts;

    [Reactive] public bool PreventUse { get; set; }
    [Reactive] public bool PickToHide { get; set; }
    [Reactive] public bool PickToFindLink { get; set; }
    [Reactive] public bool CanShowInfo { get; set; }
    [Reactive] public bool PickToShowInfo { get; set; }

    public FurniActionsComponent(IExtension extension,
        ILoggerFactory loggerFactory,
        IHabboApi api,
        IGameDataManager gameDataManager,
        RoomRightsController rightsController,
        RoomManager roomManager,
        XabbotComponent xabbot)
        : base(extension)
    {
        _logger = loggerFactory.CreateLogger<FurniActionsComponent>();
        _api = api;
        _gameDataManager = gameDataManager;
        _rightsController = rightsController;
        _roomManager = roomManager;
        _xabbot = xabbot;

        this.WhenAnyValue(
                x => x.PickToHide,
                x => x.PickToShowInfo,
                x => x.PickToFindLink,
                (a1, a2, a3) => a1 || a2 || a3
            )
            .DistinctUntilChanged()
            .Subscribe(requiresRights => {
                _logger.LogDebug("needs rights: {Test}", requiresRights);
                if (requiresRights)
                {
                    _requiredRights ??= _rightsController.RequireRights();
                }
                else
                {
                    _requiredRights?.Dispose();
                    _requiredRights = null;
                }
            });

        _gameDataManager.Loaded += () => CanShowInfo = true;
    }

    [Intercept]
    private void HandleUseFloorItem(Intercept<UseFloorItemMsg> e)
    {
        if (PreventUse) e.Block();
    }

    [Intercept]
    private void HandleUseWallItem(Intercept<UseWallItemMsg> e)
    {
        if (PreventUse) e.Block();
    }

    [Intercept]
    private void HandlePick(Intercept e, PickupFurniMsg pick)
    {
        if (PickToHide || PickToShowInfo || PickToFindLink)
            e.Block();

        IRoom? room = _roomManager.Room;
        if (room is null)
        {
            _logger.LogWarning("User is not in a room.");
            return;
        }

        IFurni? furni = room.GetFurni(pick.Type, pick.Id);
        if (furni is null)
        {
            _logger.LogWarning("Failed to find {Type} item #{Id}.", pick.Type, pick.Id);
            return;
        }

        Point? location = furni switch {
            IWallItem it => it.Location.Wall,
            IFloorItem it => it.Location,
            _ => null
        };

        if (PickToHide)
            _roomManager.HideFurni(furni);

        if (PickToShowInfo && CanShowInfo && FurniData is not null)
        {
            if (furni.TryGetInfo(out var info))
            {
                if (!furni.TryGetName(out string? name))
                    name = info.Identifier;

                if (furni is IFloorItem floor)
                {
                    _xabbot.ShowMessage($"{name} [{info.Identifier}] (id:{furni.Id}) {floor.Location} {floor.Direction}", floor.Location);
                }
                else if (furni is IWallItem wallItem)
                {
                    _xabbot.ShowMessage($"{name} [{info.Identifier}] (id:{furni.Id}) {wallItem.Location}", wallItem.Location.Wall);
                }
            }
        }

        if (PickToFindLink && furni is IFloorItem floorItem)
        {
            // find link for floor item ( usually the tele are bought together )
            // find the other tele by finding all furnis with the same type , and add or subtract of 1 from the .id of the current tele to find the other tele, in a list
            // then find the tele that is not the current tele, and flash it
            List<IFloorItem> teles = room.FloorItems
                .Where(x => x.TypeID == floorItem.TypeID)
                .ToList();

            // find the other tele by getting the tele by adding or subtracting 1 from the current tele id
            IFloorItem? tele = teles.FirstOrDefault(x => x.Id == floorItem.Id + 1);
            IFloorItem? tele2 = teles.FirstOrDefault(x => x.Id == floorItem.Id - 1);

            if (tele is not null)
            {
                FlashTele(tele);
            }
            if (tele2 is not null)
            {
                FlashTele(tele2);
            }

        }

    }

    private void FlashTele(IFloorItem? tele)
    {
        if (tele is null)
            return;

        Task.Run(async () =>
        {
            Ext.Send(new FloorItemDataUpdatedMsg(tele.Id, new LegacyData { Value = "1" }));
            Ext.SlideFurni(tele, to: tele.Location + (0, 0, 1), duration: 500);
            await Task.Delay(1000);
            Ext.Send(new FloorItemDataUpdatedMsg(tele.Id, new LegacyData { Value = "2" }));
            await Task.Delay(1000);
            Ext.SlideFurni(tele, from: tele.Location + (0, 0, 1), duration: 500);
            Ext.Send(new FloorItemDataUpdatedMsg(tele.Id, new LegacyData { Value = "0" }));
        });
    }
}
