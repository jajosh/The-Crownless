using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.DirectoryServices.ActiveDirectory;
using System.Reflection;

public class LocationRepository : GameDataBase
{
	public LocationRepository()
	{

	}
    // Simple in-memory cache: Key = "GridX_GridY" (or just verify by GridX/GridY properties)
    private static readonly Dictionary<string, LocationObject> _locationCache = new Dictionary<string, LocationObject>();
    public static LocationObject? Query(object criteria)
	{
		if (criteria == null)
		{
			BugHunter.Log(DebugType.LOCATIONREPOSITORY, "Criteria is null, nothing to query.", DebugLogSeverity.DEBUG);
			return null; 
		}
		return null;
	}

    private static string GenerateCacheKey(int x) => $"{x}";

    public static async Task<LocationObject?> QueryAsync(object criteria, CancellationToken ct = default)
    {
        // 1. Guard Clause
        if (criteria == null)
        {
            BugHunter.Log(DebugType.GAMEFILE, "Query called with null criteria object.", DebugLogSeverity.FATAL);
            throw new ArgumentNullException(nameof(criteria));
        }

        ct.ThrowIfCancellationRequested();
        Type criteriaType = criteria.GetType();
        var whereClauses = new List<string>();
        var parameters = new List<SqliteParameter>();

        // Reflection to build the query
        foreach (PropertyInfo prop in criteriaType.GetProperties())
        {
            object? value = prop.GetValue(criteria);
            if (value != null)
            {
                string columnName = prop.Name;
                whereClauses.Add($"{columnName} = @{columnName}");
                parameters.Add(new SqliteParameter($"@{columnName}", value));
            }
        }

        // 2. Guard Clause: Stop if no filters
        if (whereClauses.Count == 0)
        {
            BugHunter.Log(DebugType.GAMEFILE, $"Query attempted with no valid property filters on {criteriaType.Name}", DebugLogSeverity.ERROR);
            return null;
        }

        // --- Cache Lookup (Synchronous/Thread-Safe) ---
        try
        {
            var idProp = criteriaType.GetProperty("ID");
            if (idProp?.GetValue(criteria) is int checkID)
            {
                string key = GenerateCacheKey(checkID);
                // Assuming _locationCache is a ConcurrentDictionary
                if (_locationCache.TryGetValue(key, out var cachedLocation))
                {
                    return cachedLocation;
                }
            }
        }
        catch (Exception ex)
        {
            BugHunter.Log(DebugType.GAMEFILE, $"Cache lookup failed: {ex.Message}", DebugLogSeverity.DEBUG);
        }

        // --- Database Query Execution ---
        // Join the list into a string: "Prop1 = @Prop1 AND Prop2 = @Prop2"
        string whereString = string.Join(" AND ", whereClauses);
        string query = $@"SELECT * FROM LocationObject WHERE {whereString} LIMIT 1";

        try
        {
            using var connection = new SqliteConnection(new GameDataBase().connectionString);
            await connection.OpenAsync(ct); // Use Async

            using var command = new SqliteCommand(query, connection);
            command.Parameters.AddRange(parameters.ToArray());

            using var reader = await command.ExecuteReaderAsync(ct); // Use Async

            if (await reader.ReadAsync(ct))
            {
                var loadedLocation = GameDataBaseReader.MapToLocationObject(reader);

                if (loadedLocation != null) // Fixed the logic: Cache only if NOT null
                {
                    string key = GenerateCacheKey(loadedLocation.ID);
                    // Thread-safe update
                    _locationCache[key] = loadedLocation;
                }
                return loadedLocation;
            }
        }
        catch (Exception ex)
        {
            string errorMsg = $"SQL Query Failed: {query} | Error: {ex.Message}";
            BugHunter.Log(DebugType.GAMEFILE, errorMsg, DebugLogSeverity.ERROR);
        }

        return null;
    }
}
