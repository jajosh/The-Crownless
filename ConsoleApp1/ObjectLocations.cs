using System;

public enum LocationType
{
    Settlement,
    Dungeon,
    Link,

}
public enum GridBiomeType // The biome of the grid
{
    Any,
    BorelForest,
    TemperateBroadleafForest,

}
public enum GridBiomeSubType // What features the grid has
{
    Any,
    AbandonedBuilding,
    Town,
    HighTown,
    LowTown,
    RoyalTown,
    Farm,
    Forest

}
public class Locations 
{
    public LocationType Type { get; set; }
    public int ID { get; set; }
    public string Name { get; set; }
    public List<((int, int, int, int), (int, int, int, int))> TransitionPoints { get; set; }// Stand here, to go there
    public List<Grid> Map { get; set; }
}
public class Grid
{
    public List<string> GridMap { get; set; } = new();
    public int GridX { get; set; }
    public int GridY { get; set; }
    public GridBiomeType Biome { get; set; }
    public GridBiomeSubType SubBiome { get; set; }
    public int RandomEventChance { get; set; }
    public List<DescriptionEntry> DescriptionEntries { get; set; } = new();
    public Dictionary<char, (int weight, string text, GridBiomeType biome, GridBiomeSubType subBiome, SeasonData Season, WeatherData weather)>? TileAdds { get; set; } = new();

    public Grid() { }

    /// <summary>
    /// Adds a description entry to the grid.
    /// Null values act as "any".
    /// </summary>
    public void AddDescription(int weight, string text, GridBiomeType? biome = null, GridBiomeSubType? subBiome = null, SeasonData? season = null, WeatherData? weather = null)
    {
        DescriptionEntries.Add(new DescriptionEntry(
            weight, text, biome, subBiome, season, weather));
    }
}
public class TriggerCoordinets
{
    public int GridX { get; set; }
    public int GridY { get; set; }
    public int LocalX { get; set; }
    public int LocalY { get; set; }
    public string Description { get; set; }
    public TileStates TileState { get; set; }
    public Dictionary<int, int> PossibleNextEvents { get; set; } // event ID, weight
    public List<string> TriggerActions { get; set; } = new();

    public TriggerCoordinets()
    {
        GridX = 0;
        GridY = 0;
        LocalX = 0;
        LocalY = 0;
        PossibleNextEvents = new();
        Description = string.Empty;
        PossibleNextEvents = new();
        TriggerActions = new();
    }
    public TriggerCoordinets(int gridX, int gridY, int localX, int localY, string description, Dictionary<int, int> possibleNextEvents, List<string> triggerActions)
    {
        GridX = gridX;
        GridY = gridY;
        LocalX = localX;
        LocalY = localY;
        Description = description;
        PossibleNextEvents = possibleNextEvents;
        TriggerActions = triggerActions;

    }
}