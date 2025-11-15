using System;
public enum ActionType { Action, BonusAction, FreeAction, Ritual }
public enum ActionEffectType { Heal, Attack, Buff, Debuff }
public enum ActionRangeType { TileEffect, SingleTarget, AreaOfEffect, Self, Multiple }
public abstract class ActionObject
{
    // action Identity
    public string Name { get; }
    public ActionType Type { get; } // E.G. Action, bonus action, free action

    //Basic states
    public int? DamageDieSize { get; }
    public int? DamageDieAmount { get; }
    public int? HealDieSize { get; }
    public int? HealDieAmount { get; }
    public int? ManaCost { get; }
    public int? CastsPerDat { get; }
    public int? TimesCast { get; }

    // Meta Data
    public int? TotalTimesCast { get; }
    public bool IsSpell { get; }



    public ActionEffectType Effect { get; } // What type of action is it? Heal, Damage, ...
    // Action range and type
    public ActionRangeType RangeType { get; } // Self, touch, single target...
    public int Range { get; }


    public ActionObject(string name) => Name = name;

    public abstract void Execute(ICharacter caster, ICharacter target);
}
public class CorruptingTouch : ActionObject
{
    public int Damage { get; }
    public int Heal { get; }
    public CorruptingTouch(int damage = 10) : base(nameof(CorruptingTouch)) => Damage = damage;
    public new int Range = 1;
#pragma warning disable CS0108 // Member hides inherited member; missing new keyword
    public ActionRangeType RangeType = ActionRangeType.SingleTarget;
#pragma warning restore CS0108 // Member hides inherited member; missing new keyword

    public override void Execute(ICharacter caster, ICharacter target)
    {
        if (caster == target)
        {
            Console.WriteLine("You sure you want to hit yourself?");
        }
        target.Health.DamageHP(Damage);
        Console.WriteLine($"{caster.Name} corrupts {target.Name} for {Damage} damage!");
    }
}

public class Heal : ActionObject
{
    public int Amount { get; }
    public Heal(int amount = 15) : base(nameof(Heal)) => Amount = amount;
#pragma warning disable CS0108 // Member hides inherited member; missing new keyword
    public ActionRangeType RangeType = ActionRangeType.SingleTarget;
#pragma warning restore CS0108 // Member hides inherited member; missing new keyword


    public override void Execute(ICharacter caster, ICharacter target)
    {
        target.Health.HealHP(Amount);
        Console.WriteLine($"{caster.Name} heals {target.Name} for {Amount} health!");
    }
}