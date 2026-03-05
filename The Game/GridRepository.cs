using System;
using System.CodeDom;
using System.Data.Common;
using System.Drawing.Text;
using System.Numerics;
using System.Reflection;
using System.Reflection.Metadata;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using Microsoft.VisualBasic;

public class GridRepository : GameDataBase
{
    public GridRepository()
    {

    }

    // Simple in-memory cache: Key = "GridX_GridY" (or just verify by GridX/GridY properties)
    private static readonly Dictionary<string, GridObject> _gridCache = new Dictionary<string, GridObject>();

    private static string GenerateCacheKey(int x, int y) => $"{x}_{y}";


    public static GridObject? Query(object criteria)
    {
        // 1. Guard Clause: Stop immediately if null
        if (criteria == null)
        {
            BugHunter.Log(DebugType.GAMEFILE, "Query called with null criteria object.", DebugLogSeverity.FATAL);
            throw new ArgumentNullException(nameof(criteria));
        }

        Type criteriaType = criteria.GetType();
        var whereClauses = new List<string>();
        var parameters = new List<SqliteParameter>();

        foreach (PropertyInfo prop in criteriaType.GetProperties())
        {
            object? value = prop.GetValue(criteria);
            // Ensure we don't try to query by null properties in the anonymous object
            if (value != null)
            {
                string columnName = prop.Name;
                whereClauses.Add($"{columnName} = @{columnName}");
                parameters.Add(new SqliteParameter($"@{columnName}", value));
            }
        }

        // 2. Guard Clause: Stop if no valid filters were found
        if (whereClauses.Count == 0)
        {
            BugHunter.Log(DebugType.GAMEFILE, $"Query attempted with no valid property filters on {criteriaType.Name}", DebugLogSeverity.ERROR);
            return null; // Or throw, depending on if this is expected
        }
        BugHunter.Log(DebugType.LOG, $"{whereClauses.Count} ", DebugLogSeverity.DEBUG);
        string whereClause = string.Join(" AND ", whereClauses);
        
        // --- Cache Check ---
        // Attempt to extract GridX and GridY from criteria to check cache first
        int? checkX = null; 
        int? checkY = null;
        
        try 
        {
            var xProp = criteriaType.GetProperty("GridX");
            var yProp = criteriaType.GetProperty("GridY");
            if (xProp != null && yProp != null)
            {
                checkX = (int?)xProp.GetValue(criteria);
                checkY = (int?)yProp.GetValue(criteria);

                if (checkX.HasValue && checkY.HasValue)
                {
                    string key = GenerateCacheKey(checkX.Value, checkY.Value);
                    if (_gridCache.ContainsKey(key))
                    {
                        // BugHunter.Log(DebugType.LOG, $"Grid Cache Hit: {key}", DebugLogSeverity.DEBUG);
                        return _gridCache[key];
                    }
                }
            }
        }
        catch { /* criteria object might not match expect shape, ignore cache */ }
        // -------------------

        string query = $@"
        SELECT GridObject.*
        FROM GridObject
        WHERE {whereClause}
        LIMIT 1";

        try
        {
            using var connection = new SqliteConnection(new GameDataBase().connectionString);
            connection.Open();
            using var command = new SqliteCommand(query, connection);
            command.Parameters.AddRange(parameters.ToArray());

            using var reader = command.ExecuteReader();
            if (reader.Read())
            {
                var loadedGrid = GameDataBaseReader.MapToGridObject(reader);
                
                // Add to cache if we pulled it by coordinates
                if (loadedGrid != null)
                {
                    string key = GenerateCacheKey(loadedGrid.GridX, loadedGrid.GridY);
                    if (!_gridCache.ContainsKey(key))
                    {
                        _gridCache[key] = loadedGrid;
                    }
                }
                return loadedGrid;
            }
        }
        catch (Exception ex)
        {
            // Capture the SQL and the Error for easier debugging
            string errorMsg = $"SQL Query Failed: {query} | Error: {ex.Message}";
            BugHunter.Log(DebugType.GAMEFILE, errorMsg, DebugLogSeverity.FATAL);
        }

        return null;
    }

