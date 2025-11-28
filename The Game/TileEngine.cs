using System;

public interface ITileEngine
{
    static abstract DescriptionEntry? GetRandomMatchingDescription(GameEngine engine, List<DescriptionEntry> descriptions);
    static abstract Task<DescriptionEntry?> GetRandomMatchingDescriptionAsync(
        GameEngine engine,
        List<DescriptionEntry> descriptions,
        CancellationToken dddddddct = default);
    void FinalizeTiles(MapManager map);
    TileObject ProcessTile(char ascii, int gridX, int gridY, int LocalX, int LocalY);
}   