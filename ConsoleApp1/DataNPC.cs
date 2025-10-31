using GameNamespace;
using System;

public interface IDataNPC
{
    List<IActionable> ActiveOnScreen { get; }     // Current grid NPCs
    List<IActionable> ActiveOnLocation { get; }   // Location-specific NPCs
    void LoadGridNPCs(int gridId);
    void LoadLocationNPCs(int locationId);
    void UnloadGrid(int gridId);
}

public class DataNPC : IDataNPC
{
    private readonly Dictionary<int, List<IActionable>> _gridNPCs = new();
    private readonly Dictionary<int, List<IActionable>> _locationNPCs = new();

    public List<IActionable> ActiveOnScreen { get; private set; } = new();
    public List<IActionable> ActiveOnLocation { get; private set; } = new();

    public void LoadGridNPCs(int gridId)
    {
        ActiveOnScreen = _gridNPCs.GetValueOrDefault(gridId, new());
        // Start AI processing for these NPCs
    }

    public void LoadLocationNPCs(int locationId)
    {
        ActiveOnLocation = _locationNPCs.GetValueOrDefault(locationId, new());
    }

    public void UnloadGrid(int gridId)
    {
        ActiveOnScreen.Clear();
        // Stop AI processing
    }
}
public class DataNPC
{
    public List<NPCType> NPCType { get; set; }
    public List<NPC> NPCs { get; set; }

    // --- Safe lookups (return null if not found) ---
    public static NPCType? GetTypeNPCByID(int id)
    {
        if (MainClass.saveGame.NPCs.NPCs == null)
            throw new InvalidOperationException("NPC list has not been loaded or initialized.");
        var npcType = MainClass.saveGame.NPCs.NPCType.FirstOrDefault(n => n.ID == id);
        if (npcType == null)
            throw new KeyNotFoundException($"NPCType with ID {id} not found in hydrated list.");
        return npcType;
    }
    public static NPC? GetNamedNPCByID(int id)
    {
        if (MainClass.saveGame.NPCs.NPCs == null)
            throw new InvalidOperationException("NPC list has not been loaded or initialized.");

        var npc = MainClass.saveGame.NPCs.NPCs.FirstOrDefault(n => n.ID == id);
        if (npc == null)
            throw new KeyNotFoundException($"NPC with ID {id} not found in hydrated list.");

        return npc;
    }
   
}
public static class NPCHydrator
{
    public static DataNPC Hydrate(DataNPC npcRawData)
    {

        if (npcRawData == null)
            throw new ArgumentNullException(nameof(npcRawData));

        // Hydrate both type templates and named NPCs
        if (npcRawData.NPCType != null)
            HydrateTriggerCollections(npcRawData.NPCType);

        if (npcRawData.NPCs != null)
            HydrateTriggerCollections(npcRawData.NPCs);

        return npcRawData;
    }

    private static void HydrateTriggerCollections<T>(IEnumerable<T> npcCollection)
    {
        foreach (var npc in npcCollection)
        {
            // Each npc (either NPCType or NPC) has TriggerRawData and TriggerData
            var triggerRawDataProp = npc.GetType().GetProperty("TriggerRawData");
            var triggerDataProp = npc.GetType().GetProperty("TriggerData");

            if (triggerRawDataProp == null || triggerDataProp == null)
                continue;

            var triggerRawData = triggerRawDataProp.GetValue(npc) as List<TriggerConfig>;
            var triggerData = triggerDataProp.GetValue(npc) as IDictionary<ActionType, GameAction>;

            if (triggerData == null)
            {
                triggerData = new Dictionary<ActionType, GameAction>();
                triggerDataProp.SetValue(npc, triggerData);
            }

            if (triggerRawData == null) continue;

            foreach (var triggerConfig in triggerRawData)
            {
                if (string.IsNullOrWhiteSpace(triggerConfig.Action))
                    continue;

                var parameters = triggerConfig.Params ?? new();
                var gameAction = GameActionFactory.Create(triggerConfig.Action, parameters);

                if (gameAction != null)
                    triggerData[gameAction.Type] = gameAction; // replace or add
            }
        }
    }
}