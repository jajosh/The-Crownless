using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

public class Grid
{
    public List<string> GridMap { get; set; } = new();
    public int XPosition { get; set; }
    public int YPosition { get; set; }
    public GridBiomeType Biome { get; set; }
    public GridBiomeSubType SubBiome { get; set; }
    public int RandomEventChance { get; set; }
    public List<DescriptionEntry> DescriptionEntries { get; set; } = new();

    public Grid() { }

    /// <summary>
    /// Adds a description entry to the grid.
    /// Null values act as "any".
    /// </summary>
    public void AddDescription(
        int weight,
        string text,
        GridBiomeType? biome = null,
        GridBiomeSubType? subBiome = null,
        SeasonData? season = null,
        WeatherData? weather = null)
    {
        DescriptionEntries.Add(new DescriptionEntry(
            weight, text, biome, subBiome, season, weather));
    }
    /// <summary>
    /// Selects a random description for a new grid based on weighted entries.
    /// </summary>
    /// <param name="grid">The current grid.</param>
    /// <param name="player">The player, used for condition matching.</param>
    /// <returns>A description string, or null if none matched.</returns>
    public static string NewGridDescription(Grid grid, WeatherEngine weather)
    {
        var possible = grid.DescriptionEntries
            .Where(d => d.Matches(weather, grid.Biome, grid.SubBiome))
            .ToList();

        if (possible.Count == 0) return null;

        int totalWeight = possible.Sum(d => d.Weight);
        int roll = Random.Shared.Next(totalWeight);

        foreach (var entry in possible)
        {
            if (roll < entry.Weight)
                return entry.ToString();
            roll -= entry.Weight;
        }
        string result = possible.Last().ToString();
        return result; // fallback
    }
}
public class Tile
{
    public string? AsciiToShow { get; set; }
    public int GridX { get; set; }
    public int GridY { get; set; }
    public int LocalX { get; set; }
    public int LocalY { get; set; }

    public TileTypes TileType { get; set; }
    public bool IsWalkable { get; set; }
    public bool IsRoofed { get; set; }
    public TileTriggeractions Triggeractions { get; set; }

    // Checks if the tile needs to be processed after surrounding tiles have been processed
    public TileCheckType DeferredChecks { get; set; } = TileCheckType.None;
    // A list of descriptions for each tile type based on the local grid's biome and sub biome
    public List<DescriptionEntry> Description { get; set; }


    public Tile()
    {

    }
    public Tile(int gridX, int gridY, int localX, int localY, TileTypes tileType, bool isWalkable, bool isRoofed, List<DescriptionEntry> description, string asciiToShow)
    {
        GridX = gridX;
        GridY = gridY;
        LocalX = localX;
        LocalY = localY;
        TileType = tileType;
        IsWalkable = isWalkable;
        Description = description;
        AsciiToShow = asciiToShow;
    }
    public void AddDescription(int weight,string text,GridBiomeType? biome = null,GridBiomeSubType? subBiome = null, SeasonData? season = null,WeatherData? weather = null)
    {
        Description.Add(new DescriptionEntry(
            weight: weight,
            text: text,
            biome: biome,
            subBiome: subBiome,
            season: season,
            weather: weather));
    }
    /// <summary>
    /// Selects a random description for a tile, weighted by matching conditions.
    /// </summary>
    /// <param name="tile">The current tile.</param>
    /// <param name="player">The player used for condition checks.</param>
    /// <param name="CurrentBiome">The biome type of the grid.</param>
    /// <param name="CurrentSubBiome">The biome subtype of the grid.</param>
    /// <returns>A descriptive string, or null if none matched.</returns>
    public static string PickADescription(Tile tile, WeatherEngine weather, GridBiomeType CurrentBiome, GridBiomeSubType CurrentSubBiome)
    {
        Random random = new Random();
        var possible = tile.Description
                .Where(d => d.Matches(weather, CurrentBiome, CurrentSubBiome) || d.Matches(weather, CurrentBiome) || d.Matches(weather, CurrentSubBiome))
                .ToList();

        if (possible.Count > 0)
        {
            // Weighted random pick
            int totalWeight = possible.Sum(d => d.Weight);
            int roll = random.Next(totalWeight);
            DescriptionEntry chosen = null;

            foreach (var entry in possible)
            {
                if (roll < entry.Weight)
                {
                    chosen = entry;
                    break;
                }
                roll -= entry.Weight;
            }

            return chosen.Text;
        }
        return null;
    }
}

