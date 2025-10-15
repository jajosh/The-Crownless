using System;

/// <summary>
/// The engine that handles and does logic around the locations
/// </summary>
public class EngineLocation
{

	public EngineLocation()
	{
		//
		// TODO: Add constructor logic here
		//
	}
	/// <summary>
	/// The forloop that calls the genertor to make each room or grid
	/// </summary>
	/// <param name="location"></param>
	/// <param name="currentGrid"></param>
	/// <param name="iterationAmount"></param>
	/// <returns></returns>
	public List<Grid> GenerateGrid(LocationType type,Grid currentGrid, int iterationAmount = 1)
	{
		List<Grid> results = new List<Grid>();
		Grid sample = new Grid();
		// Perception check here 
		for (int x = 0; x < iterationAmount; x++)
		{
			sample = TheGenerator(type, currentGrid);
			results.Add(sample);
			currentGrid = sample;
		}
		return results;
	}
	public Grid TheGenerator(LocationType location, Grid currentGrid)
	{
		return new Grid();
	}
}
