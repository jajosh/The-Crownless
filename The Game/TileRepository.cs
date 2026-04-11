using Microsoft.Data.Sqlite;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Threading.Tasks;
using static System.Windows.Forms.AxHost;

public class TileRepository : GameDataBase
{
    public TileRepository()
    {

    }
    #region === Query Methods ===
    public static List<TileObject> QueryGridTiles(int gridX, int gridY)
    {
        BugHunter.Log(DebugType.SQL, "Running Tile batch qeury");
        List<TileObject> result = new List<TileObject>();
        string query = @"
                    SELECT TileObject.*
                    FROM TileObject
                    WHERE GridX = @GridX
                      AND GridY = @GridY";
        using (var connection = new SqliteConnection(new GameDataBase().connectionString))
        {
            connection.Open();
            using (var cmd = new SqliteCommand(query, connection)) // Fixed to SqliteCommand
            {
                cmd.Parameters.AddWithValue("@GridX", gridX);
                cmd.Parameters.AddWithValue("@GridY", gridY);
                using (var reader = cmd.ExecuteReader()) // Fixed to SqliteDataReader if needed, but works
                {
                    int x = 0;
                    while (reader.Read())
                    {
                        // BugHunter.Log(DebugType.SQL, $"Tile Found, Tile # {x}", DebugLogSeverity.DEBUG);
                        result.Add(GameDataBaseReader.MapToTileObject(reader));
                        if (x == 1000 || x == 1200 || x == 1275)
                            BugHunter.Log(DebugType.SQL, $" Total Tiles found in Query = {x}");
                        x++;
                    }
                }

            }
            connection.Close();
        } // Connection auto-closes/disposes here
        BugHunter.Log(DebugType.MAPRENDERING, $"GridQuery result length = {result.Count()}");
        return result;
    }
    public static async Task<List<TileObject>> QueryGridTilesAsync(int gridX, int gridY)
    {
        BugHunter.Log(DebugType.SQL, "Running Tile batch query (async)");

        List<TileObject> result = new List<TileObject>();

        string query = @"
        SELECT TileObject *
        FROM TileObject
        WHERE GridX = @GridX
          AND GridY = @GridY";

        using (var connection = new SqliteConnection(new GameDataBase().connectionString))
        {
            await connection.OpenAsync();

            using (var cmd = new SqliteCommand(query, connection))
            {
                cmd.Parameters.AddWithValue("@GridX", gridX);
                cmd.Parameters.AddWithValue("@GridY", gridY);

                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    int x = 0;
                    while (await reader.ReadAsync())
                    {
                        result.Add(GameDataBaseReader.MapToTileObject(reader));

                        if (x == 1000 || x == 1200 || x == 1275)
                            BugHunter.Log(DebugType.SQL, $" Total Tiles found in Query = {x}");

                        x++;
                    }
                }
            }
        }

