using Xabbo.Messages;

namespace Xabbo.Core;

/// <inheritdoc cref="IUser"/>
public class User(int id, int index) : Avatar(AvatarType.User, id, index), IUser
{
    public Gender Gender { get; set; } = Gender.Unisex;
    public int GroupId { get; set; } = -1;
    public int GroupStatus { get; set; }
    public string GroupName { get; set; } = "";
    public string FigureExtra { get; set; } = "";
    public int AchievementScore { get; set; }
    public bool IsStaff { get; set; }
    public string BadgeCode { get; set; } = "";

    public RightsLevel RightsLevel => CurrentUpdate?.RightsLevel ?? RightsLevel.None;
    public bool HasRights => RightsLevel > 0;

    internal User(int id, int index, in PacketReader p)
        : this(id, index)
    {
        Gender = H.ToGender(p.ReadString());
        GroupId = p.ReadInt();
        GroupStatus = p.ReadInt();
        GroupName = p.ReadString();
        FigureExtra = p.ReadString();
        AchievementScore = p.ReadInt();
        IsStaff = p.ReadBool();
    }

    protected override void OnUpdate(AvatarStatus update) { }

    public override void Compose(in PacketWriter p)
    {
        base.Compose(in p);
        p.WriteString(Gender.ToClientString().ToLower());
        p.WriteInt(GroupId);
        p.WriteInt(GroupStatus);
        p.WriteString(GroupName);
        p.WriteString(FigureExtra);
        p.WriteInt(AchievementScore);
        p.WriteBool(IsStaff);
    }
}
