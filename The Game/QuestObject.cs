
using System;
using System.Security.Cryptography.X509Certificates;
public enum QuestStatus
{
    Inactive,
    Started,
    Finished
}
//Base quest definition and quest related methods. 
public class QuestObject
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

    public QuestObject()
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
    public NPCObject TargetNPC { get; set; }
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

