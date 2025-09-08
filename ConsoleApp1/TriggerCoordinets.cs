using System;
using System.Data;
using System.Text.Json;
using System.Text.Json.Serialization;

/// <summary>
/// Summary description for Class1
/// </summary>
public class TriggerCoordinets
{
    public int GridX { get; set; }
    public int GridY { get; set; }
    public int LocalX { get; set; }
    public int LocalY { get; set; }
    public string Description { get; set; }
    public TileStates TileState { get; set; }
    public Dictionary<int, int> PossibleNextEvents { get; set; }
    public List<TileTriggeractions> TriggerActions{ get; set; } = new();

    public TriggerCoordinets()
    {
        GridX = 0;
        GridY = 0;
        LocalX = 0;
        LocalY = 0;
        PossibleNextEvents = new();
        Description = string.Empty;
        PossibleNextEvents = new();
        TriggerActions = new();
    }
    public TriggerCoordinets(int gridX, int gridY, int localX, int localY, string description, Dictionary<int, int> possibleNextEvents, List<TileTriggeractions> triggerActions)
    {
        GridX = gridX;
        GridY = gridY;
        LocalX = localX;
        LocalY = localY;
        Description = description;
        PossibleNextEvents = possibleNextEvents;
        TriggerActions = triggerActions;

    }
    

    public static void SaveToJson(string filePath, List<TriggerCoordinets> triggers)
    {
        var options = new JsonSerializerOptions
        {
            WriteIndented = true // pretty print for readability
        };

        string json = JsonSerializer.Serialize(triggers, options);
        File.WriteAllText(filePath, json);
    }
}

