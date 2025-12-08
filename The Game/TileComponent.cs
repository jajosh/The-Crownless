using MyGame.Controls;
using System;

public interface TileComponent { }
public record CuttablePlantComponent(
    Dictionary<int, int>/*weight, item ID*/ ITemObjectID, int Respawn,
    bool IsCuttable,
    List<ColorTextBox.CharData> CutDownCharDataFallBack,
    List<ColorTextBox.OverlayStep> CutDownOverlayFallBack
) : TileComponent;

public record HarvestablePlantComponent(
    List<ItemStackComponent>, int Respawn,
    Dictionary<int, int> HarvestableItem,               // itemId → amount
    List<ColorTextBox.CharData> HarvestCharDataFallBack,
    List<ColorTextBox.OverlayStep> HarvestOverlayFallBack
) : TileComponent;

public record TileInventoryComponent(
    int ITemObjectID,
    List<ItemStackComponent> HeldItems) : TileComponent;

public record IsFlammableComponent(
    int ITemObjectID,
    bool IsFlammable, 
    int BurnAmount, 
    List<ColorTextBox.CharData> BurnedCharFallBack,
    List<ColorTextBox.OverlayStep> BurnedOverLayFallBack) : TileComponent;

public record Walkable(
    bool IsWalkable,
    bool IsRoofed,
    int MovementPenalty) : TileComponent;

public record Cover(
    bool IsCover,
    CoverGrade Cover) : TileComponent;
