using GameNamespace;
using System;
using System.Reflection.Metadata;
using System.Text.Json;
using System.Text.Json.Serialization;
using Windows.ApplicationModel.DataTransfer;
using Windows.Networking.Proximity;
using Windows.Networking.Sockets;







public class NPC
{
    #region Core Info
    public int ID { get; set; }
    public string Name { get; set; } = string.Empty;
    public NPCTypeData? NPCType { get; set; }
    public NPCNamedData? NPCNamed { get; set; }
    public List<string> PossibleCombatSymbols { get; set; } = new();
    public bool isAlive { get; set; }
    #endregion

    #region Creature Info
    public CreatureType? CreatureType { get; set; }
    public CreatureSubType? CreatureSubType { get; set; }
    public List<string> Languages { get; set; } = new();
    #endregion

    #region Stats & Combat
    public int? HP { get; set; }
    public int MaxHP { get; set; }
    public int? MP { get; set; }
    public int MaxMP { get; set; }
    public int XP { get; set; }
    public int AC { get; set; }
    public Dictionary<Skill, int> Skills { get; set; } = new();
    public List<DamageTypes>? DamageResistance { get; set; } = new();
    public List<DamageTypes>? DamageImmunities { get; set; }
    public List<Conditions>? ConditionImmunities { get; set; }
    #endregion


    #region Inventory & Equipment
    public List<int>? InventoryData { get; set; } = new();
    [JsonIgnore]
    public List<Item> Inventory { get; set; } = new();

    public List<int>? ArmorData { get; set; } = new();
    [JsonIgnore]
    public List<Item>? Armor { get; set; } = new();
    #endregion

    #region Dialog
    public List<string>? RandomBattleDialog { get; set; } = new() { "My dialog hasn't been set, but I'll kill you anyway" };
    [JsonIgnore]
    public List<RandomDialog>? RandomDialog { get; set; } = new();
    public bool UseStaticRandomDialog { get; set; }
    public Dictionary<Triggers, string>? TriggerData { get; set; }
    #endregion

    [JsonIgnore]
    public Dictionary<Triggers, Action> TriggerActions { get; set; } = new();

    #region Constructor
    public NPC() { }
    #endregion

    #region Nested Classes
    public class NPCNamedData
    {
        public Dictionary<Pronouns, string>? Pronouns { get; set; } = new();
        public bool? isKnown { get; set; }
        public bool? isAlive { get; set; }
    }

    public class NPCTypeData
    {
        public List<string>? PossibleNames { get; set; }
    }
    #endregion


    public static List<NPC> Hydrate()
    {
        List<NPC> rawData = JsonLoader.LoadFromJson<List<NPC>>(FileManager.TheNPCFilePath);
        List<NPC> CleanedData = new();
        foreach (var npc in rawData)
        {
            // Build TriggerActions (what you already have)
            npc.TriggerActions = new Dictionary<Triggers, Action>();
            if (npc.TriggerData != null)
            {
                foreach (var kvp in npc.TriggerData)
                {
                    if (NPCActions.ActionLookup.TryGetValue(kvp.Value, out var action))
                        npc.TriggerActions[kvp.Key] = action;
                }
            }

            // 🔹 Map Inventory
            if (npc.InventoryData != null)
            {
                var inventoryItems = new List<Item>();
                foreach (var itemId in npc.InventoryData)
                {
                    Item item = Item.FindItemByID(itemId);
                    if (item != null)
                        inventoryItems.Add(item);
                    else
                        Console.WriteLine($"Warning: Item ID {itemId} not found.");
                }
                // You could either add a new property for the "hydrated" version
                // or replace the raw Inventory list:
                // npc.InventoryItems = inventoryItems; // if you make such a property
            }

            // 🔹 Map Armor
            if (npc.ArmorData != null)
            {
                var armorItems = new List<Item>();
                foreach (var itemId in npc.ArmorData)
                {
                    Item item = Item.FindItemByID(itemId);
                    if (item != null)
                        armorItems.Add(item);
                }
                // npc.ArmorItems = armorItems; // if you make such a property
            }

            CleanedData.Add(npc);
        }

        return CleanedData;

    }
    public static List<NPC> DeHydrate()
    {
        return new List<NPC>();
    }
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
    public List<string> text = new();
    public List<RandomDialog> RandomDialogList()
    {
        List<RandomDialog> rawData = JsonLoader.LoadFromJson<List<RandomDialog>>(FileManager.RandiomEnvironmentalDialog);
        return rawData;
    }
}



