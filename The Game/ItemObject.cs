using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using The_Game; // For IItem

public enum EquipmentSlots
{
    // Armor
    Head,
    Chest,
    Waist,
    Legs,
    Feet,
    Arms,
    Hands,
    Wrists,
    Shoulders,

    // Accessories
    Neck,
    Ring,
    Bracelet,
    Trinket,

    // Weapons
    MainHand,
    OffHand,
    Back,
    LowerBack,
    Side,


    // Miscellaneous / Custom
    Cap,
    Belt,
    Tabard
}
[Flags]
public enum Properties : long
{
    None = 0,
    Flammable = 1L << 0,   // 1
    Heatable = 1L << 1,   // 2
    WaterSoluble = 1L << 2,   // 4
    WaterProof = 1L << 3,   // 8
    // Add more later — you have room for 60+!
}
public enum PrimaryMaterial { Wood, Leather, Metal }
public enum SecondaryMaterial { Wood, Leather, Iron, Steel, Adamantian }
public enum PrimaryType { Sword, } //  For Craftable items
public enum SecondaryType { ShortSword }
public enum Rarity { Common, Uncommon, Rare, Epic, Legendary }


public class ItemObject
{
    // Identity
    public int ID { get; set; }
    public string? AsciiKey { get; set; }
    public string Name { get; set; } = string.Empty;

    // Basic Attributes
    public float Weight { get; set; }
    public Rarity Rarity { get; set; }
    [NotMapped]public List<ItemTags> Tags { get; set; }


    public PrimaryType PrimaryType { get; set; }
    // Single slot items
    public SingleEquipmentSlots? SingleSlot { get; set; }
    // Multi slot items (bitmask)
    public MultiEquipmentSlots MultiSlots { get; set; }
    // Helper: Get all valid slots for this item
    public PrimaryMaterial material { get; set; }
    public SecondaryMaterial SecondaryMaterial { get; set; }
    [NotMapped]
    public IEnumerable<MultiEquipmentSlots> ValidMultiSlots
    {
        get
        {
            if (MultiSlots == MultiEquipmentSlots.None) yield break;
            for (int i = 0; i < 64; i++)
            {
                var slot = (MultiEquipmentSlots)(1L << i);
                if (MultiSlots.HasFlag(slot))
                    yield return slot;
            }
        }
    }

    // Economy / Inventory
    public int MaxStack { get; set; }
    public int SpawnChance { get; set; }
    public int? Price { get; set; }
    public bool CanStore { get; set; }

    // Text / Display
    public List<RecordRandomText>? LoreText { get; set; }
    public List<RecordRandomText> EncumbranceErrorMessages { get; set; } = new();

    // Component-Based System
    private List<ItemComponentDictionaryEntry> _entries = new();
    [NotMapped]public Dictionary<string, IItemComponent> Components => _entries.ToDictionary(e => e.Key, e => e.Value);

    internal List<ItemComponentDictionaryEntry> Entries
    {
        get => _entries;
        set => _entries = value;
    }

    // Trigger System
    public Dictionary<TriggerEnum, ActionObject> TriggerData { get; } = new();

    public bool TryGetComponent<T>(out T component) where T : class, IItemComponent
    {
        if (Components.TryGetValue(typeof(T).Name, out IItemComponent raw))
        {
            component = (T)raw;
            return true;
        }
        component = default!;
        return false;
    }

    public void AddComponent<T>(T comp) where T : class, IItemComponent
        => Components[typeof(T).Name] = comp;
    public object Clone()
    {
        // Shallow copy for now; extend for deep if needed
        return MemberwiseClone();
    }
}
