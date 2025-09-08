using System;
using System.Text.Json;
using Windows.Networking.Proximity;
using Windows.Networking.Sockets;

public class NPC
{
    public int ID {  get; set; }
    public string Name { get; set; }
    public Dictionary<Skill, int> Skills { get; set; } = new();
    public Dictionary<NPCTriggerActions, Action>? TriggerActions { get; set; } = new();
    public List<Item>? Inventory { get; set; } = new();
    public List<Item>? Armor { get; set; } = new();
    public List<string>? RandomBattleDialog { get; set; } = new() { "My dialog hasn't been set, but I'll kill you anyway" };
    public NPCTypeData? NPCType { get; set; }
    public NPCNamedData? NPCNamedData { get; set; }
    public List<string> PossibleCombatSymbols { get; set; }

    public NPC()
    {
        
    }
}
public class NPCNamedData
{
    public List<int>? RandomDialogTrees { get; set; }
}
public class NPCTypeData
{
    public List<string>? PossibleNames { get; set; }
}

public class NPCTypeLoader
{
    NPC npcReference = new NPC();
    public static List<NPC>? NPCDataBase { get; set; }
    public NPCTypeLoader(string path)
    {
           
    }
    public static NPC? FindNPCTypeByIQ(int NPCTypeID)
    {
        NPC? foundNPC = NPCDataBase.Find(q => q.ID == NPCTypeID);

        if (foundNPC != null)
        {
            Console.WriteLine($"NPC Found: {foundNPC.Name}");
            return foundNPC;
        }
        else
        {
            Console.WriteLine("NPC not found");
            return null;
        }
    }
    public static NPC? findNPCTypeByName(string NPCtypeNameToFind)
    {
        if (string.IsNullOrWhiteSpace(NPCtypeNameToFind))
        {
            Console.WriteLine("Invalid NPCType.");
            return null;
        }
        NPC? foundNPC = NPCDataBase.Find(item => item.Name.Equals(NPCtypeNameToFind, StringComparison.OrdinalIgnoreCase));
        if (foundNPC != null)
        {
            Console.WriteLine($"NPCType Found: {foundNPC.Name}");
            return foundNPC;
        }
        else
        {
            Console.WriteLine($"NPCType Not Found: {foundNPC}");
            return null;
        }
    }
}
public class NPCNamedLoader
{
    NPC npcReference = new NPC();
    public static List<NPC>? DataBase { get; set; }
    
    public static NPC? FindNPCByIQ(int NPCID)
    {
        NPC? foundNPC = DataBase.Find(q => q.ID == NPCID);

        if (foundNPC != null)
        {
            Console.WriteLine($"NPC Found: {foundNPC.Name}");
            return foundNPC;
        }
        else
        {
            Console.WriteLine("NPC not found");
            return null;
        }
    }
    public static NPC? findNPCByName(string NPCNameToFind)
    {
        if (string.IsNullOrWhiteSpace(NPCNameToFind))
        {
            Console.WriteLine("Invalid NPCType.");
            return null;
        }
        NPC? foundNPC = DataBase.Find(item => item.Name.Equals(NPCNameToFind, StringComparison.OrdinalIgnoreCase));
        if (foundNPC != null)
        {
            Console.WriteLine($"NPCType Found: {foundNPC.Name}");
            return foundNPC;
        }
        else
        {
            Console.WriteLine($"NPCType Not Found: {foundNPC}");
            return null;
        }
    }
}
