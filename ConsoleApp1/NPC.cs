using GameNamespace;
using System;
using System.Text.Json.Serialization;
using Windows.Media.Playback;


public class NPCType
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
    public SkillsCompanent Skills { get; set; }
    #endregion

    #region Abilities & Traits
    public List<Languages>? Languages { get; set; }
    public List<DamageTypes>? DamageResistance { get; set; }
    public List<DamageTypes>? DamageImmunities { get; set; }
    public List<Conditions>? ConditionImmunities { get; set; }
    #endregion

    #region Dialog
    /// <summary>
    /// Null means "fall back to global list".
    /// </summary>
    public List<ObjectRandomText>? RandomDialog { get; set; }
    #endregion

    #region Naming
    public List<string>? DefaultNames { get; set; }
    #endregion

    #region Triggers
    public List<TriggerConfig>? TriggerRawData { get; set; } = new();
    [JsonIgnore]
    public Dictionary<ActionType, GameAction> TriggerData { get; set; } = new();

    [JsonIgnore]
    public InventoryComponent? Inventory { get; set; }
    public List<int>? InventoryRawData { get; set; }
    #endregion

    #region Combat
    public List<string>? PossibleCombatSymbols { get; set; } = new();
    #endregion

    #region Utilities
    // Utility: lookup by ID
    public static NPCType? GetNPCTypeByID(int id)
    {
        return FileManager.NPCTypes.Find(t => t.ID == id);
    }

    #endregion
    public NPCType()
    {
        // Identity
        ID = 0;
        RaceName = string.Empty;

        // Classification
        Type = default;
        SubType = default;
        Alignments = new List<AlignmentChart>();

        // Stats
        MaxHP = 0;
        MaxMP = 0;
        TileSpeed = 0;
        Skills = new SkillsCompanent();

        // Abilities & Traits
        Languages = new List<Languages>();
        DamageResistance = new List<DamageTypes>();
        DamageImmunities = new List<DamageTypes>();
        ConditionImmunities = new List<Conditions>();

        // Dialog
        RandomDialog = new List<ObjectRandomText>();

        // Naming
        DefaultNames = new List<string>();

        // Triggers
        TriggerData = new Dictionary<ActionType, GameAction>();

        // Combat
        PossibleCombatSymbols = new List<string>();
    }

}


public class NPC : ICharacter
{
    #region Identity
    public int ID { get; set; }
    public string Name { get; set; }
    public int TypeID { get; set; }
    public CreatureType Race { get; set; }
    public Dictionary<string, string> Aliases { get; set; } = new();
    #endregion

    #region Positioning / Movement
    public List<(int GridX, int GridY, int LocalX, int LocalY)>? Waypoints { get; set; }
    public int? CurrentWaypointIndex { get; set; }

    public int TileSpeed { get; set; } = 6; // Default per IActionable
    public int ActionCount { get; set; } = 1;
    public int BonusActionCount { get; set; } = 1;

    public (int GridX, int GridY, int LocalX, int LocalY) GetPosition()
    {
        if (Waypoints != null && CurrentWaypointIndex.HasValue &&
            CurrentWaypointIndex.Value >= 0 && CurrentWaypointIndex.Value < Waypoints.Count)
        {
            return Waypoints[CurrentWaypointIndex.Value];
        }
        return (0, 0, 0, 0);
    }
    #endregion

    #region Components
    public HealthComponent Health { get; set; }
    public SkillsCompanent Skills { get; set; }
    public Root Root { get; set; }
    public VenderComponent? Vender { get; set; }
    #endregion

    #region Stats & Abilities

    public List<Languages>? Languages { get; set; }
    public List<DamageTypes>? DamageResistance { get; set; }
    public List<DamageTypes>? DamageImmunities { get; set; }
    public List<Conditions>? ConditionImmunities { get; set; }
    #endregion

    #region Dialog
    public List<ObjectRandomText>? RandomDialog { get; set; }
    public bool UseStaticRandomDialog { get; set; }
    public string? Ascii { get; set; }
    #endregion

    #region Inventory & Equipment
    public List<int> InventoryData { get; set; } = new();
    [JsonIgnore] public InventoryComponent Inventory { get; set; }

    public List<int> ArmorData { get; set; } = new();
    [JsonIgnore] public List<Item> Armor { get; set; } = new();
    #endregion

    #region Triggers & Actions
    public List<TriggerConfig>? TriggerRawData { get; set; } = new();
    [JsonIgnore]
    public Dictionary<ActionType, GameAction> TriggerData { get; set; } = new();

