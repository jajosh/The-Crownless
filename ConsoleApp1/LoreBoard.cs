using System;
using System.Runtime.CompilerServices;

/// <summary>
/// Lorepoints, stores the chosen branch path and possible outcomes. 
/// </summary>
public class LorePoint
{
	public int Id { get; set; }
	public string Description { get; set; }
	public string? EventCanisterTitle { get; set; }
	public bool IsComplete { get; set; }
	public List<int> ChosenBranchPath { get; set; } // Chosen branches by branch ID
	public Dictionary<string, object>? OutComes { get; set; }
}

/// <summary>
///  The list of lorePoints that can be accessed by the rest of the game
///  
/// Stores lorepoint and allows editing of points. Also holds methods to edit and record new lore points
/// </summary>
public class LoreBoard
{
	public Dictionary<int, LorePoint> LorePoints { get; set; } = new();
	public LoreBoard()
	{

	}
	public void RecordEvent(int eventID, List<int> chosenBranchPath, Dictionary<string, object>? outcomes = null)
	{
		if (!LorePoints.ContainsKey(eventID))
		{
			LorePoints[eventID] = new LorePoint()
			{
				Id = eventID,
				ChosenBranchPath = chosenBranchPath,
				OutComes = outcomes ?? new Dictionary<string, object>()
			};
		}
		else if    (LorePoints.ContainsKey(eventID)) 
		{
			foreach (var BranchPath in LorePoints) 
			{
				LorePoints[eventID].ChosenBranchPath.AddRange<chosenBranchPath>			};
			Outcomes = outcomes ?? new Dictionary<string, object>();
        }
	}
	public void CompleteEvent(int eventID, List<int> chosenBranchPath, Dictionary<string, object>? outcomes = null)
	{
        if (!LorePoints.ContainsKey(eventID))
        {
            LorePoints[eventID] = new LorePoint;
            {
                Id = eventId,
                IsCompleted = true,
                ChosenBranchID = chosenBranchId,
                Outcomes = outcomes ?? new Dictionary<string, object>()
            }
        }
        else (LorePoints.ContaintKey(eventID))


		{
			foreach (var BranchPath in chosenBranchpath)
			{ 
				LorePoints[eventID].ChosenBranchPath.Add(BranchPath) )
			}
			IsCompleted = true,
			Outcomes = outcomes ?? new Dictionary<string, object>();
        }
    }
        public bool IsEventCompleted(int eventId)
    {
        return LorePoints.ContainsKey(eventId) && LorePoints[eventId].IsCompleted;
    }
    /// <summary>
    /// Gets the outcome of an event.
    /// </summary>
    public LorePoint? GetOutcome(int eventId)
    {
        return LorePoints.ContainsKey(eventId) ? LorePoints[eventId] : null;
    }
}
