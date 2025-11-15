using System;
using Newtonsoft.Json;

public class NPCObject : NPCManager
{
    public bool CanUseBonus()
    {
        return false;
    }
    public List<TriggerConfig> TriggerKey { get; set; } // Used for hydration
    public List<ActionObject> TriggerData { get; set; }
    public Dictionary<CoinType, int> Money { get; set; }
    #region Identity
    public int ID { get; set; }
    public string Name { get; set; }
    public int TypeID { get; set; }
    public RaceComponent Race { get; set; }
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
    public SkillComponent Skills { get; set; }
    public RootComponent Root { get; set; }
    public VenderComponent? Vender { get; set; }
    #endregion

    #region Stats & Abilities

    public List<Languages>? Languages { get; set; }
    public List<DamageTypes>? DamageResistance { get; set; }
    public List<DamageTypes>? DamageImmunities { get; set; }
    public List<Conditions>? ConditionImmunities { get; set; }
    #endregion

    #region Dialog
    public List<RecordRandomText>? RandomDialog { get; set; }
    public bool UseStaticRandomDialog { get; set; }
    public char? Ascii { get; set; }
    #endregion

    #region Inventory & Equipment
    public InventoryComponent Inventory { get; set; }
    #endregion

    #region Triggers & Actions
    public int? Initiative { get; set; }
    #endregion


    public NPCObject()
    {
        
    }
    // --- Deep Clone ---
    public NPCObject Clone()
    {
        return new NPCObject
        {
           
        };
    }
    public new List<ActionObject> PossibleAttacks(ICharacter Target, List<ICharacter> allies)
    {
        List<ActionObject> results = new();
        return results;
    }


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