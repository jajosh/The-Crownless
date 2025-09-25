using GameNamespace;
using System;

public class NPCData
{
    public List<NPCType> TypeNPCDehydrated = new();
    public List<NPC> NamedNPCDehydrated = new();
    public List<NPCType> TypeNPCHyraded = new();
    public List<NPC> NamedNPCHydrated = new();

    // --- Safe lookups (return null if not found) ---
    public NPCType? GetTypeNPCByID(int id)
    {
        return TypeNPCHyraded.FirstOrDefault(n => n.ID == id);
    }

    public NPC? GetNamedNPCByID(int id)
    {
        
        return NamedNPCHydrated.FirstOrDefault(n => n.ID == id);
    }

    // --- Strict lookups (throw exception if not found) ---
    public NPCType GetTypeNPCByIDOrThrow(int id)
    {
        var npcType = TypeNPCHyraded.FirstOrDefault(n => n.ID == id);
        if (npcType == null)
            throw new KeyNotFoundException($"NPCType with ID {id} not found in hydrated list.");
        return npcType;
    }

    public NPC GetNamedNPCByIDOrThrow(int id)
    {
        var npc = NamedNPCHydrated.FirstOrDefault(n => n.ID == id);
        if (npc == null)
            throw new KeyNotFoundException($"NPC with ID {id} not found in hydrated list.");
        return npc;
    }

    public static NPCData Hydrate()
    {
        var data = new NPCData();

        // --- Load NPCTypes directly into the hydrated list ---
        data.TypeNPCHyraded = JsonLoader.LoadFromJson<List<NPCType>>(FileManager.TheNPCTypeFilePath)
                              ?? new List<NPCType>();

        // Assign to FileManager for lookups
        FileManager.NPCTypes = data.TypeNPCHyraded;

        // Hydrate trigger data for each NPCType
        foreach (var npcType in FileManager.NPCTypes)
        {
            if (npcType.TriggerRawData == null) continue;

            foreach (var trigger in npcType.TriggerRawData)
            {
                if (Enum.TryParse<ActionNames>(trigger, out var actionName) &&
                    ActionRegistry.Actions.TryGetValue(actionName, out var factory))
                {
                    npcType.TriggerData[(TriggerEnum)Enum.Parse(typeof(TriggerEnum), trigger)] = factory();
                }
            }
        }

        // --- Load NPCs directly into the hydrated list ---
        data.NamedNPCHydrated = JsonLoader.LoadFromJson<List<NPC>>(FileManager.TheNPCFilePath)
                                ?? new List<NPC>();

        // Assign to FileManager for lookups
        FileManager.NPCs = data.NamedNPCHydrated;

        // Hydrate each NPC
        foreach (var npc in FileManager.NPCs)
        {
            // --- Inventory ---
            if (npc.InventoryData != null)
            {
                foreach (var id in npc.InventoryData)
                {
                    var item = new Item().FindItemByID(id);
                    if (item != null)
                        npc.Inventory.Add(item);
                }
            }

            // --- Armor ---
            if (npc.ArmorData != null)
            {
                foreach (var id in npc.ArmorData)
                {
                    var item = new Item().FindItemByID(id);
                    if (item != null)
                        npc.Armor.Add(item);
                }
            }

            // --- Inherit missing values from NPCType ---
            var type = NPCType.GetNPCTypeByID(npc.TypeID);
            if (type != null)
            {
                npc.TileSpeed ??= type.TileSpeed;
                npc.MaxHP ??= type.MaxHP;
                npc.MaxMP ??= type.MaxMP;
                npc.BaseStats ??= type.BaseStats;
                npc.Languages ??= type.Languages;
                npc.DamageResistance ??= type.DamageResistance;
                npc.DamageImmunities ??= type.DamageImmunities;
                npc.ConditionImmunities ??= type.ConditionImmunities;
                npc.RandomDialog ??= type.RandomDialog;
            }
            else if (MainClass.saveGame.Flags.IsDebug)
            {
                Console.WriteLine($"⚠️ NPCType not found for NPC.TypeID={npc.TypeID}");
            }

            // --- Static random dialog (optional) ---
            if (npc.UseStaticRandomDialog)
            {
                // TODO: implement logic
            }
        }

        return data;
    }

}