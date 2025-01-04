using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Threading.Tasks;
using DynamicData;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Xabbo.Extension;
using Xabbo.Core;
using Xabbo.Core.GameData;
using Xabbo.Services.Abstractions;
using System.Collections.ObjectModel;
using System.Reactive;
using Xabbo.Core.Messages.Incoming;

namespace Xabbo.ViewModels
{
    public class FurniDataViewModel : ViewModelBase
    {
        private readonly IExtension _extension;
        private readonly IUiContext _uiContext;
        private readonly IGameDataManager _gameDataManager;

        private readonly SourceCache<FurniInfoViewModel, (ItemType, long, string?)> _furniCache =
            new(x => (x.Type, x.TypeID, x.Identifier));
        private readonly ReadOnlyObservableCollection<FurniInfoViewModel> _furni;

        public ReadOnlyObservableCollection<FurniInfoViewModel> Furni => _furni;

        // Updated Filter Options
        public List<string> FilterOptions { get; } = new() { "Name", "Description", "ClassName" };
        public ReactiveCommand<FurniInfoViewModel, Unit> SpawnFurniCommand { get; }

        [Reactive] public string SelectedFilter { get; set; } = "Name";
        [Reactive] public string FilterText { get; set; } = "";

        [Reactive] public bool IsLoading { get; set; }
        [Reactive] public string ErrorText { get; set; } = "";

        public FurniDataViewModel(
            IExtension extension,
            IUiContext uiContext,
            IGameDataManager gameDataManager)
        {
            _extension = extension;
            _uiContext = uiContext;
            _gameDataManager = gameDataManager;

            _gameDataManager.Loaded += OnGameDataLoaded;
            SpawnFurniCommand = ReactiveCommand.Create<FurniInfoViewModel>(SpawnFurni);

            // Setup DynamicData pipeline with filtering
            _furniCache
                .Connect()
                .Filter(this.WhenAnyValue(
                    vm => vm.FilterText,
                    vm => vm.SelectedFilter,
                    (text, filter) => CreateFilter(text, filter)))
                .ObserveOn(RxApp.MainThreadScheduler)
                .Bind(out _furni)
                .Subscribe();

            _extension.Connected += OnGameConnected;
            _extension.Disconnected += OnGameDisconnected;
        }

        private void SpawnFurni(FurniInfoViewModel furni)
        {
            // generate a random id for the furni
            int id = new Random().Next(0, int.MaxValue);
            var itemdata = Extensions.GetByType(furni.Type, furni.TypeID);
            if (itemdata is null) return;
            if (itemdata.Type == ItemType.Floor)
            {
                // find the item from the FurniData and create a new FloorItem
                var floor = new FloorItem(id, itemdata.TypeID, new Tile(1, 1, 0), 0, 0, 0, new EmptyItemData(), 0,
                    FurniUsage.Anyone, 0, "Xabbo");
                _extension.Send(new FloorItemAddedMsg(floor));
            }
            else if(itemdata.Type == ItemType.Wall)
            {
                var wall = new WallItem(id, itemdata.TypeID, WallLocation.Zero, "", 0, FurniUsage.Anyone, 0, "Xabbo");
                _extension.Send(new WallItemAddedMsg(wall));
            }

        }
        private void OnGameDataLoaded()
        {
            FurniData? furniData = _gameDataManager.Furni;
            if (furniData is null) return;

            _uiContext.Invoke(() =>
            {
                foreach (FurniInfo info in furniData)
                {
                    _furniCache.AddOrUpdate(new FurniInfoViewModel(info));
                }
            });
        }

        private async void OnGameConnected(ConnectedEventArgs e)
        {
            try
            {
                IsLoading = true;

                await _gameDataManager.WaitForLoadAsync(_extension.DisconnectToken);

                FurniData? furniData = _gameDataManager.Furni;
                if (furniData is null) return;

                await _uiContext.InvokeAsync(() =>
                {
                    foreach (FurniInfo info in furniData)
                    {
                        _furniCache.AddOrUpdate(new FurniInfoViewModel(info));
                    }
                });
            }
            catch (Exception ex)
            {
                ErrorText = $"Failed to load furni data: {ex.Message}.";
            }
            finally
            {
                IsLoading = false;
            }
        }

        private void OnGameDisconnected() => _furniCache.Clear();

        // Updated CreateFilter method
        private Func<FurniInfoViewModel, bool> CreateFilter(string? filterText, string selectedFilter)
        {
            if (string.IsNullOrWhiteSpace(filterText))
                return static vm => true;

            return vm =>
            {
                if (string.IsNullOrWhiteSpace(filterText))
                    return true;

                return selectedFilter switch
                {
                    "Name" => vm.Identifier.Contains(filterText, StringComparison.OrdinalIgnoreCase),
                    "Description" => vm.Description.Contains(filterText, StringComparison.OrdinalIgnoreCase),
                    "ClassName" => vm.ClassName.Contains(filterText, StringComparison.OrdinalIgnoreCase),
                    _ => true,
                };
            };
        }
    }
}
