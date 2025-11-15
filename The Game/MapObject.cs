using System;

public class MapObject
{
    private const int GridY = 25;
    private const int GridX = 51;
    public static Dictionary<char, Func<int, int, int, int, char, TileObject>>? _tileHandlers;
    public MapObject()
	{
        _tileHandlers = new Dictionary<char, Func<int, int, int, int, char, TileObject>>
        {
            #region === Spawn Points, Characters, and Triggers ===
            { '&', TileProcessor.OnNPCSpawn }
            #endregion
        };
    }
}
