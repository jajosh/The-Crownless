using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using MyGame.Controls;
using The_Game;


// The Tile object
public class TileObject
{
    // === 🧩 Tile Core Identity & Rendering ===
    public int TileId { get; set; }
    public TileRenderProfile BaseRender { get; set; }

    public TileTypes TileType { get; set; }

    // === 🗺️ Locational Data (RootComponent Coordinates) ===
    public int RootGridX { get; set; }
    public int RootGridY { get; set; }
    public int RootLocalX { get; set; }
    public int RootLocalY { get; set; }
    [NotMapped]
    public RootComponent Root
    {
        get => new RootComponent(RootGridX, RootGridY, RootLocalX, RootLocalY);
        set
        {
            RootGridX = value.GridX;
            RootGridY = value.GridY;
            RootLocalX = value.LocalX;
            RootLocalY = value.LocalY;
        }
    }


    // === ⚙️ Behavior and Interaction Components ===
    [NotMapped] public List<TileComponents> Components { get; set; }// Separate Table
    [NotMapped] public List<TileProperties> Properties { get; set; } // Separate Table
    public List<TileEffectState> Effects { get; set; } // JsonBlob
    public List<TileTriggerActions> TriggerActions { get; set; } // JsonBlob
    public CoverGrade Cover { get; set; } = CoverGrade.none;

    // === 📝 State and Deferred Processing ===
    [NotMapped] public List<TileCheckType> DeferredChecks { get; set; } // initial Tile processing only, not needed in game
    [NotMapped] public List<DescriptionEntry> Description { get; set; }// Separate Table
    [NotMapped]
    public ICharacter? Occupant
    { get; set; }


    // Used for tile processing. 
    public TileObject(int gridX, int gridY, int localX, int localY, TileTypes tileType, bool isWalkable, bool isRoofed, List<DescriptionEntry> description, string asciiToShow) : this()
    {
        RootGridX = gridX;
        RootGridY = gridY;
        RootLocalX = localX;
        RootLocalY = localY;
        TileType = tileType;

        // Initialize BaseRender Profile
        BaseRender = new TileRenderProfile(asciiToShow);
        BaseRender.CharData.MainChar = asciiToShow;

        Description = description ?? new List<DescriptionEntry>();
    }
    public TileObject()
    {
        // Initialize all list properties to prevent NullReferenceException
        BaseRender = new TileRenderProfile("F");
        Components = new List<TileComponents>();
        Properties = new List<TileProperties>();
        Effects = new List<TileEffectState>();
        TriggerActions = new List<TileTriggerActions>();
        DeferredChecks = new List<TileCheckType>();
        Description = new List<DescriptionEntry>();
    }

    // Adds a description
    public void AddDescription(int weight, string text, GridBiomeType? biome = null, GridBiomeSubType? subBiome = null, SeasonData? season = null, WeatherData? weather = null)
    {
        Description.Add(new DescriptionEntry(
            descriptionWeight: weight,
            textEntry: text,
            biome: biome,
            subBiome: subBiome,
            season: season,
            weather: weather));
    }
    public bool IsBurned()
    {
        foreach (var tileProperties in Properties)
        {
            if (tileProperties.TileProperty == TileProperty.Burned)
                return true;
        }
        return false;
    }
}