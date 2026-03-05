using System;

public class MapObject
{
    private const int GridY = 25;
    private const int GridX = 51;
    public static Dictionary<string, Func<int, int, int, int, string, TileObject>>? _tileHandlers;
    public MapObject()
    {
        _tileHandlers = new Dictionary<string, Func<int, int, int, int, string, TileObject>>
        {
            #region === Spawn Points, Characters, and Triggers ===
            { "&", TileProcessor.OnNPCSpawn }
            #endregion
        };
    }
}