    public static async Task<GridObject?> QueryAsync(object criteria, CancellationToken ct = default)
    {
        // 1. Guard Clause: Stop immediately if null
        if (criteria == null)
        {
            BugHunter.Log(DebugType.GAMEFILE, "Query called with null criteria object.", DebugLogSeverity.FATAL);
            throw new ArgumentNullException(nameof(criteria));
        }

        Type criteriaType = criteria.GetType();
        var whereClauses = new List<string>();
        var parameters = new List<SqliteParameter>();

        foreach (PropertyInfo prop in criteriaType.GetProperties())
        {
            object? value = prop.GetValue(criteria);
            // Ensure we don't try to query by null properties in the anonymous object
            if (value != null)
            {
                string columnName = prop.Name;
                whereClauses.Add($"{columnName} = @{columnName}");
                parameters.Add(new SqliteParameter($"@{columnName}", value));
            }
        }

        // 2. Guard Clause: Stop if no valid filters were found
        if (whereClauses.Count == 0)
        {
            BugHunter.Log(DebugType.GAMEFILE, $"Query attempted with no valid property filters on {criteriaType.Name}", DebugLogSeverity.ERROR);
            return null; // Or throw, depending on if this is expected
        }
        BugHunter.Log(DebugType.LOG, $"{whereClauses.Count} ", DebugLogSeverity.DEBUG);
        string whereClause = string.Join(" AND ", whereClauses);

        // --- Cache Check (Async version) ---
        int? checkX = null; 
        int? checkY = null;
        try 
        {
            var xProp = criteriaType.GetProperty("GridX");
            var yProp = criteriaType.GetProperty("GridY");
            if (xProp != null && yProp != null)
            {
                checkX = (int?)xProp.GetValue(criteria);
                checkY = (int?)yProp.GetValue(criteria);

                if (checkX.HasValue && checkY.HasValue)
                {
                    string key = GenerateCacheKey(checkX.Value, checkY.Value);
                    if (_gridCache.ContainsKey(key))
                    {
                        return _gridCache[key];
                    }
                }
            }
        }
        catch { }
        // -------------------

        string query = $@"
        SELECT GridObject.*
        FROM GridObject
        WHERE {whereClause}
        LIMIT 1";

        try
        {
            using var connection = new SqliteConnection(new GameDataBase().connectionString);
            connection.Open();
            using var command = new SqliteCommand(query, connection);
            command.Parameters.AddRange(parameters.ToArray());

            using var reader = command.ExecuteReader();
            if (await reader.ReadAsync(ct))
            {
                var loadedGrid = GameDataBaseReader.MapToGridObject(reader);
                 if (loadedGrid != null)
                {
                    string key = GenerateCacheKey(loadedGrid.GridX, loadedGrid.GridY);
                    if (!_gridCache.ContainsKey(key))
                    {
                        _gridCache[key] = loadedGrid;
                    }
                }
                return loadedGrid;
            }
        }
        catch (Exception ex)
        {
            // Capture the SQL and the Error for easier debugging
            string errorMsg = $"SQL Query Failed: {query} | Error: {ex.Message}";
            BugHunter.Log(DebugType.GAMEFILE, errorMsg, DebugLogSeverity.FATAL);
        }

        return null;
    }

    public void SaveGridToDataBase(List<GridObject> grids)
    {

        using var connection = new SqliteConnection(new GameDataBase().connectionString);
        connection.Open();

        using var transaction = connection.BeginTransaction();

        GridObject currentGrid = null;
        BugHunter.Log(DebugType.GRIDPROCESSING, $"Total grids in the gridKey = {grids.Count}", DebugLogSeverity.DEBUG);
        try
        {
            foreach (var grid in grids)
            {
                currentGrid = grid;

                BugHunter.Log(DebugType.GRIDREPOSITORY, $"currentGrid location ID = {currentGrid.LocationID}", DebugLogSeverity.DEBUG); // !!! Debug !!!

                currentGrid.GridID = InsertMainGrid(currentGrid, connection, transaction);
                
                // Update Cache immediately
                string key = GenerateCacheKey(currentGrid.GridX, currentGrid.GridY);
                _gridCache[key] = currentGrid;

                if (currentGrid.DescriptionEntries != null && currentGrid.DescriptionEntries.Count > 0)
                {

                    BugHunter.Log(DebugType.GRIDPROCESSING, $"CurrentGrid ID = {currentGrid.GridID} | Adding related descriptions");
                    DescriptionRepository.InsertDescriptions(currentGrid.GridID, "Grid", currentGrid.DescriptionEntries, connection, transaction);
                    BugHunter.Log(DebugType.GRIDPROCESSING, $"CurrentGrid ID = {currentGrid.GridID} | Descroptions have been added.");
                }
            }
            transaction.Commit();
            BugHunter.Log(DebugType.GRIDPROCESSING, "Grids have been saved to sql", DebugLogSeverity.INFO);
        }
        catch (Exception ex)
        {
            transaction.Rollback();
            string gridInfo = currentGrid != null ? $"{currentGrid.GridID} = ID, {currentGrid.GridMapKey.Count} = GridMapLength,{currentGrid.GridX}, {currentGrid.GridY}, {currentGrid.Biome} = grid.Biome, {currentGrid.SubBiome} = gird.SubBiome, {currentGrid.RandomEventChance} = grid.RandomEventChance" : "unknown";

            BugHunter.Log(DebugType.GRIDPROCESSING, $"Error saving grid to database at {gridInfo} | Error: {ex.Message}", DebugLogSeverity.FATAL);
            BugHunter.Log(DebugType.GRIDPROCESSING, $"{currentGrid.DescriptionEntries.Count} descriptions linked to this grid.");
        }
    }

    private int InsertMainGrid(GridObject grid, SqliteConnection connection, SqliteTransaction transaction)
    {
        string sql = @"
                INSERT OR REPLACE INTO GridObject (GridX, GridY, LocationID, Biome, SubBiome, RandomEventChance, GridMapKey)
                VALUES (@GridX, @GridY, @LocationID, @Biome, @SubBiome, @RandomEventChance, @GridMapKey);
                SELECT last_insert_rowid();";
        using (var command = new SqliteCommand(sql, connection, transaction))
        {
            command.Parameters.AddWithValue("@GridX", grid.GridX);
            command.Parameters.AddWithValue("@GridY", grid.GridY);
            command.Parameters.AddWithValue("@LocationID", grid.LocationID);
            command.Parameters.AddWithValue("@Biome", grid.Biome.ToString());
            command.Parameters.AddWithValue("@SubBiome", grid.SubBiome.ToString());
            command.Parameters.AddWithValue("@RandomEventChance", grid.RandomEventChance);
            byte[] GridMapKey = JsonLoader.SerializeJsonBlob(grid.GridMapKey);
            command.Parameters.AddWithValue("@GridMapKey", GridMapKey);

            long newId = (long)command.ExecuteScalar();
            return (int)newId;
        }
    }

}
