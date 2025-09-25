using System;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;

/// <summary>
/// Lorepoints, stores the chosen branch path and possible outcomes. Used to track individual points of story knowledge that may need to be called upon 
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
	/// <summary>
	///  Adds and updates a lorepoint
	/// </summary>
	/// <param name="eventID"> ID to add or find</param>
	/// <param name="chosenBranchPath"> The dialog tree of paths taken</param>
	/// <param name="outcomes">  Any outcomes (the leaves)</param>
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
                LorePoints[eventID].ChosenBranchPath.AddRange(chosenBranchPath);
                LorePoints[eventID].Id = eventID;
				LorePoints[eventID].ChosenBranchPath = chosenBranchPath;
				LorePoints[eventID].OutComes = outcomes ?? new Dictionary<string, object>();
            }
        }
	}
    /// <summary>
    /// add or update an event as completed. Changes IsComplete to true.
    /// </summary>
    /// <param name="eventID"> ID to add or find</param>
    /// <param name="chosenBranchPath"> The dialog tree of paths taken</param>
    /// <param name="outcomes">  Any outcomes (the leaves)</param>
    public void CompleteEvent(int eventID, List<int> chosenBranchPath, Dictionary<string, object>? outcomes = null)
	{
		if (!LorePoints.ContainsKey(eventID))
		{
			LorePoints[eventID] = new LorePoint
			{
				Id = eventID,
				IsComplete = true,
				ChosenBranchPath = chosenBranchPath,
				OutComes = outcomes ?? new Dictionary<string, object>()
			};
		}
		else if(LorePoints.ContainsKey(eventID))
		{

			LorePoints[eventID].IsComplete = true;
            LorePoints[eventID].ChosenBranchPath.AddRange(chosenBranchPath);
            LorePoints[eventID].Id = eventID;
			LorePoints[eventID].ChosenBranchPath = chosenBranchPath;
				LorePoints[eventID].OutComes = outcomes ?? new Dictionary<string, object>();
        }
    }
	/// <summary>
	/// Returns true if the event is completed. What else would it do?
	/// </summary>
	/// <param name="eventId"></param>
	/// <returns></returns>
    public bool IsEventCompleted(int eventId)
	{
    return LorePoints.ContainsKey(eventId) && LorePoints[eventId].IsComplete;
	}
    /// <summary>
    /// Gets the outcome of an event.
    /// </summary>
    public LorePoint? GetOutcome(int eventId)
    {
        return LorePoints.ContainsKey(eventId) ? LorePoints[eventId] : null;
    }
}
