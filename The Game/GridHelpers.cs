using System;

public class GridMapData
{
    public int Id { get; set; }

    // This is automatically saved/loaded as JSON!
    public List<string> GridMapKey { get; set; } = new();
}