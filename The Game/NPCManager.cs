using Newtonsoft.Json;
using System;
using The_Game;
using WindowsFormsApp1;

public class NPCManager : INPCEngine
{
    List<NPCObject> NPCs => NPCs;
    public NPCManager()
	{
	}
    #region === IActionable ===
    // checks if the NPC can attack. Tied to IActionable
    public bool CanAttack(IActionable target)
    {
        //RootComponent caster = Root;
        //RootComponent tar = target.Root;
        //int x = caster.LocalX - tar.LocalX;
        //x = Math.Abs(x);
        //int y = caster.LocalY - tar.LocalY;
        //y = Math.Abs(y);
        //foreach (var trigger in TriggerData)
        //{
        //    if (trigger.Value.Range <= x || trigger.Value.Range <= y)
        //    {
        //        return true;
        //    }
        //}
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
        //int x = Target.Root.LocalX - Root.LocalX;
        //int y = Target.Root.LocalY - Root.LocalY;
        //x = Math.Abs(x);
        //y = Math.Abs(y);
        //foreach (var action in TriggerData)
        //{
        //    int range = action.Value.Range;
        //    if (range <= x || range <= y)
        //        actions.Add(action.Value);
        //}
        return actions;
    }
    #endregion
}

