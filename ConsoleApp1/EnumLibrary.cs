using System;

public enum damanageType
{
    Slashing,
    Piercing,
    Bludgeoning,
    None
}
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

#region === Triggers ===
public enum NPCTriggerActions
{
    Trade,
    Attack,
    Flee,
    RequestHelp,
    GiveQuest,
    Steal,
    Bribe,
    Talk
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
#region === Grid Enum Data
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

public static class WordAlias
{
    public static readonly Dictionary<string, List<string>> Verbs = new()
    {
        ["Open"] = new() {
            "open", "unlock", "unseal", "unlatch", "unbolt", "crack",
            "pry", "force", "break into", "access", "lift", "pull",
            "slide", "reveal", "push", "pop open"
        },
        // Add other actions here
    };
    public static readonly Dictionary<string, List<string>> Responses = new()
    {
        ["Yes"] = new() {
            "yes", "y"
        },
        // Add other responses here
    };
}

