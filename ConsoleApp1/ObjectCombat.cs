using System;
// Defined light weight structure for fast cloning
public struct CombatantState
{
    // Unique Iditifier
    public Root Root;
    public int ID;

    // === Key Ai Data ===
    public HealthComponent Health;
    public int TileSpeed;
    public int ActionPoints;
    public int BonusActionPoints;
    public int FreeActionPoints;

    public List<ItemStack> Inventory;
    public List<Conditions> ConditionImmunities;
    public List<DamageTypes> DamageResistance;
    public List<DamageTypes> DamageImmunities;
    // Deep copy constructor
    public CombatantState(CombatantState other)
    {
        Root = other.Root;
        ID = other.ID;
        Health = other.Health;
        TileSpeed = other.TileSpeed;
        ActionPoints = other.ActionPoints;
        BonusActionPoints = other.BonusActionPoints;
        FreeActionPoints = other.FreeActionPoints;

        Inventory = other.Inventory?.Select(item => item.Clone()).ToList();
        ConditionImmunities = other.ConditionImmunities != null ? new List<Conditions>(other.ConditionImmunities) : null;
        DamageResistance = other.DamageResistance != null ? new List<DamageTypes>(other.DamageResistance) : null;
        DamageImmunities = other.DamageImmunities != null ? new List<DamageTypes>(other.DamageImmunities) : null;
    }
}

// The Game State tracker
public class GameState
{
    // A list of mutable state for every combatant
    public List<CombatantState> CombatantState { get; set; }

    //The ID of the current actor whose turn it is
    public NPC CurrentActorID { get; set; }
    //The ID of the "nect" actor in the turn order (for multi-turn planning
    public NPC NextActorID { get; set; }
    public List<NPC> Actors { get; set; }
    public List<IActionable> Actionables { get; set; }
    
    // Enviromental Data
    public int GridX { get; set; }
    public int GridY { get; set; }
    public WeatherData WeatherData { get; set; }
    public SeasonData Season { get; set; }
    public List<TileEffectState>? TileEffects { get; set; }

    // Trun Tracker
    public int TurnCounter { get; set; }

    // Random Elements
    public int RandomSeed { get; set; }
    
    // TheCompy fuction. Clones the game state for turn planning. 
    public GameState Clone()
    {
        // Simple copy from original
        var newGameState = new GameState
        {
            CurrentActorID = this.CurrentActorID.Clone(),
            NextActorID = this.NextActorID.Clone(),
            GridX = this.GridX,
            GridY = this.GridY,
            TurnCounter = this.TurnCounter,
            RandomSeed = this.RandomSeed,

            WeatherData = this.WeatherData,
            Season = this.Season,
            // Deep copy the dynamic lists
            CombatantState = this.CombatantState.Select(state => new CombatantState(state)).ToList(),
            Actors = this.Actors.Select(npc => npc.Clone()).ToList(),
            TileEffects = this.TileEffects?.Select(e => e.Clone()).ToList()
        };

        return newGameState;
    }
    public GameState SimulateAction(int actorID, GameAction action)
    {
        // 1. Create a deep copy of the current state
        var futureState = this.Clone();

        // 2. Locate the actor in the copied state
        var actor = futureState.CombatantState.Find(c => c.ID == actorID);

        // 3. Apply the action's effect to the future state:

        switch (action.RangeType)
        {
            case ActionRangeType.Self:
                {
                    action.Execute(futureState.CurrentActorID, futureState.CurrentActorID);
                }
                break;
            case ActionRangeType.SingleTarget:
                {
                    
                }
                break;
            case ActionRangeType.AreaOfEffect:
                {

                }
                break;
            case ActionRangeType.TileEffect:
                {

                }
                break;
        }

        // 4. Return the new simulated state
        return futureState;
    }
    public GameState AdvanceTurn()
    {
        // 1. Create deep copy
        var futureState = this.Clone();

        // 2. Perform end-of-turn cleanup (dot damage, status duration countdown)
        // ...

        // 3. Cycle to the next actor
        futureState.CurrentActorID = futureState.NextActorID;
        // Recalculate NextActorID and reset Movement/Action points for the new actor
        // ...

        return futureState;
    }
    public int EvaluateAction(GameState state, GameAction action)
    {
        int score = 0;
        switch (action.RangeType)
        {
            case ActionRangeType.Self:
                {
                    if (action.DamageDieAmount == 0)
                    {
                        if(action.HealDieAmount > 0)
                        {
                            int x = 0;
                            while (x < action.HealDieAmount)
                            {
                                score += (int)((action.HealDieSize / 2) + 1); // Expected heal is half die size +1.
                                x++; // adds to score for each die, larger healing spells cost more
                            }
                        }
                    }
                    else
                        score += -10; // Do not want to hurt yourself
                }
                break;
            case ActionRangeType.SingleTarget:
                {
                    List<IActionable> actionable = // Need a method to find all targets within the grid
                }
                break;
            case ActionRangeType.AreaOfEffect:
                {
                    // Find groups of targets
                    // Needs to take into account possible side effects
                    //  like fireball in a wooded room. This needs to subtract cost
                }
                break;
            case ActionRangeType.TileEffect:
                {
                    // Things like throw traps down, targeting a tile
                    // logic should be more aimed to trapping the player
                }
                break;
        }
        return score;
    }

    public List<IActionable> FindAllTargets(GameState gameState, CombatantState combatantState)
    {
        List<IActionable> actionables = new List<IActionable>();
        // Finds a list of all actionabls, this should include
        return actionables;
    }
}

