﻿using System;
using System.Collections.Generic;
using System.Text;

using Xabbo.Messages;

namespace Xabbo.Core;

public class FloorPlan : IFloorPlan, IParserComposer<FloorPlan>
{
    private static readonly char[] _newlineChars = ['\r', '\n'];

    public string? OriginalString { get; private set; }

    public bool UseLegacyScale { get; set; }
    public int Scale => UseLegacyScale ? 32 : 64;
    public int WallHeight { get; set; }
    public Point Size { get; }
    public Area Area => new(Size);

    private readonly int[] _tiles;
    public IReadOnlyList<int> Tiles => _tiles;

    public int this[int x, int y]
    {
        get => GetHeight(x, y);
        set => SetHeight(x, y, value);
    }

    public int this[Point point]
    {
        get => this[point.X, point.Y];
        set => this[point.X, point.Y] = value;
    }

    public FloorPlan(int width, int length)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(width, 0);
        ArgumentOutOfRangeException.ThrowIfLessThan(length, 0);

        Size = (width, length);
        _tiles = new int[width * length];
    }


    public FloorPlan(string map)
    {
        OriginalString = map;

        UseLegacyScale = false;
        WallHeight = -1;

        _tiles = ParseString(map, out int width, out int length);
        Size = (width, length);
    }

    public int GetHeight(int x, int y)
    {
        if (x < 0 || x >= Size.X)
            throw new ArgumentOutOfRangeException(nameof(x));
        if (y < 0 || y >= Size.Y)
            throw new ArgumentOutOfRangeException(nameof(y));

        return _tiles[y * Size.X + x];
    }

    public int GetHeight((int X, int Y) location) => GetHeight(location.X, location.Y);

    public int GetHeight(Tile tile) => GetHeight(tile.X, tile.Y);

    public void SetHeight(int x, int y, int height)
    {
        if (x < 0 || x >= Size.X)
            throw new ArgumentOutOfRangeException(nameof(x));
        if (y < 0 || y >= Size.Y)
            throw new ArgumentOutOfRangeException(nameof(y));
        ArgumentOutOfRangeException.ThrowIfLessThan(height, -1);

        _tiles[y * Size.X + x] = height;
    }

    public void SetHeight((int X, int Y) location, int height) => SetHeight(location.X, location.Y, height);

    public void SetHeight(Tile location, int height) => SetHeight(location.X, location.Y, height);

    public bool IsWalkable(Point point) => IsWalkable(point.X, point.Y);

    public bool IsWalkable(int x, int y)
    {
        if (x < 0 || x >= Size.X || y < 0 || y >= Size.Y)
            return false;

        return GetHeight(x, y) >= 0;
    }

    public bool IsWalkable((int X, int Y) location) => IsWalkable(location.X, location.Y);

    public bool IsWalkable(Tile location) => IsWalkable(location.X, location.Y);

    public override string ToString()
    {
        var sb = new StringBuilder();

        for (int y = 0; y < Size.Y; y++)
        {
            if (y > 0) sb.Append('\r');
            for (int x = 0; x < Size.X; x++)
            {
                sb.Append(GetCharacterFromHeight(_tiles[y * Size.X + x]));
            }
        }

        return sb.ToString();
    }

    void IComposer.Compose(in PacketWriter p)
    {
        p.WriteBool(UseLegacyScale);
        p.WriteInt(WallHeight);
        p.WriteString(ToString());
    }

    static FloorPlan IParser<FloorPlan>.Parse(in PacketReader p)
    {
        bool useLegacyScale = false;
        int wallHeight = 0;

        useLegacyScale = p.ReadBool();
        wallHeight = p.ReadInt();


        string map = p.ReadString();

        return new FloorPlan(map)
        {
            UseLegacyScale = useLegacyScale,
            WallHeight = wallHeight
        };
    }

    public static FloorPlan ParseString(string map) => new(map);

    private static int[] ParseString(string map, out int width, out int length)
    {
        string[] lines = map.Split(_newlineChars, StringSplitOptions.RemoveEmptyEntries);
        length = lines.Length;
        width = 0;
        for (int i = 0; i < length; i++)
        {
            if (lines[i].Length > width)
                width = lines[i].Length;
        }

        int[] tiles = new int[width * length];
        for (int i = 0; i < tiles.Length; i++)
            tiles[i] = -1;

        for (int y = 0; y < lines.Length; y++)
        {
            for (int x = 0; x < lines[y].Length; x++)
            {
                char c = lines[y][x];
                if (c != 'x' && c != 'X')
                {
                    tiles[y * width + x] = H.GetHeightFromCharacter(lines[y][x]);
                }
            }
        }

        return tiles;
    }

    public static char GetCharacterFromHeight(int height)
    {
        if (height is >= 0 and < 10)
            return (char)('0' + height);
        else if (height is >= 10 and < 36)
            return (char)('a' + (height - 10));
        else
            return 'x';
    }

    public static FloorPlan ConvertHeightmapToFloorPlan(Heightmap heightmap)
    {
        // Create a new floor plan with the same dimensions as the heightmap
        var floorPlan = new FloorPlan(heightmap.Size.X, heightmap.Size.Y);

        for (int y = 0; y < heightmap.Size.Y; y++)
        {
            for (int x = 0; x < heightmap.Size.X; x++)
            {
                var tile = heightmap[x, y];

                int tileHeight;
                // If the tile isn't free, mark it as blocked (-1)
                if (!tile.IsFree)
                {
                    tileHeight = -1;
                }
                else
                {
                    // Convert the tile height (short) to an integer for the floor plan
                    // Here we simply cast. If needed, adjust or scale this value.
                    tileHeight = (int)tile.Height;

                    // Clamp the tile height to a range representable by FloorPlan
                    if (tileHeight < 0)
                        tileHeight = -1;
                    else if (tileHeight > 35)
                        tileHeight = 35;
                }

                floorPlan[x, y] = tileHeight;
            }
        }

        return floorPlan;
    }



}
