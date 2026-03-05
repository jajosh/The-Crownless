using System;

public class LocationRepository : GameDataBase
{
	public LocationRepository()
	{

	}

	public static LocationObject? Query(object criteria)
	{
		if (criteria == null)
		{
			BugHunter.Log(DebugType.LOCATIONREPOSITORY, "Criteria is null, nothing to query.", DebugLogSeverity.DEBUG);
			return null; 
		}
		return null;
	}
}
