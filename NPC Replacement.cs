using System;

public class NPCType
{
    public int ID { get; set; }
    public string RaceName { get; set; } = string.Empty;

    public int HP { get; set; }
    public int MP { get; set; }
    public CreatureType Type { get; set; }
    public CreatureSubType SubType { get; set; }

    public Dictionary<Skill, int> BaseStats { get; set; } = new();

    public List<Languages>? Languages { get; set; }
    public List<DamageTypes>? DamageResistance { get; set; }
    public List<DamageTypes>? DamageImmunities { get; set; }
    public List<Conditions>? ConditionImmunities { get; set; }

    public List<RandomDialog>? RandomDialog { get; set; } // Null if using global list
    public List<string>? DefaultNames { get; set; }

    public Dictionary<Triggers, string>? TriggerData { get; set; }
    [JsonIgnore]
    public Dictionary<Triggers, Action> TriggerActions { get; set; } = new();
}

public class NPC
{
    public int ID { get; set; }
    public string Name { get; set; } = string.Empty;

    public int TypeID { get; set; }
    public int Type { get; set; } // This is the NPTType ID. 

    public string Race => Type?.RaceName ?? "Unknown";
    public Dictionary<string, string> Aliases { get; set; } = new();

    public int CurrentHP { get; set; }
    public int CurrentMP { get; set; }

    // Overrides from base type
    public List<Languages>? Languages { get; set; }
    public List<DamageTypes>? DamageResistance { get; set; }
    public List<DamageTypes>? DamageImmunities { get; set; }
    public List<Conditions>? ConditionImmunities { get; set; }
    public List<RandomDialog>? RandomDialog { get; set; }

    public bool isAlive { get; set; } = true;
    public bool UseStaticRandomDialog { get; set; }

    // Inventory & Equipment
    public List<int> InventoryData { get; set; } = new();
    [JsonIgnore]
    public List<Item> Inventory { get; set; } = new();

    public List<int> ArmorData { get; set; } = new();
    [JsonIgnore]
    public List<Item> Armor { get; set; } = new();

    public Dictionary<Triggers, string>? TriggerData { get; set; }
    [JsonIgnore]
    public Dictionary<Triggers, Action> TriggerActions { get; set; } = new();

    // Hydration method (stub)
    public static List<NPC> Hydrate(SaveGame saveGame)
    {
        // Example structure
        List<NPC> rawData = JsonLoader.LoadFromJson<List<NPC>>(FileManager.NPCSaveFile);
        // map Inventory, Armor, Triggers, etc. here
        return rawData;
    }
}

public class NPCActions
{
    public static readonly Dictionary<string, Action> ActionLookup = new();
    public static void CorruptingTouch()
    {

    }
    public static void HorrifiyingVisage()
    {

    }
    public static void Wail()
    {

    }
}

public class RandomDialog
{
    public WeatherData? weatherMain { get; set; }
    public SeasonData? seasonMain { get; set; }
    public GridBiomeType? biomeMain { get; set; }
    public GridBiomeSubType? biomeSub { get; set; }
    public CreatureType? creatureType { get; set; }
    public bool isBattleDialog { get; set; }
    public List<string> text = new();
    public List<RandomDialog> RandomDialogList()
    {
        List<RandomDialog> rawData = JsonLoader.LoadFromJson<List<RandomDialog>>(FileManager.RandiomEnvironmentalDialog);
        return rawData;
    }
}