using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using The_Game; // For IItem

namespace The_Game;


public enum ItemTrigger
{
    OnEquip,
    OnUnequip,
    OnHit,          // When this weapon strikes
    OnHitReceived,  // When wearing this armor and struck
    OnUse,          // Consumables, activated items
    OnPickup,
    OnDrop,
    OnTurnStart,
    OnTurnEnd,
    OnDeath,        // Cursed items, death triggers
}
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
public enum ItemProperties : long
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
public enum PrimaryType { Weapon, Food, Potion,  } //  Item Base Type
public enum SecondaryType { ShortSword } // Item Specific type
public enum Rarity { Common, Uncommon, Rare, Epic, Legendary }


public class ItemObject : ICloneable
{
    // Identity
    public int? ID { get; set; } // Auto generated when adding item to sqlite database
    public string? AsciiKey { get; set; } // Used to build the Item in conjuction to the item affix and prefix systems. For random item generation. Thsi is the base item symbol
    public string Name { get; set; } = string.Empty;

    // Basic Attributes
    public float Weight { get; set; }
    public Rarity Rarity { get; set; }

    // Item Type Infor
    public PrimaryType PrimaryType { get; set; }// E.G. Weapon, Food
    public SecondaryType? SecondaryType { get; set; } // E.G. ShortSword, LongSword
    public PrimaryMaterial? Material { get; set; } // E.G. Wood, stone, organic
    public SecondaryMaterial? SecondaryMaterial { get; set; } // E.G. Item inlays. Like a golden hand guard on a sword
    public ItemProperties Properties { get; set; }
   

    // Economy / Inventory
    public int MaxStack { get; set; }
    public int Price { get; set; } // Base Sell Price
    public bool CanStore { get; set; } // sql databass uses 0 for true

    // Text / Display --- Saved as a JsonBlob
    public List<DescriptionEntry>? LoreText { get; set; } // For examining the item
    public List<DescriptionEntry> EncumbranceErrorMessages { get; set; } = new(); // Over encumbrance messages

    // Meta Data
    [NotMapped]public List<IItemComponent> Components { get; set; } // Separate DB table
    public Dictionary<ItemTrigger, List<ActionObject>> TriggerData { get; set; } // JsonBlob

    public ItemObject()
    {
        Components = new List<IItemComponent>();
        TriggerData = new Dictionary<ItemTrigger, List<ActionObject>>();
        EncumbranceErrorMessages = new List<DescriptionEntry>();
    }


    // Explicit interface implementation (satisfies ICloneable)
    object ICloneable.Clone() => Clone();

    // Strongly typed version callers actually use
    public ItemObject Clone()
    {
        ItemObject clone = (ItemObject)MemberwiseClone();

        // Deep copy components — each component needs its own Clone()
        clone.Components = Components.Select(c => c switch
        {
            EnchantableComponent e => e with
            {
                ActiveEnchantments = new List<Enchantment>(e.ActiveEnchantments)
            },
            ArmorComponent a => a with
            {
                Invalidated = a.Invalidated?.ToList()
            },
            _ => c // All other records are safe as-is
        }).ToList();

        // Deep copy trigger data — new dictionary, new lists, new ActionObjects
        clone.TriggerData = TriggerData.ToDictionary(
            kvp => kvp.Key,
            kvp => kvp.Value
                .Select(action => (ActionObject)action.Clone())
                .ToList<ActionObject>()
        );

        // These are display/lore data — unlikely to mutate in combat
        // but cheap to copy so worth doing
        clone.LoreText = LoreText?.ToList();
        clone.EncumbranceErrorMessages = EncumbranceErrorMessages.ToList();

        return clone;
    }
}
