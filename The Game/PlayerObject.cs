using System;
using The_Game;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

// The Player Object
public class PlayerObject : ICharacter
{
    // === Components ===
    #region Components
    public InventoryComponent? Inventory { get; set; }
    public HealthComponent Health { get; set; }
    public SkillComponent Skills { get; set; }
    #endregion

    // === Identity ===
    #region Identity
    public string Name { get; set; }
    public RaceComponent Race { get; set; } // May take out

    // Key = pronoun set ID, Value = (subjective, possessive, reflexive)
    public Dictionary<int, (string Subjective, string Possessive, string Reflexive)> pronouns =
        new()
        {
            { 1, ("he", "his", "himself") },
            { 2, ("she", "her", "herself") },
            { 3, ("they", "their", "themselves") }
        };

    public int pronounKey = 3; // Default
    #endregion

    // === Actions (IActionable) ===
    #region Actions
    public int ActionCount { get; set; } = 1;          // Base actions
    public int BonusActionCount { get; set; } = 1;     // Base bonus actions
    public int Initiative { get; set; }                // Combat initiative roll
    public RootComponent Root { get; set; }            // Player position
    public int TileSpeed { get; set; } = 6;            // Default movement speed
    #endregion

    // === Money (IMoney) ===
    #region Money
    public Dictionary<CoinType, int> Money { get; set; }
    #endregion

    // === Triggers & Actions ===
    #region Triggers
    public List<TriggerConfig> TriggerKey { get; set; } // Used for hydration
    public List<ActionObject> TriggerData { get; set; }
    [JsonIgnore]
    #endregion

    // === Events ===
    #region Events
    public int CurrentEvent { get; set; }
    public Dictionary<int, int> PastEvents { get; set; }
    #endregion

    // === Constructor ===
    public PlayerObject()
    {
        Inventory = null;
        InventoryData = new List<int>();
        Health = new HealthComponent();
        Skills = new SkillComponent();
        Name = string.Empty;
        Race = new RaceComponent();
        // pronouns is already initialized in the field declaration
        // pronounKey is already initialized in the field declaration
        // ActionCount is already initialized in the field declaration
        // BonusActionCount is already initialized in the field declaration
        Initiative = 0; // Default value, adjust if needed
        Root = new RootComponent();
        // TileSpeed is already initialized in the field declaration
        Money = new Dictionary<CoinType, int>();
        TriggerKey = new List<TriggerConfig>();
        TriggerData = new List<ActionObject>();
        CurrentEvent = 0; // Default value, adjust if needed
        PastEvents = new Dictionary<int, int>();
    }
    public bool CanAttack(IActionable target)
    {
        RootComponent caster = Root;
        RootComponent tar = target.Root;
        int x = caster.LocalX - tar.LocalX;
        x = Math.Abs(x);
        int y = caster.LocalY - tar.LocalY;
        y = Math.Abs(y);
        foreach (var trigger in TriggerData)
        {
            if (trigger.Range <= x || trigger.Range <= y)
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
    public List<ActionObject> PossibleAttacks(ICharacter Target, List<ICharacter> allies)
    {
        List<ActionObject> actions = new();
        int x = Target.Root.LocalX - Root.LocalX;
        int y = Target.Root.LocalY - Root.LocalY;
        x = Math.Abs(x);
        y = Math.Abs(y);
        foreach (var action in TriggerData)
        {
            int range = action.Range;
            if (range <= x || range <= y)
                actions.Add(action);
        }
        return actions;
    }
    public bool IsWearingMetal()
    {
        foreach (var item in Inventory.EquipedItems)
        {
            foreach (var stack in item.Value)
                {
                if (stack.Item.material == PrimaryMaterial.Metal)
                {
                    return true;
                }
            }
        }
        return false;
    }
    public bool IsWieldingSword()
    {
        foreach (var slotPair in Inventory.EquipedItems)
        {
            EquipmentSlots slot = slotPair.Key;                   // e.g. Head, Chest, Weapon, etc.
            List<ItemStackComponent> itemsInSlot = slotPair.Value;

            foreach (ItemStackComponent itemStack in itemsInSlot)
            {
                if (itemStack == null)
                    continue;

                if (itemStack.Item.PrimaryType == PrimaryType.Sword)
                {
                    return true;
                }

            }
        }
        return false;
    }

}
