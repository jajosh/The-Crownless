using System;
using System.Text;
using The_Game;
using System.Drawing;
using System.Windows.Forms;
using MyGame.Controls;
using System.Xml;
using System.ComponentModel.DataAnnotations.Schema;


// The Tile object
public class TileObject
{
    public int TileId;
    // Saves as json
    [NotMapped]
    public TileRenderProfile TileRenderProfile { get; set; }

    // Locational Data
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

    // Meta Datarandom spawned items. Like a bow sitting on a table
    public TileTypes TileType { get; set; }
    public bool IsWalkable { get; set; } = true;
    public bool IsRoofed { get; set; } = false;
    public bool IsCovered { get; set; } = false;

    public bool IsFlammable { get; set; } = true;
    public int BurnAmount { get; set; } // As a percentage

    [NotMapped]
    public TileRenderProfile TileRenderProfileBurnFallBack { get; set; }

    [NotMapped]
    public TileRenderProfile TileRenderProfileCutDownFallBack { get; set; }


    // === Components ===
    [NotMapped]public List<TileComponents> Components { get; set; }
    [NotMapped]public List<TileProperties> Propterties { get; set; }
    // === Time and weather Based Interactions ===
    public List<TileTriggerActions> TriggerActions { get; set; }
    public CoverGrade Cover { get; set; } = CoverGrade.none;

    // Checks if the tile needs to be processed after surrounding tiles have been processed

    [NotMapped]
    public TileCheckType DeferredChecks { get; set; } = TileCheckType.None;

    // A list of descriptions for each tile type based on the local grid's biome and sub biome

    [NotMapped]
    public List<DescriptionEntry> Description { get; set; }
    // Used for tile processing. 
    public TileObject(int gridX, int gridY, int localX, int localY, TileTypes tileType, bool isWalkable, bool isRoofed, List<DescriptionEntry> description, char asciiToShow)
    {
        Root.GridX = gridX;
        Root.GridY = gridY;
        Root.LocalX = localX;
        Root.LocalY = localY;
        TileType = tileType;
        IsWalkable = isWalkable;
        Description = description;
        TileRenderProfile = new TileRenderProfile();
        TileRenderProfile.CharData.Char = asciiToShow;
        IsFlammable = false;
    }
    public TileObject()
    {

    }

    // Adds a description
    public void AddDescription(int weight, string text, GridBiomeType? biome = null, GridBiomeSubType? subBiome = null, SeasonData? season = null, WeatherData? weather = null)
    {
        Description.Add(new DescriptionEntry(
            weight: weight,
            text: text,
            biome: biome,
            subBiome: subBiome,
            season: season,
            weather: weather));
    }
    public bool IsBurned()
    {
        foreach (var tileProperties in Propterties)
        {
            if (tileProperties.TileProperty == TilePropertie.Burned)
                return true;
        }
        return false;
    }
}