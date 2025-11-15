using System;
using System.Text;
using The_Game;
using System.Drawing;
using System.Windows.Forms;
using MyGame.Controls;
using System.Xml;

public enum TileTypes
{
    empty,
    Grass,
    Tree,
    Forest,
    DirtPath,
    StoneBricks,
    Wall,
    Window,
    Pew,
    Alter,
    Town
}

public enum TileTriggerActions
{
    SecretChest,
    PlantSpawn,
    Trader,
    Quest,
    Event,
    Door,
    Combat
}

public enum CoverGrade
{
    full,
    half,
    quarter,
    none
}
public class TileState
{

    // Visual Look
    public MyGame.Controls.ColorTextBox.CharData CharData { get; set; }
    public MyGame.Controls.ColorTextBox.OverlayStep Overlay { get; set; }
}
// The Tile object
public class TileObject
{
    // Visual Look
    public MyGame.Controls.ColorTextBox.CharData CharData { get; set; }
    public MyGame.Controls.ColorTextBox.OverlayStep Overlay { get; set; }

    // Locational Data
    public RootComponent Root { get; set; } = new RootComponent(0, 0, 0, 0);

    // Meta Data
    public List<ItemStackComponent> Items { get; set; } // For item drops and random spawned items. Like a bow sitting on a table
    public TileTypes TileType { get; set; }
    public bool IsWalkable { get; set; } = true;
    public bool IsRoofed { get; set; } = false;
    public bool IsCovered { get; set; } = false;

    public bool IsFlammable { get; set; } = true;
    public int BurnAmount { get; set; } // As a percentage
    public char? AsciiBrunedFallBack { get; set; }
    public MyGame.Controls.ColorTextBox.CharData? BurnedCharDataFallBack { get; set; } // The fall back chardata if the tile is burned to 100 percent
    public MyGame.Controls.ColorTextBox.OverlayStep BurnedCharDataOverLayFallBack { get; set; }

    //  === Plant Life Related info === 
    public bool IsVegitation { get; set; } = false;
    public bool IsHarvestable { get; set; } = false;
    public Dictionary<int, int>? HarvestableItem { get; set; } // item ID, weight
    public bool IsCutable { get; set; }
    public MyGame.Controls.ColorTextBox.CharData? CutdownCharDataFallBack { get; set; } // NewCharData for when the tile is "cut" down
    public MyGame.Controls.ColorTextBox.OverlayStep CutdownCharDataOverlayFallBack { get; set; }

    // === Storage/Container Items ===
    public InventoryComponent? ContainerInventory { get; set; }
    public int ContainerCapacity { get; set; }

    // === Item Progression ===
    public int XP { get; set; }
    public int Level { get; set; }

    // === Time and weather Based Interactions ===
    public DateTime? ExpiresAt { get; set; }
    public WeatherData WeatherInteraction { get; set; }

    public TileTriggerActions Triggeractions { get; set; }
    public CoverGrade cover { get; set; } = CoverGrade.none;

    // Checks if the tile needs to be processed after surrounding tiles have been processed
    public TileCheckType DeferredChecks { get; set; } = TileCheckType.None;

    // A list of descriptions for each tile type based on the local grid's biome and sub biome
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
        CharData.Char = asciiToShow;
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
}