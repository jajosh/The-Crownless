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
public enum PrimaryType { Weapon, Food, Potion,  } //  Item Base Type
public enum SecondaryType { ShortSword } // Item Specific type
public enum Rarity { Common, Uncommon, Rare, Epic, Legendary }


public class ItemObject
{
    // Identity
    public int ID { get; set; }
    public string? AsciiKey { get; set; }
    public string Name { get; set; } = string.Empty;

    // Basic Attributes
    public float Weight { get; set; }
    public Rarity Rarity { get; set; } //

    // Item Type Infor
    public PrimaryType PrimaryType { get; set; }// E.G. Weapon, Food
    public SecondaryType? SecondaryType { get; set; } // E.G. ShortSword
    public PrimaryMaterial? material { get; set; } // E.G. Wood, stone, organic
    public SecondaryMaterial? SecondaryMaterial { get; set; } // E.G. Item inlays. Like a golden hand guard on a sword
   

    // Economy / Inventory
    public int MaxStack { get; set; }
    public int Price { get; set; } // Base Sell Price
    public bool CanStore { get; set; } // 0 1, 0 = true

    // Text / Display --- Saved as a JsonBlob
    public List<DescriptionEntry>? LoreText { get; set; }
    public List<DescriptionEntry> EncumbranceErrorMessages { get; set; } = new();

    // Meta Data
    [NotMapped]public List<IItemComponent> Components { get; set; } // Separate DB table
    public Dictionary<TriggerEnum, ActionObject> TriggerData { get; set; } // JsonBlob

    

    
    public object Clone()
    {
        // Shallow copy for now; extend for deep if needed
        return MemberwiseClone();
    }
}
