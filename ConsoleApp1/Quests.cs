using GameNamespace;
using System;
using System.Security.Cryptography.X509Certificates;
using Windows.Foundation.Collections;
using Windows.Media.Protection;
using Windows.UI.Text;

//Base quest definition and quest related methods. 
public class Quest
{
    public int ID { get; set; }
    public string? Title { get; set; }
    public bool IsComplete { get; set; }
    public QuestStatus Status { get; set; }
    public string? Description { get; set; }

    // Composable Subtypes
    public FetchQuestData? FetchData { get; set; }
    public KillQuestData? KillData { get; set; }
    public SpeakQuestData? SpeakData { get; set; }
    public DefendQuestData? DefendData { get; set; }

    public Quest()
    {
        ID = 0;
        Title = string.Empty;
        IsComplete = false;
        Status = QuestStatus.Inactive;
        Description = string.Empty;
    }
}
public class FetchQuestData
{
    public List<int> NeededItems { get; set; } = new();
    public NPC TargetNPC { get; set; }
}

public class KillQuestData
{
    public List<int> NeededItems { get; set; } = new();
    public List<int> TargetNPCs { get; set; } = new();
    public int TurnInNPC { get; set; }
}

public class SpeakQuestData
{
    public List<int> TargetNPCsID { get; set; } = new();
}

public class DefendQuestData
{
    public List<int> TargetNPCs { get; set; } = new();
    public List<int> Monsters { get; set; } = new();
}


//Loads the database of quests from a text file.
public class QuestDatabase()
{

    Quest quest = new Quest();

    public static Quest? FindQuestByIQ(int questId)
    {
        Quest? foundQuest = FileManager.Quests.Find(q => q.ID == questId);

        if (foundQuest != null)
        {
            Console.WriteLine($"Quest Found: {foundQuest.Title}");
            return foundQuest;
        }
        else
        {
            Console.WriteLine("Quest not found");
            return null;
        }
    }
    public static Quest? findQuestByTitle(string TitleToFind)
    {
        if (string.IsNullOrWhiteSpace(TitleToFind))
        {
            Console.WriteLine("Invalid Quest.");
            return null;
        }
        Quest? foundQuest = FileManager.Quests.Find(item => item.Title.Equals(TitleToFind, StringComparison.OrdinalIgnoreCase));
        if (foundQuest != null)
        {
            Console.WriteLine($"Item Found: {foundQuest.Title}");
            return foundQuest;
        }
        else
        {
            Console.WriteLine($"Item Not Found: {foundQuest}");
            return null;
        }
    }
}