/// <summary>
/// Allows the Grid Tile Description to hold either BridBiomeType or GridBiomeSubType
/// </summary>
public struct BiomeRef
{
    //var desc1 = new BiomeRef(GridBiomeType.Forest);
    //var desc2 = new BiomeRef(GridBiomeSubType.DarkForest);
    //var desc3 = BiomeRef.Any;  // matches everything
    public GridBiomeType? Main { get; }
    public GridBiomeSubType? Sub { get; }
    public bool IsAny { get; }   // <--- wildcard flag

    // Constructors
    public BiomeRef(GridBiomeType type)
    {
        Main = type;
        Sub = null;
        IsAny = false;
    }

    public BiomeRef(GridBiomeSubType subtype)
    {
        Main = null;
        Sub = subtype;
        IsAny = false;
    }

    private BiomeRef(bool any)   // special ctor for Any
    {
        Main = null;
        Sub = null;
        IsAny = any;
    }

    public static BiomeRef Any => new BiomeRef(true);

    public override string ToString()
    {
        if (IsAny) return "Any";
        return Main?.ToString() ?? Sub?.ToString() ?? "Unknown";
    }
}
public class DescriptionEntry
{
    public int Weight { get; set; }
    public string Text { get; set; }

    // Use nullable enums to allow "Any"
    public GridBiomeType? Biome { get; set; } = null; // null = any
    public GridBiomeSubType? SubBiome { get; set; } = null; // null = any
    public SeasonData? Season { get; set; } = null; // null = any
    public WeatherData? Weather { get; set; } = null; // null = any

    public DescriptionEntry() { } // Needed for JSON deserialization

    public DescriptionEntry(int weight, string text, GridBiomeType? biome = null, GridBiomeSubType? subBiome = null,
                            SeasonData? season = null, WeatherData? weather = null)
    {
        Weight = weight;
        Text = text;
        Biome = biome;
        SubBiome = subBiome;
        Season = season;
        Weather = weather;
    }
   

    // Checks if this entry applies to a given player and biome
    public bool Matches(WeatherEngine weather, GridBiomeType currentBiome, GridBiomeSubType currentSubBiome)
    {
        bool biomeMatch = !Biome.HasValue || Biome.Value == currentBiome;
        bool subBiomeMatch = !SubBiome.HasValue || SubBiome.Value == currentSubBiome;
        bool seasonMatch = !Season.HasValue || Season.Value == weather.Seasons;
        bool weatherMatch = !Weather.HasValue || Weather.Value == weather.Weather;

        return biomeMatch && subBiomeMatch && seasonMatch && weatherMatch;
    }
    public bool Matches(WeatherEngine weather, GridBiomeType currentBiome)
    {
        bool biomeMatch = !Biome.HasValue || Biome.Value == currentBiome;
        bool seasonMatch = !Season.HasValue || Season.Value == weather.Seasons;
        bool weatherMatch = !Weather.HasValue || Weather.Value == weather.Weather;

        return biomeMatch && seasonMatch && weatherMatch;
    }
    public bool Matches(WeatherEngine weather, GridBiomeSubType currentSubBiome)
    {
        bool subBiomeMatch = !SubBiome.HasValue || SubBiome.Value == currentSubBiome;
        bool seasonMatch = !Season.HasValue || Season.Value == weather.Seasons;
        bool weatherMatch = !Weather.HasValue || Weather.Value == weather.Weather;

        return subBiomeMatch && seasonMatch && weatherMatch;
    }
    
}