        BugHunter.Log(DebugType.MAPRENDERING, $"GridQuery result length = {result.Count}");
        return result;
    }
    public static TileObject Query(object criteria)
    {
        if (criteria == null)
        {
            BugHunter.Log(
                DebugType.TILEPROCESSING,
                $"Search Criteria is null | Exception: {nameof(ArgumentNullException)}",
                DebugLogSeverity.FATAL
            );
        }

        Type criteriaType = criteria.GetType();
        var whereClauses = new List<string>();
        var parameters = new List<SqliteParameter>();

        foreach (PropertyInfo prop in criteriaType.GetProperties())
        {
            object? value = prop.GetValue(criteria);
            if (value != null)
            {
                string columnName = prop.Name;
                whereClauses.Add($"TileObject.{columnName} = @{columnName}");
                parameters.Add(new SqliteParameter($"@{columnName}", value));
            }
        }

        if (whereClauses.Count == 0)
        {
            BugHunter.Log(
                DebugType.TILEPROCESSING,
                $"Failure to fill whereClauses | Exception: {nameof(ArgumentNullException)}",
                DebugLogSeverity.FATAL
            );
        }

        string whereClause = string.Join(" AND ", whereClauses);
        string query = $@"
            SELECT TileObject.*
            FROM TileObject
            WHERE {whereClause}
            LIMIT 1";

        TileObject? tile = null;

        using (var connection = new SqliteConnection(new GameDataBase().connectionString))
        {
            connection.Open();

            // 1. Load the main tile row
            using (var cmd = new SqliteCommand(query, connection))
            {
                cmd.Parameters.AddRange(parameters.ToArray());
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        tile = GameDataBaseReader.MapToTileObject(reader);
                    }
                }
            }

            if (tile == null) return null;

            // 2. Load components from the separate TileComponents table
            string compQuery = "SELECT * FROM TileComponents WHERE TileID = @TileID";
            using (var compCmd = new SqliteCommand(compQuery, connection))
            {
                compCmd.Parameters.AddWithValue("@TileID", tile.TileId);
                using (var reader = compCmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        tile.Components.Add(GameDataBaseReader.MapToComponents(reader));
                    }
                }
            }
        }

        return tile;
    }
    public static async Task<TileObject?> QueryAsync(object criteria, CancellationToken ct = default)
    {
        if (criteria == null)
        {
            BugHunter.Log(
                DebugType.TILEPROCESSING,
                $"Search Criteria is null | Exception: {nameof(ArgumentNullException)}",
                DebugLogSeverity.FATAL
            );
        }

        // 1. Build the query (Keep this synchronous, it's just string manipulation)
        Type criteriaType = criteria.GetType();
        var whereClauses = new List<string>();
        var parameters = new List<SqliteParameter>();

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

        if (whereClauses.Count == 0)
        {
            BugHunter.Log(
                DebugType.TILEPROCESSING,
                $"Failure to fill whereClases | Exception: {nameof(ArgumentNullException)}",
                DebugLogSeverity.FATAL
            );
        }

        string whereClause = string.Join(" AND ", whereClauses);
        string query = $@"
        SELECT TileObject.* FROM TileObject 
        LEFT JOIN DescriptionEntry ON TileObject.TileID = DescriptionEntry.TypeID 
        AND DescriptionEntry.DescriptionType = 'Tile'
        LEFT JOIN TileComponents ON TileObject.TileID = TileComponents.TileID
        LEFT JOIN TileProperties ON TileObject.TileID = TileProperties.TileID
        WHERE {whereClause} 
        LIMIT 1";

        // 2. Execute Asynchronously
        try
        {
            // Use your connection factory/property
            using var connection = new SqliteConnection(new GameDataBase().connectionString);

            // OpenAsync takes the cancellation token
            await connection.OpenAsync(ct);

            using var cmd = new SqliteCommand(query, connection);
            cmd.Parameters.AddRange(parameters.ToArray());

            // ExecuteReaderAsync returns the reader without blocking the thread
            using var reader = await cmd.ExecuteReaderAsync(ct);

            if (await reader.ReadAsync(ct))
            {
                // Note: MapToTileObject likely remains synchronous unless it does more DB calls
                return GameDataBaseReader.MapToTileObject(reader);
            }
        }
        catch (OperationCanceledException)
        {
            // This is expected when the player moves or the task is cancelled
            return null;
        }
        catch (Exception ex)
        {
            BugHunter.Log(
                DebugType.TILEPROCESSING,
                $"Failed to cancel | Exception: {ex}",
                DebugLogSeverity.FATAL
            );
        }

        return null;
    }
    #endregion
    public TileObject GetTileFullObject(int tileId, string connectionstring)
    {
        TileObject tile = null;

        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            // -------------------------
            // --- Get the Main Tile ---
            // -------------------------
            string tileSql = "SELECT *, BaseRender FROM TileObject WHERE TileID = @TileID";
            using (var command = new SqliteCommand(tileSql, connection))
            {
                command.Parameters.AddWithValue("@TileID", tileId);
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        tile = GameDataBaseReader.MapToTileObject(reader);
                        tile.Components = new List<TileComponents>();
                        tile.Properties = new List<TileProperties>();
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            // --------------------------
            // --- Get the Components ---
            // --------------------------
            string compSql = "SELECT * FROM TileComponents WHERE TileID = @TileID";
            using (var command = new SqliteCommand(compSql, connection))
            {
                command.Parameters.AddWithValue("@TileID", tileId);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        tile.Components.Add(GameDataBaseReader.MapToComponents(reader));
                    }
                }
            }
            // --------------------------
            // --- Get the Properties ---
            // --------------------------
            compSql = "SELECT * FROM TileProperties WHERE TileID = @TileID";
            using (var command = new SqliteCommand(compSql, connection))
            {
                command.Parameters.AddWithValue("@TileID", tileId);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        tile.Properties.Add(GameDataBaseReader.MapToProperties(reader));
                    }
                }
            }
            // --------------------------
            // --- Get the Descriptions ---
            // --------------------------
            string descSql = "SELECT * FROM DescriptionEntry WHERE TypeID = @TileID AND DescriptionType = 'Tile'";
            using (var command = new SqliteCommand(descSql, connection))
            {
                command.Parameters.AddWithValue("@TileID", tileId);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        tile.Description.Add(GameDataBaseReader.MapToDescription(reader));
                    }
                }
            }
        }
        return tile;
    }

    public void SaveAllTilesToDatabase(List<TileObject> tiles)
    {
        using var connection = new SqliteConnection(new GameDataBase().connectionString);
        connection.Open();

        // Start ONE transaction for the entire map
        using var transaction = connection.BeginTransaction();

        // Track current tile for the catch block
        TileObject? currentTile = null;

        try
        {
            foreach (var tile in tiles)
            {
                currentTile = tile;
                // 1. Insert Main Tile (UPSERT)
                int tileId = InsertMainTile(currentTile, connection, transaction);

                // 2. Clear existing child records to prevent duplication bloat
                DeleteExistingTileChildren(tileId, connection, transaction);

                // 3. Insert fresh Components
                InsertComponents(tileId, currentTile.Components, connection, transaction);

                // 4. Insert fresh Properties
                InsertProperties(tileId, currentTile.Properties, connection, transaction);

                // 5. Insert fresh Descriptions
                if (currentTile.Description != null && currentTile.Description.Count > 0)
                {
                    DescriptionRepository.InsertDescriptions(tileId, "Tile", currentTile.Description, connection, transaction);
                }

            }

            // Commit ONCE after the loop finishes successfully
            transaction.Commit();
            BugHunter.Log(DebugType.GAMEFILE, $"Successfully saved {tiles.Count} tiles.");
        }
        catch (Exception ex)
        {
            // If ANY tile fails, the whole save is rolled back to prevent a corrupt world
            transaction.Rollback();

            string tileInfo = currentTile != null
                ? $"{currentTile.BaseRender.CharData.MainChar} at ({currentTile.GridX},{currentTile.GridY},{currentTile.LocalX},{currentTile.LocalY})"
                : "Unknown";

            BugHunter.Log(
                DebugType.TILEPROCESSING,
                $"FATAL: Database Save Failed. All changes rolled back. Failed on Tile: {tileInfo} | Error: {ex.Message}",
                DebugLogSeverity.FATAL
            );
        }
    }

    private int InsertMainTile(TileObject tile, SqliteConnection connection, SqliteTransaction transaction)
    {
        string sql = @"
        INSERT INTO TileObject (GridX, GridY, LocalX, LocalY, TileType, BaseRender)
        VALUES (@GridX, @GridY, @LocalX, @LocalY, @TileType, @BaseRender)
        ON CONFLICT(GridX, GridY, LocalX, LocalY) DO UPDATE SET
            TileType   = excluded.TileType,
            BaseRender = excluded.BaseRender;";

        using (var command = new SqliteCommand(sql, connection, transaction))
        {
            command.Parameters.AddWithValue(@"GridX", tile.GridX);
            command.Parameters.AddWithValue(@"GridY", tile.GridY);
            command.Parameters.AddWithValue(@"LocalX", tile.LocalX);
            command.Parameters.AddWithValue(@"LocalY", tile.LocalY);
            command.Parameters.AddWithValue(@"TileType", tile.TileType.ToString());

            byte[] jsonString = JsonLoader.SerializeJsonBlob<TileRenderProfile>(tile.BaseRender);
            command.Parameters.AddWithValue(@"BaseRender", jsonString);

            command.ExecuteNonQuery();
        }

        // Directly query for the ID to be 100% robust against edge cases with last_insert_rowid() during UPSERT
        string idSql = "SELECT TileID FROM TileObject WHERE GridX=@GridX AND GridY=@GridY AND LocalX=@LocalX AND LocalY=@LocalY;";
        using (var idCmd = new SqliteCommand(idSql, connection, transaction))
        {
            idCmd.Parameters.AddWithValue("@GridX", tile.GridX);
            idCmd.Parameters.AddWithValue("@GridY", tile.GridY);
            idCmd.Parameters.AddWithValue("@LocalX", tile.LocalX);
            idCmd.Parameters.AddWithValue("@LocalY", tile.LocalY);

            object result = idCmd.ExecuteScalar();
            if (result == null || result == DBNull.Value)
            {
                BugHunter.Log(DebugType.TILEREPOSITORY, new InvalidOperationException("Failed to retrieve TileID after upsert"), DebugLogSeverity.FATAL);
                return -1;
            }

            return (int)(long)result;
        }
    }

    private void DeleteExistingTileChildren(int tileId, SqliteConnection connection, SqliteTransaction transaction)
    {
        // Clear components and properties. Descriptions are handled in DescriptionRepository.InsertDescriptions
        using (var cmd1 = new SqliteCommand("DELETE FROM TileComponents WHERE TileID = @TileID", connection, transaction))
        {
            cmd1.Parameters.AddWithValue("@TileID", tileId);
            cmd1.ExecuteNonQuery();
        }

        using (var cmd2 = new SqliteCommand("DELETE FROM TileProperties WHERE TileID = @TileID", connection, transaction))
        {
            cmd2.Parameters.AddWithValue("@TileID", tileId);
            cmd2.ExecuteNonQuery();
        }
    }
    private void InsertComponents(int tileId, List<TileComponents> components, SqliteConnection connection, SqliteTransaction transaction)
    {
        const string sql = @"
        INSERT INTO TileComponents (TileID, TypeName, Data)
        VALUES (@TileID, @TypeName, @Data);";

        using var command = new SqliteCommand(sql, connection, transaction);
        command.Parameters.Add("@TileID", SqliteType.Integer);
        command.Parameters.Add("@TypeName", SqliteType.Text);
        command.Parameters.Add("@Data", SqliteType.Blob);

        foreach (var component in components)
        {
            if (component?.TileComponent == null)
                continue;
            TileComponent comp = component.TileComponent;
            Type runtimeType = comp.GetType();
            byte[] componentData = JsonLoader.SerializeJsonBlob(comp, runtimeType);

            command.Parameters["@TileID"].Value = tileId;
            command.Parameters["@TypeName"].Value = component.TileComponent.GetType().Name;
            command.Parameters["@Data"].Value = componentData;

            command.ExecuteNonQuery();
        }
    }
    private void InsertProperties(int tileId, List<TileProperties> properties, SqliteConnection connection, SqliteTransaction transaction)
    {
        string sql = @"
            INSERT INTO TileProperties (TileID, TilePropertyName, TileProperties)
            VALUES (@TileID, @TilePropertyName, @TileProperties);";

        using (var command = new SqliteCommand(sql, connection, transaction))
        {
            foreach (var property in properties)
            {
                command.Parameters.Clear();
                command.Parameters.AddWithValue("@TileID", tileId);
                command.Parameters.AddWithValue("@TilePropertyName", property.TilePropertyName);
                command.Parameters.AddWithValue("@TileProperties", property.TileProperty.ToString());

                command.ExecuteNonQuery();

            }
        }
    }

    public int GetTileCount(string connectionString)
    {
        string sql = "SELECT COUNT(*) FROM TileObject;";

        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            using (var command = new SqliteCommand(sql, connection))
            {
                // ExecuteScalar retrieves the single result (the count)
                long count = (long)command.ExecuteScalar();
                return (int)count;
            }
        }
    }
}