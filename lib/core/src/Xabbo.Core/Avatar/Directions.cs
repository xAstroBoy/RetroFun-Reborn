﻿namespace Xabbo.Core;

/// <summary>
/// Represents a direction in a room.
/// </summary>
/// <remarks>
/// Values range from 0-7, with each increment representing a 45-degree turn in the clockwise direction.
/// The first value (North) points towards the negative Y axis. (<c>↗</c>)
/// </remarks>
public enum Directions
{
    /// <summary>
    /// Represents the north direction.
    /// </summary>
    /// <remarks>
    /// Points towards the negative Y axis. (<c>↗</c>)
    /// </remarks>
    North = 0,
    /// <summary>
    /// Represents the north-east direction.
    /// </summary>
    /// <remarks>
    /// Points towards the positive X, negative Y axis. (<c>→</c>)
    /// </remarks>
    NorthEast = 1,
    /// <summary>
    /// Represents the east direction.
    /// </summary>
    /// <remarks>
    /// Points towards the positive X axis. (<c>↘</c>)
    /// </remarks>
    East = 2,
    /// <summary>
    /// Represents the south-east direction.
    /// </summary>
    /// <remarks>
    /// Points towards the positive X, positive Y axis. (<c>↓</c>)
    /// </remarks>
    SouthEast = 3,
    /// <summary>
    /// Represents the south direction.
    /// </summary>
    /// <remarks>
    /// Points towards the positive Y axis. (<c>↙</c>)
    /// </remarks>
    South = 4,
    /// <summary>
    /// Represents the south-west direction.
    /// </summary>
    /// <remarks>
    /// Points towards the negative X, positive Y axis. (<c>←</c>)
    /// </remarks>
    SouthWest = 5,
    /// <summary>
    /// Represents the west direction.
    /// </summary>
    /// <remarks>
    /// Points towards the negative X axis. (<c>↖</c>)
    /// </remarks>
    West = 6,
    /// <summary>
    /// Represents the north-west direction.
    /// </summary>
    /// <remarks>
    /// Points towards the negative X, negative Y axis. (<c>↑</c>)
    /// </remarks>
    NorthWest = 7
}

partial class XabboEnumExtensions
{
    public static Point ToVector(this Directions direction) => direction switch
    {
        Directions.North => (0, -1),
        Directions.NorthEast => (1, -1),
        Directions.East => (1, 0),
        Directions.SouthEast => (1, 1),
        Directions.South => (0, 1),
        Directions.SouthWest => (-1, 1),
        Directions.West => (-1, 0),
        Directions.NorthWest => (-1, -1),
        _ => throw new System.Exception($"Unknown direction: '{direction}'.")
    };

    public static Directions Opposite(this Directions direction) => (Directions)(((int)direction + 4) % 8);
}