using MyGame.Controls;
using System;
using System.ComponentModel.DataAnnotations.Schema;
public enum TileCheckType
{
    None = 0,
    NeighborRoofed = 1 << 0,
    NeighborWalkable = 1 << 1,
    CheckTriggerCoordinets = 1 << 2
    // Add more as needed
}
public enum TileProperty
{
    Burned
}
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
public static class TileHelpers
{
    private static readonly (int dx, int dy)[] NeighborOffsets =
    {
        (-1, 0), (1, 0), (0, -1), (0, 1) // 4-directional
    };

    public static bool HasRoofedNeighbor(MapManager map, TileObject tile)
    {
        return false;
    }

    public static bool HasWalkableNeighbor(MapManager map, TileObject tile)
    {
        return false;
    }
}
public class TileComponents
{
    // Tile ID
    public int TileID { get; set; }
    public TileComponent TileComponent { get; set; }
}
public class TileProperties
{
    // Tile ID
    public int TileID { get; set; }
    public TileProperty TileProperty { get; set; }
}
public class TileRenderProfile
{
    // Visual Look
    public MyGame.Controls.ColorTextBox.CharData CharData { get; set; }
    public MyGame.Controls.ColorTextBox.OverlayStep Overlay { get; set; }
    public TileRenderProfile()
    {
        CharData = new ColorTextBox.CharData();
        Overlay = new ColorTextBox.OverlayStep();
    }
}