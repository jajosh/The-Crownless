using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


    public enum ActionNames
    {
        CorruptingTouch,
        HorrifiyingVisage,
        Wail,
        Heal
    }
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
    public enum TriggerEnum
    {
        Trade,
        Attack,
        Flee,
        RequestHelp,
        GiveQuest,
        Steal,
        Bribe,
        Talk,
    }
    public enum Skill
    {
        Strength,
        Dexterity,
        Charisma,
        Intelligence,
        Wisdom,
        Constitution
    }
    public enum EquipmentSlots
    {
        // Armor
        Head,
        Chest,
        Waist,
        Legs,
        Feet,
        Arms,
        Hands,
        Wrists,
        Shoulders,

        // Accessories
        Neck,
        Ring,
        Bracelet,
        Trinket,

        // Weapons
        MainHand,
        OffHand,
        Back,
        LowerBack,
        Side,


        // Miscellaneous / Custom
        Cap,
        Belt,
        Tabard
    }
    public enum ItemMatieralType { Wood, Leather, Iron, Steel, Adamantian }
    public enum ItemType { ShortSword, } //  For Craftable items

