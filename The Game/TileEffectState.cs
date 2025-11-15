using System;
using MyGame.Controls;

public class TileEffectState
{
    public int ID { get; set; }
    public string Name { get; set; } = string.Empty;
    public List<string> Description { get; set; } = new();


    // 🎨 Visuals
    public ColorComponent ForeGroundColor { get; set; }
    public ColorComponent BackGroundColor { get; set; }


    // ⚔️ Effect Mechanics
    public int? DamageOverTime { get; set; }
    public DamageTypes? DamageType { get; set; }
    public int? MovementPenalty { get; set; }
    public int? Duration { get; set; }
    public int Radius { get; set; }
    public RootComponent Root { get; set; }


    // 👁️ Environmental Interaction
    public bool? BlockVision { get; set; }


    // 💫 Conditions
    public int? ChanceToApplyCondition { get; set; }
    public Conditions? ConditionToApply { get; set; }


    // ⚙️ Behavior Flags
    public bool IsStackable { get; set; } = false;
    public bool IsSpreadable { get; set; } = false;
    public bool IsCleansable { get; set; } = true;


    // 🧠 Runtime State
    public bool IsActive { get; set; } = false;
    public TileEffectState Clone()
    {
        return new TileEffectState
        {
            ID = this.ID,
            Name = this.Name,
            Description = new List<string>(this.Description),

            ForeGroundColor = this.ForeGroundColor,
            BackGroundColor = this.BackGroundColor,

            DamageOverTime = this.DamageOverTime,
            DamageType = this.DamageType,
            MovementPenalty = this.MovementPenalty,
            Duration = this.Duration,
            Radius = this.Radius,
            Root = this.Root != null ? this.Root.Clone() : null,

            BlockVision = this.BlockVision,

            ChanceToApplyCondition = this.ChanceToApplyCondition,
            ConditionToApply = this.ConditionToApply,

            IsStackable = this.IsStackable,
            IsSpreadable = this.IsSpreadable,
            IsCleansable = this.IsCleansable,

            IsActive = this.IsActive
        };
    }
}
