using System.Text.Json;
using System;
using System.ComponentModel.DataAnnotations.Schema;


// Used the dehydrated tiles. 
public class GridObject
{
    // This is stored as JSON text in one column
    public string GridMapKeyJson { get; set; } = "[]"; // default empty list

    // Runtime property — NOT mapped to DB
    [NotMapped]
    public List<string> GridMapKey
    {
        get => string.IsNullOrEmpty(GridMapKeyJson)
            ? new List<string>()
            : JsonSerializer.Deserialize<List<string>>(GridMapKeyJson)!;
        set => GridMapKeyJson = JsonSerializer.Serialize(value);
    }

    public int GridX { get; set; }
    public int GridY { get; set; }
    public GridBiomeType Biome { get; set; }
    public GridBiomeSubType SubBiome { get; set; }
    public int RandomEventChance { get; set; }
    [NotMapped]
    public List<DescriptionEntry> DescriptionEntries { get; set; } = new();
    [NotMapped]
    public Dictionary<char, TileAddData>? TileAdds { get; set; } = new();

    public GridObject() { }

    /// <summary>
    /// Adds a description entry to the grid.
    /// Null values act as "any".
    /// </summary>
    public void AddDescription(int weight, string text, GridBiomeType? biome = null, GridBiomeSubType? subBiome = null, SeasonData? season = null, WeatherData? weather = null)
    {
        DescriptionEntries.Add(new DescriptionEntry
    (
            text, weight, biome, subBiome, season, weather));
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
