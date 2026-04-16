using System;

namespace The_Game;
public class  NPCObject : ICharacter
{
    #region === Identity ===

    public int ID { get; set; }
    public string Name { get; set; }
    public int TypeID { get; set; }
    public RaceComponent Race { get; set; }
    private TileRenderProfile _render = new TileRenderProfile();
    public TileRenderProfile Render
    {
        get
        {
            _render.CharData.MainChar = "@";
            _render.CharData.ShadowChar = "@";
            _render.CharData.TintChar = "@";
            return _render;
        }
        set => _render = value;
    }
    #endregion

    #region === Positioning and Movement ===

    public List<RootComponent> WayPoints { get; set; }
    public int? CurrentWaypointIndex { get; set; }

    public int TileSpeed { get; set; } = 6; // Default per IActionable
    public int ActionCount { get; set; } = 1;
    public int BonusActionCount { get; set; } = 1;
    public RootComponent Root { get; set; }
    #endregion

    #region === Stats & Abilities ===

    public Dictionary<CoinType, int> Money { get; set; }
    public HealthComponent Health { get; set; }
    public SkillComponent Skills { get; set; }
    public List<Languages>? Languages { get; set; }
    public List<DamageTypes>? DamageResistance { get; set; }
    public List<DamageTypes>? DamageImmunities { get; set; }
    public List<Conditions>? ConditionImmunities { get; set; }
    #endregion

    #region === Dialog === 

    public List<DescriptionEntry>? RandomDialog { get; set; }
    public bool UseStaticRandomDialog { get; set; }
    #endregion

    public InventoryComponent Inventory { get; set; }

    #region === Actions and Components ===

    public int? Initiative { get; set; }
    public Dictionary<ActionEffectType, List<ActionObject>> TriggerAction { get; set; }
    public NPCComponent Component { get; set; }

    #endregion

    public NPCObject()
	{
        WayPoints = new();
        Money = new Dictionary<CoinType, int>();
        Health = new HealthComponent();
        Skills = new SkillComponent();
        Languages = new List<Languages>();
        DamageImmunities = new List<DamageTypes>();
        DamageResistance = new List<DamageTypes>();
        ConditionImmunities = new List<Conditions>();
        RandomDialog = new List<DescriptionEntry>();
	}
    public bool CanAttack(IActionable Target)
    {
        return true;
        #region == Find distance between the target and caster ==
        #endregion
    }
    public void PerformAttack(IActionable target)
    {

    }
    public bool CanUseBonus()
    {
        return false;
    }
    public void PreformBonus(IActionable target)
    {
            
    }
    public CombatRelation GetCombatRelation(IActionable target)
    {
        return CombatRelation.Neutral;
    }
    public List<ActionObject> PossibleAttacks(ICharacter Target, List<ICharacter> allies, List<ICharacter> Enemies)
    {
        List<ActionObject> results = new();
        return results;
    }
}
public interface NPCComponent { }
public record TrainerComponent(
    Skill TrainedSkill) : NPCComponent;
