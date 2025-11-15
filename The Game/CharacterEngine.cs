using The_Game;
using WindowsFormsApp1;
using Newtonsoft.Json;

public interface ICharacter : ISkills, IActor, IActionable, IInventory { }
public interface IActionable
{
    // ─── Identity ───────────────────────────────────────────────
    string Name { get; set; }
    RaceComponent Race { get; set; }

    // ─── Components ─────────────────────────────────────────────
    RootComponent Root { get; set; }     // Position or location component
    HealthComponent Health { get; set; } // Health & Mana

    // ─── Data & State ───────────────────────────────────────────
    List<TriggerConfig> TriggerKey { get; set; } // Used for hydration
    List<ActionObject> TriggerData { get; set; }


    bool CanAttack(IActionable target);
    void PerformAttack(IActionable target);
    bool CanUseBonus();
    void PerformBonus(IActionable target);
    CombatRelation GetCombatRelation(SaveGame saveGame);
    List<ActionObject> PossibleAttacks(ICharacter Target, List<ICharacter> allies);
}
public interface IHealth
{
    HealthComponent Health { get; }
}
public interface IInventory
{
    Dictionary<CoinType, int> Money { get; set; }
    [JsonIgnore]
    InventoryComponent Inventory { get; set; }
}
public interface ISkills
{
    SkillComponent Skills { get; set; }
}
public interface IActor
{

    // ─── Core Stats ─────────────────────────────────────────────
    int TileSpeed { get; set; }          // Movement speed
    int ActionCount { get; }             // Base actions
    int BonusActionCount { get; }        // Base bonus actions
}