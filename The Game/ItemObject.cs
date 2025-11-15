using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using The_Game; // For IItem

public enum Rarity { Common, Uncommon, Rare, Epic, Legendary }
public class Item
{
    public string? AsciiKey { get; set; }
    public string Name { get; set; } = string.Empty;
    public int ID { get; set; }
    public int SpawnChance { get; set; }
    public int MaxStack { get; set; }
    public int? Price { get; set; }
    public bool CanStore { get; set; }
    public int? MaxDurability { get; set; }
    public int? CurrentDurability { get; set; }
    public float Weight { get; set; }
    public Rarity Rarity { get; set; }
    public HashSet<string> Tags { get; set; } = new();

    public List<string>? LoreText { get; set; }
    public List<string> EncumbranceErrorMessages { get; set; } = new();

    // New component-based storage
    public Dictionary<string, IItemComponent> Components { get; set; } = new();

    [JsonIgnore]
    public Dictionary<TriggerEnum, ActionObject> TriggerData { get; } = new();

    public List<TriggerConfig>? TriggerRawData { get; set; }

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


    // Use records for auto-cloning (C# 9+)
    public record WeaponDataFrame(
        int? DamageDie,
        Skill DamageBonusSkill,
        DamageTypes DamageType
    );
    public object Clone()
    {
        // Shallow copy for now; extend for deep if needed
        return MemberwiseClone();
    }
}
