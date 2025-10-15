using GameNamespace;
using System;

public class DataNPC
{
    public List<NPCType> NPCType { get; set; }
    public List<NPC> NPCs { get; set; }

    // --- Safe lookups (return null if not found) ---
    public NPCType? GetTypeNPCByID(int id)
    {
        var npcType = NPCType.FirstOrDefault(n => n.ID == id);
        if (npcType == null)
            throw new KeyNotFoundException($"NPCType with ID {id} not found in hydrated list.");
        return npcType;
    }
    public NPC? GetNamedNPCByID(int id)
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
            var triggerData = triggerDataProp.GetValue(npc) as IDictionary<CombatActionType, GameAction>;

            if (triggerData == null)
            {
                triggerData = new Dictionary<CombatActionType, GameAction>();
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
                    triggerData[gameAction.ActionType] = gameAction; // replace or add
            }
        }
    }
}