using System;
using WindowsFormsApp1;

public class PlayerManager : IPlayerEngine
{
    public PlayerObject PlayerCharacter { get; set; }
    public PlayerManager()
    {
        PlayerCharacter = new PlayerObject();
    }
    public bool CanAttack(IActionable target)
    {
        RootComponent caster = PlayerCharacter.Root;
        RootComponent tar = target.Root;
        int x = caster.LocalX - tar.LocalX;
        x = Math.Abs(x);
        int y = caster.LocalY - tar.LocalY;
        y = Math.Abs(y);
        foreach (var trigger in PlayerCharacter.TriggerData)
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
            try
            {
                // Placeholder logic: deal flat 1 damage
                healthTarget.Health.DamageHP(1);
            }
            catch (Exception ex)
            {
                BugHunter.LogException(ex);
            }
        }
    }
    public bool CanUseBonus() => PlayerCharacter.BonusActionCount > 0;
    public void PerformBonus(IActionable target)
    {
        // Placeholder: define custom bonus behavior
    }
    public CombatRelation GetCombatRelation()
    {
        // Placeholder: return default Neutral
        return CombatRelation.Neutral;
    }
    public List<ActionObject> PossibleAttacks(ICharacter Target, List<ICharacter> allies)
    {
        List<ActionObject> actions = new();
        int x = Target.Root.LocalX - PlayerCharacter.Root.LocalX;
        int y = Target.Root.LocalY - PlayerCharacter.Root.LocalY;
        x = Math.Abs(x);
        y = Math.Abs(y);
        foreach (var action in PlayerCharacter.TriggerData)
        {
            int range = action.Range;
            if (range <= x || range <= y)
                actions.Add(action);
        }
        return actions;
    }
}