using System;


#region === Entitiy helper classes ===
public class HealthComponent
{
    public int CurrentHP { get; set; }
    public int MaxHP { get; set; }
    public int CurrentMP { get; set; }
    public int MaxMP { get; set; }
    public bool IsAlive { get; set; }
    public void DamageHP(int amount)
    {
        CurrentHP = CurrentHP - amount;
    }
    public void DamageMP(int amount)
    {
        CurrentMP = CurrentMP - amount;
    }
    public void HealHP(int amount)
    {
        CurrentHP = CurrentHP - amount;
    }
    public void HealMP(int amount)
    {
        CurrentMP = CurrentMP - amount;
    }
    public HealthComponent Clone()
    {
        var HealthComponent = new HealthComponent
        {
            CurrentHP = this.CurrentHP,
            MaxHP = this.MaxHP,
            CurrentMP = this.CurrentMP,
            MaxMP = this.MaxMP
        };
        return HealthComponent;
    }
}
#endregion