using System;
using Newtonsoft.Json;
public enum GridBiomeType // The biome of the grid
{
    Any,
    BorelForest,
    TemperateBroadleafForest,

}
public enum GridBiomeSubType // What features the grid has
{
    Any, // Used for random description
    AbandonedBuilding,
    Town,
    HighTown,
    LowTown,
    RoyalTown,
    Farm,
    Forest

}
// Used the dehydrated tiles. 
public class GridObject
{
    public List<string> GridMapKey { get; set; } = new();
    public int GridX { get; set; }
    public int GridY { get; set; }
    public GridBiomeType Biome { get; set; }
    public GridBiomeSubType SubBiome { get; set; }
    public int RandomEventChance { get; set; }
    public List<DescriptionEntry> DescriptionEntries { get; set; } = new();
    public Dictionary<char, TileAddData>? TileAdds { get; set; } = new();

    public GridObject() { }

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
public class TileAddData
{
    public int Weight { get; set; }
    public string Text { get; set; }
    public GridBiomeType Biome { get; set; }
    public GridBiomeSubType SubBiome { get; set; }
    public SeasonData Season { get; set; }
    public WeatherData Weather { get; set; }
}
