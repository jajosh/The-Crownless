using GameNamespace;
using Spectre.Console;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

public enum CoverGrade
{
    full,
    half,
    quarter,
    none
}


public class Tile
{
    // Visual Look
    public char? AsciiToShow { get; set; }
    public Spectre.Console.Color Color { get; set; } = Spectre.Console.Color.White;

    // Locational Data
    public int GridX { get; set; }
    public int GridY { get; set; }
    public int LocalX { get; set; }
    public int LocalY { get; set; }

    // Meta Data
    public TileTypes TileType { get; set; }
    public bool IsWalkable { get; set; }
    public bool IsRoofed { get; set; }
    public bool IsFlamable { get; set; }
    public int? BurnAmount { get; set; } // As a percentage


    public TileTriggeractions Triggeractions { get; set; }
    public CoverGrade cover { get; set; }

    // Checks if the tile needs to be processed after surrounding tiles have been processed
    public TileCheckType DeferredChecks { get; set; } = TileCheckType.None;

    // A list of descriptions for each tile type based on the local grid's biome and sub biome
    public List<DescriptionEntry> Description { get; set; }


    public Tile()
    {

    }
    public Tile(int gridX, int gridY, int localX, int localY, TileTypes tileType, bool isWalkable, bool isRoofed, List<DescriptionEntry> description, char asciiToShow)
    {
        GridX = gridX;
        GridY = gridY;
        LocalX = localX;
        LocalY = localY;
        TileType = tileType;
        IsWalkable = isWalkable;
        Description = description;
        AsciiToShow = asciiToShow;
        IsFlamable = false;
        Color = Color.White;
    }
    // Adds a description
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
    // Picks a random description based on current conditions
    public static string PickADescription(Tile tile, EngineWeather weather, GridBiomeType CurrentBiome, GridBiomeSubType CurrentSubBiome)
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
    public Tile GetTileByLocation(int GridX, int GridY, int LocalX, int LocalY)
    {
        return new Tile();
        
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
    public bool Matches(EngineWeather weather, GridBiomeType currentBiome, GridBiomeSubType currentSubBiome)
    {
        bool biomeMatch = !Biome.HasValue || Biome.Value == currentBiome;
        bool subBiomeMatch = !SubBiome.HasValue || SubBiome.Value == currentSubBiome;
        bool seasonMatch = !Season.HasValue || Season.Value == weather.Seasons;
        bool weatherMatch = !Weather.HasValue || Weather.Value == weather.Weather;

        return biomeMatch && subBiomeMatch && seasonMatch && weatherMatch;
    }
    public bool Matches(EngineWeather weather, GridBiomeType currentBiome)
    {
        bool biomeMatch = !Biome.HasValue || Biome.Value == currentBiome;
        bool seasonMatch = !Season.HasValue || Season.Value == weather.Seasons;
        bool weatherMatch = !Weather.HasValue || Weather.Value == weather.Weather;

        return biomeMatch && seasonMatch && weatherMatch;
    }
    public bool Matches(EngineWeather weather, GridBiomeSubType currentSubBiome)
    {
        bool subBiomeMatch = !SubBiome.HasValue || SubBiome.Value == currentSubBiome;
        bool seasonMatch = !Season.HasValue || Season.Value == weather.Seasons;
        bool weatherMatch = !Weather.HasValue || Weather.Value == weather.Weather;

        return subBiomeMatch && seasonMatch && weatherMatch;
    }
    
}
public class TileEffectState
{
    public int ID { get; set; }
    public string Name { get; set; } = string.Empty;
    public List<string> Description { get; set; } = new();


    // 🎨 Visuals
    public ConsoleColor? ForegroundColor { get; set; }
    public ConsoleColor? BackgroundColor { get; set; }


    // ⚔️ Effect Mechanics
    public int? DamageOverTime { get; set; }
    public DamageTypes? DamageType { get; set; }
    public int? MovementPenalty { get; set; }
    public int? Duration { get; set; }
    public int Radius { get; set; }
    public Root Root { get; set; }


    // 👁️ Environmental Interaction
    public bool? BlockVision { get; set; }


    // 💫 Conditions
    public int? ChanceToApplyCondition { get; set; }
    public Conditions? ConditionToApply { get; set; }


    // ⚙️ Behavior Flags
    public bool IsStackable { get; set; } = false;
    public bool IsSpreadable { get; set; } = false;
    public bool IsCleansable { get; set; } = true;


    // 🧠 Runtime State
    public bool IsActive { get; set; } = false;
    public TileEffectState Clone()
    {
        return new TileEffectState
        {
            ID = this.ID,
            Name = this.Name,
            Description = new List<string>(this.Description),

            ForegroundColor = this.ForegroundColor,
            BackgroundColor = this.BackgroundColor,

            DamageOverTime = this.DamageOverTime,
            DamageType = this.DamageType,
            MovementPenalty = this.MovementPenalty,
            Duration = this.Duration,
            Radius = this.Radius,
            Root = this.Root != null ? this.Root.Clone() : null, // assumes Root has Clone()

            BlockVision = this.BlockVision,

            ChanceToApplyCondition = this.ChanceToApplyCondition,
            ConditionToApply = this.ConditionToApply,

            IsStackable = this.IsStackable,
            IsSpreadable = this.IsSpreadable,
            IsCleansable = this.IsCleansable,

            IsActive = this.IsActive
        };
    }
}

