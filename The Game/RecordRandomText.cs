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
// The random text and dialog based on weather, biome, creature type, dialog category and speaker
public record RecordRandomText
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
    public String Text { get; set; }
}