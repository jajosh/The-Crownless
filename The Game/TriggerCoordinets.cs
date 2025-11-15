using System;
public enum TileStates
{
    Open,
    Closed,
    Plant,
    NoPlant

}
public class TriggerCoordinets
{
    RootComponent Root { get; set; }
    public string Description { get; set; }
    public TileStates TileState { get; set; }
    public Dictionary<int, int> PossibleNextEvents { get; set; } // event ID, weight
    public List<string> TriggerActions { get; set; } = new();

    public TriggerCoordinets()
    {
        Root.GridX = 0;
        Root.GridY = 0;
        Root.LocalX = 0;
        Root.LocalY = 0;
        Description = string.Empty;
        PossibleNextEvents = new();
        TriggerActions = new();
    }
    public TriggerCoordinets(int gridX, int gridY, int localX, int localY, string description, Dictionary<int, int> possibleNextEvents, List<string> triggerActions)
    {
        Root.GridX = gridX;
        Root.GridY = gridY;
        Root.LocalX = localX;
        Root.LocalY = localY;
        Description = description;
        PossibleNextEvents = possibleNextEvents;
        TriggerActions = triggerActions;

    }
}