
using Newtonsoft.Json;
using System;
using The_Game;

public enum CreatureType
{
    Undead
}
public enum CreatureSubType
{
    Spirit,

}
public enum AlignmentChart
{
    LawfulGood,
    LawfulNeutral,
    LawfulEvil,
    NeutralGood,
    NeutralNeutral,
    NeutralEvil,
    ChaoticGood,
    ChaoticNeutral,
    ChaoticEvil
}
public record NPCTypeObject
{
    #region Identity
    public int ID { get; set; }
    public string RaceName { get; set; } = string.Empty;
    #endregion

    #region Classification
    public CreatureType Type { get; set; }
    public CreatureSubType SubType { get; set; }
    public List<AlignmentChart>? Alignments { get; set; }

    #endregion

    #region Stats
    public int MaxHP { get; set; }
    public int MaxMP { get; set; }
    public int TileSpeed { get; set; }
    public SkillComponent Skills { get; set; }
    #endregion

    #region Abilities & Traits
    // Just a list of variables that the NPC has, logic is elsewhere. 
    public List<Languages>? Languages { get; set; }
    public List<DamageTypes>? DamageResistance { get; set; }
    public List<DamageTypes>? DamageImmunities { get; set; }
    public List<Conditions>? ConditionImmunities { get; set; }
    #endregion

    #region Dialog
    /// <summary>
    /// Null means "fall back to global list".
    /// </summary>
    public List<RecordRandomText>? RandomDialog { get; set; }
    #endregion

    #region Naming
    public List<string>? DefaultNames { get; set; }
    #endregion

    #region Triggers
    public List<TriggerConfig>? TriggerKey { get; set; } = new();
    [JsonIgnore]
    public List<ActionObject> TriggerData { get; set; } = new();

    [JsonIgnore]
    public InventoryComponent? Inventory { get; set; }
    public List<int>? InventoryKey { get; set; }
    #endregion

    #region Combat
    public List<string>? PossibleCombatSymbols { get; set; } = new();
    #endregion

    #region Utilities
    // Utility: lookup by ID
    public static NPCTypeObject? GetNPCTypeByID(int id)
    {
        return NPCTypeManager.NPCTypes.Find(t => t.ID == id);
    }

    #endregion
    public NPCTypeObject()
    {
        //// Identity
        //ID = 0;
        //RaceName = string.Empty;

        //// Classification
        //Type = default;
        //SubType = default;
        //Alignments = new List<AlignmentChart>();

        //// Stats
        //MaxHP = 0;
        //MaxMP = 0;
        //TileSpeed = 0;
        //Skills = new SkillsCompanent();

        //// Abilities & Traits
        //Languages = new List<Languages>();
        //DamageResistance = new List<DamageTypes>();
        //DamageImmunities = new List<DamageTypes>();
        //ConditionImmunities = new List<Conditions>();

        //// Dialog
        //RandomDialog = new List<ObjectRandomText>();

        //// Naming
        //DefaultNames = new List<string>();

        //// Triggers
        //TriggerData = new Dictionary<ActionType, GameActionObject>();

        //// Combat
        //PossibleCombatSymbols = new List<string>();
    }
}