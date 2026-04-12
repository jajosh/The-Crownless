using MyGame.Controls;
using System;

namespace The_Game;

public enum TrapType { }
public interface TileComponent { }

public record IsRoofedComponent(
    bool IsRoofed
) : TileComponent;
public record CuttablePlantComponent(
    Dictionary<int, int>/*weight, item ID*/ ITemObjectID, int Respawn,
    bool IsCuttable,
    List<TileRenderProfile> VisualFallBack) : TileComponent;

public record HarvestablePlantComponent(
    List<ItemStackComponent> Loot, int Respawn,
    Dictionary<int, int> HarvestableItem,               // itemId → amount
    List<TileRenderProfile> VisualFallBack) : TileComponent;

public record TileInventoryComponent(
    int ITemObjectID,
    List<ItemStackComponent> HeldItems) : TileComponent;

public record IsFlammableComponent(
    int ITemObjectID,
    bool IsFlammable, 
    int BurnAmount,
    List<TileRenderProfile> VisualFallBack) : TileComponent;

public record IsWalkableComponent(
    bool IsWalkable,
    float WalkabilityCost) : TileComponent;

public record CoverComponent(
    bool IsCover,
    CoverGrade Cover) : TileComponent;
public record DestructibleComponent(
    HealthComponent Health,
    Dictionary<DamageTypes, int >/*Damage type, resistance value*/ Resistances,
    Dictionary<DamageTypes, int >/*Damage type, Vulerbilities value*/ Vulerbilities) : TileComponent;
public record OpenableComonent(
    bool isOpen,
    bool CanBeLocked,
    bool CanBeLockedPicked,
    bool RequiresKey,
    int LockPickDC) : TileComponent;
public record ChestComponent(
    InventoryComponent Inventory,
    bool AccessToRandomDropTable,
    LootTableCatigory Table) : TileComponent;
public record TrapComponent(
    int DamageDie,
    int DamageDieAmount,
    TrapType Type) : TileComponent;
public record Respawnable(
    ) : TileComponent;
public record TiledEffectComponent(
    int EffectID,
    int Interval) : TileComponent;
