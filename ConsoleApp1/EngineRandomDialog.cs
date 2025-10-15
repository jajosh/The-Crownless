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
public class ObjectRandomText
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
}
public class EngineRandomText
{
    
    public EngineRandomText()
    {

    }
}