    [JsonIgnore] public int? Initiative { get; set; }
    #endregion


    public NPC()
    {
        ID = 0;
        Name = string.Empty;
        TypeID = 0;
        Root = new Root();
        Race = new CreatureType();
        Aliases = new Dictionary<string, string>();

        Waypoints = new List<(int GridX, int GridY, int LocalX, int LocalY)>();
        CurrentWaypointIndex = null;
        TileSpeed = 6;
        ActionCount = 1;
        BonusActionCount = 1;

        Health = new HealthComponent();
        Skills = new SkillsCompanent();

        Languages = new List<Languages>();
        DamageResistance = new List<DamageTypes>();
        DamageImmunities = new List<DamageTypes>();
        ConditionImmunities = new List<Conditions>();

        RandomDialog = new List<ObjectRandomText>();
        UseStaticRandomDialog = false;
        Ascii = string.Empty;

        InventoryData = new List<int>();
        ArmorData = new List<int>();
        Armor = new List<Item>();

        Initiative = null;
    }
    // --- Deep Clone ---
    public NPC Clone()
    {
        return new NPC
        {
            ID = this.ID,
            Name = string.Copy(this.Name),
            TypeID = this.TypeID,
            Root = this.Root?.Clone(),                // assumes Root has Clone()
            Race = this.Race,
            Aliases = new Dictionary<string, string>(this.Aliases),

            Waypoints = this.Waypoints != null ? new List<(int, int, int, int)>(this.Waypoints) : null,
            CurrentWaypointIndex = this.CurrentWaypointIndex,
            TileSpeed = this.TileSpeed,
            ActionCount = this.ActionCount,
            BonusActionCount = this.BonusActionCount,
            Skills = this.Skills,

            Health = this.Health?.Clone(),            // assumes HealthComponent has Clone()


            Languages = new List<Languages>(this.Languages),
            DamageResistance = new List<DamageTypes>(this.DamageResistance),
            DamageImmunities = new List<DamageTypes>(this.DamageImmunities),
            ConditionImmunities = new List<Conditions>(this.ConditionImmunities),
            RandomDialog = new List<ObjectRandomText>(this.RandomDialog),

            UseStaticRandomDialog = this.UseStaticRandomDialog,
            Ascii = string.Copy(this.Ascii),

            InventoryData = new List<int>(this.InventoryData),
            ArmorData = new List<int>(this.ArmorData),
            Armor = this.Armor?.Select(a => a.Clone()).ToList(), // assumes Item has Clone()

            Initiative = this.Initiative
        };
    }
    #region === ICharacter Methods ===

    public List<GameAction> PossibleAttacks(ICharacter Target, List<ICharacter> allies)
    {
        List<GameAction> actions = new();
        int x = Target.Root.LocalX - Root.LocalX;
        int y = Target.Root.LocalY - Root.LocalY;
        x = Math.Abs(x);
        y = Math.Abs(y);
        foreach (var action in TriggerData)
        {
            int range = action.Value.Range;
            if (range <= x || range <= y)
                actions.Add(action.Value);
        }
        return actions;
    }
    #endregion
    #region === IActionable ===
    // checks if the NPC can attack. Tied to IActionable
    public bool CanAttack(IActionable target)
    {
        Root caster = Root;
        Root tar = target.Root;
        int x = caster.LocalX - tar.LocalX;
        x = Math.Abs(x);
        int y = caster.LocalY - tar.LocalY;
        y = Math.Abs(y);
        foreach (var trigger in TriggerData)
        {
            if (trigger.Value.Range <= x || trigger.Value.Range <= y)
            {
                return true;
            }
        }
        return false;
    }

    public void PerformAttack(IActionable target)
    {
        if (CanAttack(target) && target is IHealth healthTarget)
        {
            // Placeholder logic: deal flat 1 damage
            healthTarget.Health.DamageHP(1);
        }
    }

    public bool CanUseBonus() => BonusActionCount > 0;

    public void PerformBonus(IActionable target)
    {
        // Placeholder: define custom bonus behavior
    }

    public CombatRelation GetCombatRelation(SaveGame saveGame)
    {
        // Placeholder: return default Neutral
        return CombatRelation.Neutral;
    }
    #endregion
}

#region === Components ===
public class VenderComponent
{
    public int BaseStock { get; set; }
    public int SpecialItemStock { get; set; }
    public int SpecialItemRestockRate { get; set; }
}
public class TrainerComponent
{

}
#endregion