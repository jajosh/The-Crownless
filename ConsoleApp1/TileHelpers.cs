public static class TileHelpers
{
    private static readonly (int dx, int dy)[] NeighborOffsets =
    {
        (-1, 0), (1, 0), (0, -1), (0, 1) // 4-directional
    };

    public static bool HasRoofedNeighbor(Map map, Tile tile)
    {
        foreach (var (dx, dy) in NeighborOffsets)
        {
            if (map.MapKey.TryGetValue((tile.GridX, tile.GridY, (tile.LocalX + dx, tile.LocalY + dy)), out var neighbor))
            {
                if (neighbor.IsRoofed)
                    return true;
            }
        }
        return false;
    }

    public static bool HasWalkableNeighbor(Map map, Tile tile)
    {
        foreach (var (dx, dy) in NeighborOffsets)
        {
            if (map.MapKey.TryGetValue((tile.GridX, tile.GridY, (tile.LocalX + dx, tile.LocalY + dy)), out var neighbor))
            {
                if (neighbor.IsWalkable)
                    return true;
            }
        }
        return false;
    }
}
