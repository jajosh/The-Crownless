using System;

#region === Grid Biome Enum Data
public enum GridBiomeType // The biome of the grid
{
    Any,
    BorelForest,
    TemperateBroadleafForest,

}
public enum GridBiomeSubType
{
    Any,
    AbandonedBuilding,
    Town,
}
public enum TileTypes
{
    empty,
    Grass,
    Tree,
    Forest,
    DirtPath,
    StoneBricks,
    Wall,
    Window,
    Pew,
    Alter,
    Town
}
public enum TileStates
{
    Open,
    Closed,
    Plant,
    NoPlant

}
public enum TileTriggeractions
{
    SecretChest,
    PlantSpawn,
    Trader,
    Quest,
    Event,
    Door,
    Combat
}
public enum TileCheckType
{
    None = 0,
    NeighborRoofed = 1 << 0,
    NeighborWalkable = 1 << 1,
    // Add more as needed
}

#endregion
public enum QuestStatus
{
    Inactive,
    Active,
    Completed,
    Failed
}
public enum EventType
{
    Dialog,
    Combat,
    SkillCheck

}
#region === Player and NPC Enum Data ===
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
    RingLeft,
    RingRight,
    Trinket,

    // Weapons
    MainHand,
    OffHand,
    Ranged,

    // Miscellaneous / Custom
    Back,
    Belt,
    Tabard
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
public enum ActionTypes
{
    CorruptingTouch,
    HorrifiyingVisage,
    Wail,
}
public enum Pronouns
{
    subjective,
    objective,
    possessive
}
public enum AlignmentChart
{
    LawfulGood,
    LawfulNeutral,
    LawfulEvil,
    NeutralGood,
    NeutralNeutral,
    NeutralEvil,
    ChaoticGood,
    ChaoticNeutral,
    ChaoticEvil
}

public enum CreatureType
{
    Undead
}
public enum CreatureSubType
{
    Spirit,

}
public enum ResponseFeelingType
{
    Happy,
    kind,
    Aggressive,
    Inquisitive,
    Neutral
}
#endregion


#region === Triggers ===
public enum TriggerKey
{
    Combat,
    Social
}
public enum Triggers
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
#endregion


#region === Skill and money enums
//Enum containing coin types
public enum Moneytype
{
    Copper,
    Silver,
    Gold
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
//List of all sub skills in the game. 
public enum SkillSubset
{

}
#endregion
public struct EnvironmentRef
{
    BiomeRef biomeRef;
    WeatherRef weatherRef;
}

