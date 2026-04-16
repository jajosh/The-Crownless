using System;
using System.Text.Json;
namespace The_Game;
    public class CorruptingTouch : ActionObject, IDamaging, ICloneable
    {
        public int DieSize { get; set; }
        public int DieAmount { get; set; }

        // Parameterless constructor for the deserializer
        public CorruptingTouch() : base(nameof(CorruptingTouch)) { }

        public int RollDamage() => Enumerable.Range(0, DieAmount)
            .Sum(_ => Random.Shared.Next(1, DieSize + 1));

        public override void Execute(ICharacter caster, IActionable target)
        {
            int dmg = RollDamage();
            target.Health.DamageHP(dmg);
        }

        public override CorruptingTouch Clone()
        {
            return Clone();
        }
    }
    public class FireBall : ActionObject, IDamaging
    {
        public int DieSize { get; set; }
        public int DieAmount { get; set; }

        // Parameterless constructor for the deserializer
        public FireBall() : base(nameof(FireBall)) { }

        public int RollDamage() => Enumerable.Range(0, DieAmount)
            .Sum(_ => Random.Shared.Next(1, DieSize + 1));

        public override void Execute(ICharacter caster, IActionable target)
        {
            int dmg = RollDamage();
            target.Health.DamageHP(dmg);
        }
        public override FireBall Clone()
        {
            return new FireBall
            {
                DieSize = this.DieSize,
                DieAmount = this.DieAmount
            };
        }
    }

    public class HealAction : ActionObject, IHealing, IManaCost
    {
        public int DieSize { get; set; } = 8;
        public int DieAmount { get; set; } = 1;
        public int Cost { get; set; } = 10;

        public HealAction() : base(nameof(HealAction))
        {
            Type = ActionType.Action;
            RangeType = ActionRangeType.SingleTarget;
            Range = 5;
        }
        public override HealAction Clone()
        {
            return new HealAction
            {
                DieSize = this.DieSize,
                DieAmount = this.DieAmount,
                Cost = this.Cost
            };
        }
        public int RollHeal() => Random.Shared.Next(1, DieSize + 1) * DieAmount;

        public override void Execute(ICharacter caster, IActionable target)
        {
            int amount = RollHeal();
            target.Health.HealHP(amount);
            BugHunter.Log(DebugType.LOG, $"{caster.Name} heals {target.Name} for {amount}!", DebugLogSeverity.INFO, true);
        }
    }
public class ApplyStatusAction : ActionObject, IDamaging
{
    public ActionEffectType Type { get; init; }
    public int DieSize { get; set; }
    public int DieAmount { get; init; }
    public ApplyStatusAction() : base(nameof(ApplyStatusAction))
    {
        RangeType = ActionRangeType.AreaOfEffect;
        Range = 10;
    }
    public int RollDamage() => Random.Shared.Next(0, DieSize + 1) * DieAmount;
    public override void Execute(ICharacter caster, IActionable target)
    {
        int amount = RollDamage();
        target.Health.DamageHP(amount);
        BugHunter.Log(DebugType.LOG, $"{caster.Name} damaged {target.Name} for {amount} with a {Type} status effect!", DebugLogSeverity.INFO, true);
    }
    public override ApplyStatusAction Clone()
    {
        return new ApplyStatusAction
        {
            DieSize = this.DieSize,
            DieAmount = this.DieAmount,
            Type = this.Type
        };
    }
}