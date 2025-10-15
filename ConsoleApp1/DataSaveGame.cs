using System;
using System.Text.Json.Serialization;
using Windows.ApplicationModel.DataTransfer;

/// <summary>
/// Helper Class to handle the save object. 
/// </summary>
public class SaveGame
{
    public Player PlayerCharacter { get; set; }
    public EngineWeather Weather { get; set; }
    public EngineLocation LocationEngine { get; set; }
    public StatusFlags Flags { get; set; }
    public List<Quest> CompletedQuests { get; set; }
    public List<Quest> ActiveQuests { get; set; }
    public DataNPC NPCs { get; set; }
    public List<Item> Items { get; set; }
    public List<TriggerCoordinets> Triggers { get; set; }
    public LoreBoard LoreBoard { get; set; }
    public Map GameMap { get; set; }
    public SaveGame()
    {

    }
}