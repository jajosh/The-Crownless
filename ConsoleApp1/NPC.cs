using Newtonsoft.Json;
using System;
using Windows.Media.Playback;

public class NPCData
{
    public List<NPCType> TypeNPCDehydrated = new();
    public List<NPC> NamedNPCDehydrated = new();
    [JsonIgnore]
    public List<NPCType> TypeNPCHyraded= new();
    [JsonIgnore]
    public List<NPC> NamedNPCHydrated = new();


    // Hydration method (stub)
    public List<NPC> Hydrate(List<NPC> npc)
    {
        foreach (var NPC in npc)
        {
            if (NPC.TriggerData != null)
            {
                foreach (var kvp in NPC.TriggerData)
                {
                    switch (kvp.Value)
                    {
                        case ""
                    }
                }
            }
        }
    }
    public static List<NPC> DeHydrate()
    {
        return new List<NPC>();
    }
    public static NPCData ProcessNPCData()
    {
        NPCData rawData = JsonLoader.LoadFromJson<NPCData>(FileManager.TheNPCFilePath);
        rawData.Hydrate(rawData.NPCRawData);
        
    }
}
public class NPCType
{
    public int ID { get; set; }
    public string RaceName { get; set; } = string.Empty;

    public int HP { get; set; }
    public int MP { get; set; }
    public int TileSpeed { get; set; }
    public CreatureType Type { get; set; }
    public CreatureSubType SubType { get; set; }

    public Dictionary<Skill, int> BaseStats { get; set; } = new();

    public List<Languages>? Languages { get; set; }
    public List<DamageTypes>? DamageResistance { get; set; }
    public List<DamageTypes>? DamageImmunities { get; set; }
    public List<Conditions>? ConditionImmunities { get; set; }

    public List<EngineRandomDialog>? RandomDialog { get; set; } // Null if using global list
    public List<string>? DefaultNames { get; set; }

    public Dictionary<Triggers, string>? TriggerData { get; set; }
    [JsonIgnore]
    public Dictionary<Triggers, Action>? TriggerActions { get; set; } = new();
    public List<string> PossibleCombatSymbols { get; set; } = new();
}

public class NPC
{
    //Not included in NPCType or needs to be reference separetlly. Really just stuff that isnt going to be overridden.
    public int ID { get; set; }
    public string Name { get; set; }

    public int TypeID { get; set; }

    public string Race { get; set; }
    public Dictionary<string, string> Aliases { get; set; } = new();

    public int CurrentHP { get; set; }
    public int CurrentMP { get; set; }

    // Overrides from base type
    public int TileSpeed { get; set; }
    public int MaxHP { get; set; }
    public int MaxMP { get; set; }
    public List<Languages>? Languages { get; set; }
    public List<DamageTypes>? DamageResistance { get; set; }
    public List<DamageTypes>? DamageImmunities { get; set; }
    public List<Conditions>? ConditionImmunities { get; set; }
    public List<EngineRandomDialog>? RandomDialog { get; set; }
    public string ascii { get; set; }

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
    public Dictionary<Triggers, Action> TriggerActions { get; set; }
    public NPC()
    {

    }
}

public class AI
{
    public Action NextAction { get; set; }
    
    public AI()
    {

    }
}

