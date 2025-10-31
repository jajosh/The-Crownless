using System;
using System.Reflection;
using System.Transactions;
using Windows.ApplicationModel.Background;
public enum ActionType {Action, BonusAction, FreeAction, Ritual}
public enum ActionEffectType { Heal, Attack, Buff, Debuff}
public enum ActionRangeType { TileEffect, SingleTarget, AreaOfEffect, Self, Multiple}
/// <summary>
/// Provides factory methods for creating instances of game actions based on their names.
/// </summary>
public static class GameActionFactory
{
    private static readonly Dictionary<string, Type> _actionTypes;

    static GameActionFactory()
    {
        _actionTypes = Assembly.GetExecutingAssembly()
            .GetTypes()
            .Where(t => t.IsSubclassOf(typeof(GameAction)) && !t.IsAbstract)
            .ToDictionary(t => t.Name, t => t);
    }

    public static GameAction? Create(string actionName, Dictionary<string, object>? parameters = null)
    {
        if (!_actionTypes.TryGetValue(actionName, out var type))
        {
            Console.WriteLine($"[WARN] No GameAction found for '{actionName}'");
            return null;
        }

        // If no params, try default constructor
        if (parameters == null || parameters.Count == 0)
            return Activator.CreateInstance(type) as GameAction;

        // Try to match constructor parameters
        foreach (var ctor in type.GetConstructors())
        {
            var ctorParams = ctor.GetParameters();

            var args = new object?[ctorParams.Length];
            bool match = true;

            for (int i = 0; i < ctorParams.Length; i++)
            {
                var p = ctorParams[i];
                if (parameters.TryGetValue(p.Name!, out var rawValue))
                {
                    try
                    {
                        args[i] = Convert.ChangeType(rawValue, p.ParameterType);
                    }
                    catch
                    {
                        match = false;
                        break;
                    }
                }
                else
                {
                    match = false;
                    break;
                }
            }

            if (match)
                return ctor.Invoke(args) as GameAction;
        }

        Console.WriteLine($"[WARN] No matching constructor for '{actionName}' with given params");
        return null;
    }
}

/// <summary>
/// Represents an abstract action that can be performed within the game, such as an attack, spell, or ability. Serves as
/// a base class for defining specific game actions with a name, range, and execution logic.
/// </summary>
public abstract class GameAction
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


    public GameAction(string name) => Name = name;

    public abstract void Execute(ICharacter caster, ICharacter target);
}
public class CorruptingTouch : GameAction
{
    public int Damage { get; }
    public int Heal { get; }
    public CorruptingTouch(int damage = 10) : base(nameof(CorruptingTouch)) => Damage = damage;
    public new int range = 1;
    public ActionRangeType RangeType = ActionRangeType.SingleTarget;

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

public class Heal : GameAction
{
    public int Amount { get; }
    public Heal(int amount = 15) : base(nameof(Heal)) => Amount = amount;
    public ActionRangeType RangeType = ActionRangeType.Self;
    

    public override void Execute(ICharacter caster, ICharacter target)
    {
        target.Health.HealHP(Amount);
        Console.WriteLine($"{caster.Name} heals {target.Name} for {Amount} health!");
    }
}