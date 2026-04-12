using System;

namespace The_Game;


public interface IPlayerEngine
{
    PlayerObject PlayerCharacter { get; set; }
    bool CanAttack(IActionable target);
    void PerformAttack(IActionable target);
    bool CanUseBonus();
    void PerformBonus(IActionable target);
    CombatRelation GetCombatRelation();
    List<ActionObject> PossibleAttacks(ICharacter Target, List<ICharacter> allies);
}
