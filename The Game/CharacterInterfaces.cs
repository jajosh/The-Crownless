using The_Game;
using Newtonsoft.Json;
using MyGame.Controls;

namespace The_Game
{
    public interface ICharacter : ISkills, IActor, IActionable, IInventory, ITileOccupant
    {
        bool CanAttack(IActionable target);
        void PerformAttack(IActionable target);
        bool CanUseBonus();
        void PreformBonus(IActionable target);
        CombatRelation GetCombatRelation(IActionable target);
    }
    public interface ITileOccupant
    {
        string Name { get; }
        RootComponent Root { get; set; }
        TileRenderProfile Render { get; set; }
        HealthComponent Health { get; set; }
    }
    public interface IActionable : ITileOccupant
    {
        // --- Identity -----------------------------------------------
        RaceComponent Race { get; set; }

        // --- Components ---------------------------------------------
        RootComponent Root { get; set; }     // Position or location component
        HealthComponent Health { get; set; } // Health & Mana

        // --- Data & State -------------------------------------------
        Dictionary<ActionEffectType, List<ActionObject>> TriggerAction { get; set; }

        // === Visual ===


        
        List<ActionObject> PossibleAttacks(ICharacter Target, List<ICharacter> allies, List<ICharacter> Enemies);
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

        // --- Core Stats ---------------------------------------------
        int TileSpeed { get; set; }          // Movement speed
        int ActionCount { get; }             // Base actions
        int BonusActionCount { get; }        // Base bonus actions
    }
}
