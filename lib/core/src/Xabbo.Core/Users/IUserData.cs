namespace Xabbo.Core;

/// <summary>
/// Represents information about the current user.
/// </summary>
public interface IUserData
{
    int Id { get; }
    string Name { get; }
    string Figure { get; } 
    string Motto { get; }
}
