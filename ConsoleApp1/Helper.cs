using System;

#region === Grid Biome Enum Data
public enum GridBiomeType // The biome of the grid
{
    Any,
    BorelForst,
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
    Door
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
    Common,
    Elvish
}
public enum DamageTypes
{
    Slashing,
    Piercing,
    Bludgeoning,
    None
}
public enum Conditions
{
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
    Undead,
    kind,
    Aggressive,
    Inquisitive,
    Neutral
}
public enum CreatureSubType
{
    Banshee
}
public enum ResponseFeelingType
{
    Happy
}
#endregion


#region === Triggers ===
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
    Talk
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
public class EnvironmentDialog
{
    public WeatherData? weatherMain {  get; set; }
    public SeasonData? seasonMain { get; set; }
    public GridBiomeType? biomeMain {  get; set; }
    public GridBiomeSubType? biomeSub {  get; set; }
    public List<string> text = new();
}

