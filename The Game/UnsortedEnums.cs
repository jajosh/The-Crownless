using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace The_Game
{


    public enum Languages
    {
        None,
        Common,
        Elvish,
        Abyssal
    }
    public enum DamageTypes
    {
        None,
        Slashing,
        Piercing,
        Bludgeoning,
        Necrotic,
        Cold,
        Poison,
        Posioned
    }
    public enum Conditions
    {
        None,
        Charmed,
        Exhaustion,
        Frightened,
        Grappled,
        Paralyzed,
        Petrified,
        Poisoned,
        Prone,
        restrained
    }
    public enum CombatRelation { Ally, Enemy, Neutral }
}

