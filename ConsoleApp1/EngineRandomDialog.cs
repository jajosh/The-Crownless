using System;

public enum DialogCategory
{
    Ambient,
    Combat,
    Ally,
    Friendly,
    Description,
    Rare
}
public enum DialogSpeaker
{
    NPC,
    Player,
    Narration
}
/// <summary>
/// Represents a random text entry used for dialog or descriptions, with associated context such as weather, season,
/// biome, and creature type.
/// </summary>
/// <remarks>This class is typically used to store and retrieve context-aware text for in-game events, dialog, or
/// environmental descriptions. The properties allow filtering or categorizing text based on game state, such as combat
/// scenarios, relationship status, or environmental conditions.</remarks>
public class EngineRandomText
{
    // ID tag for the text
    public int ID { get; set; }
    // Context tags
    public WeatherData? Weather { get; set; }
    public SeasonData? Season { get; set; }
    public GridBiomeType? Biome { get; set; }
    public GridBiomeSubType? BiomeSub { get; set; }
    public CreatureType? CreatureType { get; set; }
    // Categorization
    public List<DialogCategory> Categories { get; set; } = new List<DialogCategory>();
    public DialogSpeaker Speaker { get; set; }
    // The actual text line
    public List<String> Text { get; set; }
    public EngineRandomText()
    {

    }
}
