using System;
public enum TileCheckType
{
    None = 0,
    NeighborRoofed = 1 << 0,
    NeighborWalkable = 1 << 1,
    CheckTriggerCoordinets = 1 << 2
    // Add more as needed
}
public static class TileHelpers
{
    private static readonly (int dx, int dy)[] NeighborOffsets =
    {
        (-1, 0), (1, 0), (0, -1), (0, 1) // 4-directional
    };

    public static bool HasRoofedNeighbor(MapManager map, TileObject tile)
    {
        return false;
    }

    public static bool HasWalkableNeighbor(MapManager map, TileObject tile)
    {
        return false;
    }
}