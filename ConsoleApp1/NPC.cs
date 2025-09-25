using GameNamespace;
using Newtonsoft.Json;
using System;
using Windows.Media.Playback;


public class NPCType
{

    public int ID { get; set; }
    public string RaceName { get; set; } = string.Empty;

    public List<AlignmentChart>? Alignments { get; set; }

    public int MaxHP { get; set; }
    public int MaxMP { get; set; }
    public int TileSpeed { get; set; }

    public CreatureType Type { get; set; }
    public CreatureSubType SubType { get; set; }

    public Dictionary<Skill, int> BaseStats { get; set; } = new();

    public List<Languages>? Languages { get; set; }
    public List<DamageTypes>? DamageResistance { get; set; }
    public List<DamageTypes>? DamageImmunities { get; set; }
    public List<Conditions>? ConditionImmunities { get; set; }

    // Null means "fall back to global list"
    public List<EngineRandomText>? RandomDialog { get; set; }

    public List<string>? DefaultNames { get; set; }

    public List<string>? TriggerRawData { get; set; } = new();

    [JsonIgnore]
    public Dictionary<TriggerEnum, GameAction> TriggerData { get; set; } = new();

    public List<string>? PossibleCombatSymbols { get; set; } = new();
    // Utility: lookup by ID
    public static NPCType? GetNPCTypeByID(int id)
    {
        return FileManager.NPCTypes.Find(t => t.ID == id);
    }
}

public class NPC : IActionable
{

    public void ApplyDamage(int amount)
    {
        CurrentHP -= amount;
    }
    //Not included in NPCType or needs to be reference separetlly. Really just stuff that isnt going to be overridden.
    public int ID { get; set; }
    public string Name { get; set; }

    public int TypeID { get; set; }

    public int Gridx { get; set; }
    public int GridY { get; set; }
    public int LocalX { get; set; }
    public int LocalY { get; set; }
    public List<(int GridX, int GridY, int LocalX, int LocalY)>? Waypoints { get; set; }
    public int? CurrentWaypointIndex { get; set; }
    public CreatureType Race { get; set; }
    public Dictionary<string, string> Aliases { get; set; } = new();

    public int CurrentHP { get; set; }
    public int CurrentMP { get; set; }

    // Overrides from base type
    public int? TileSpeed { get; set; }
    public int? MaxHP { get; set; }
    public int? MaxMP { get; set; }
    public Dictionary<Skill, int>? BaseStats { get; set; } = new();
    public List<Languages>? Languages { get; set; }
    public List<DamageTypes>? DamageResistance { get; set; }
    public List<DamageTypes>? DamageImmunities { get; set; }
    public List<Conditions>? ConditionImmunities { get; set; }
    public List<EngineRandomText>? RandomDialog { get; set; }
    // The ascii that will be used for this NPC. Overrides default.
    public string? ascii { get; set; }

    // True if NPC is alive. 
    public bool isAlive { get; set; } = true;
    // True if the NPC also has access to static radnom dialog
    public bool UseStaticRandomDialog { get; set; }

    // Inventory & Equipment
    public List<int> InventoryData { get; set; } = new();
    [JsonIgnore]
    public List<Item> Inventory { get; set; } = new();

    public List<int> ArmorData { get; set; } = new();
    [JsonIgnore]
    public List<Item> Armor { get; set; } = new();

    // Key of 
    public Dictionary<TriggerKey, string>? TriggerData { get; set; }
    [JsonIgnore]
    public Dictionary<Triggers, Action>? CombatTriggers { get; set; } = new();
    [JsonIgnore]
    public Dictionary<Triggers, Action>? SocialTriggers { get; set; } = new();
    [JsonIgnore]
    public int? Inititive { get; set; }
    
    public NPC()
    {

    }
}