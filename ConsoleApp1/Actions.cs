using System;
public enum ActionNames
{
    CorruptingTouch,
    HorrifiyingVisage,
    Wail,
    Heal
}
public enum TriggerEnum
{
    #region --- NPC Triggers
    Trade,
    Attack,
    Flee,
    RequestHelp,
    GiveQuest,
    Steal,
    Bribe,
    Talk,
    #endregion
}
// Used to connect any object that can serve as caster or target of an action
public interface IActionable
{
    string Name { get; }
    int CurrentHP { get; set; } // Only for objects that have HP; can ignore for others
    void ApplyDamage(int amount);

    public int Gridx { get; set; }
    public int GridY { get; set; }
    public int LocalX { get; set; }
    public int LocalY { get; set; }
}
public abstract class GameAction
{
    public string Name { get; }
    public GameAction(string name) => Name = name;

    public abstract void Execute(IActionable caster, IActionable target);
}

public static class ActionRegistry
{
    public static readonly Dictionary<ActionNames, Func<GameAction>> Actions = new()
    {
        { ActionNames.CorruptingTouch, () => new CorruptingTouch(10) },
        { ActionNames.Heal, () => new Heal(15) }
    };
}
public class CorruptingTouch : GameAction
{
    public int Damage { get; }
    public CorruptingTouch(int damage = 10) : base(nameof(CorruptingTouch)) => Damage = damage;

    public override void Execute(IActionable caster, IActionable target)
    {
        if (caster == target)
        {
            Console.WriteLine("You sure you want to hit yourself?");
        }
        target.ApplyDamage(Damage);
        Console.WriteLine($"{caster.Name} corrupts {target.Name} for {Damage} damage!");
    }
}

public class Heal : GameAction
{
    public int Amount { get; }
    public Heal(int amount = 15) : base(nameof(Heal)) => Amount = amount;

    public override void Execute(IActionable caster, IActionable target)
    {
        target.CurrentHP += Amount;
        Console.WriteLine($"{caster.Name} heals {target.Name} for {Amount} health!");
    }
}