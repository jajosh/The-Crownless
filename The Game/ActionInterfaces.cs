using System;
namespace The_Game
{
    public interface IDamaging
    {
        int DieSize { get; }
        int DieAmount { get; }
        int RollDamage();
    }

    public interface IHealing
    {
        int DieSize { get; }
        int DieAmount { get; }
        int RollHeal();
    }

    public interface IManaCost
    {
        int Cost { get; }
    }

    public interface ILimitedUse
    {
        int CastsPerDay { get; }
        int TimesCastToday { get; set; }
        bool CanCast => TimesCastToday < CastsPerDay;
    }
}