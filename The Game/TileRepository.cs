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
                    WHERE RootGridX = @RootGridX
                      AND RootGridY = @RootGridY";
        using (var connection = new SqliteConnection(new GameDataBase().connectionString))
        {
            connection.Open();
            using (var cmd = new SqliteCommand(query, connection)) // Fixed to SqliteCommand
            {
                cmd.Parameters.AddWithValue("@RootGridX", gridX);
                cmd.Parameters.AddWithValue("@RootGridY", gridY);
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
        SELECT TileObject.*
        FROM TileObject
        WHERE RootGridX = @RootGridX
          AND RootGridY = @RootGridY";

        using (var connection = new SqliteConnection(new GameDataBase().connectionString))
        {
            await connection.OpenAsync();

            using (var cmd = new SqliteCommand(query, connection))
            {
                cmd.Parameters.AddWithValue("@RootGridX", gridX);
                cmd.Parameters.AddWithValue("@RootGridY", gridY);

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
        if (criteria == null) // Null check
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
            SELECT TileObject.* 
            FROM TileObject 
            LEFT JOIN DescriptionEntry ON TileObject.TileID = DescriptionEntry.TypeID 
            AND DescriptionEntry.DescriptionType = 'Tile'
            LEFT JOIN TileComponents ON TileObject.TileID = TileComponents.TileID
            LEFT JOIN TileProperties ON TileObject.TileID = TileProperties.TileID
            WHERE {whereClause} 
            LIMIT 1";

        using (var connection = new SqliteConnection(new GameDataBase().connectionString))
        {
            connection.Open();
            using (var cmd = new SqliteCommand(query, connection)) // Fixed to SqliteCommand
            {
                cmd.Parameters.AddRange(parameters.ToArray());
                using (var reader = cmd.ExecuteReader()) // Fixed to SqliteDataReader if needed, but works
                {
                    if (reader.Read())
                    {
                        return GameDataBaseReader.MapToTileObject(reader);
                    }
                }

            }
        } // Connection auto-closes/disposes here

        return null;
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
                // 1. Insert Main Tile
                int newTileId = InsertMainTile(currentTile, connection, transaction);

                // 2. Insert Components
                InsertComponents(newTileId, currentTile.Components, connection, transaction);

                // 3. Insert Properties
                InsertProperties(newTileId, currentTile.Properties, connection, transaction);

                // 4. Insert Descriptions
                if (currentTile.Description != null && currentTile.Description.Count > 0)
                {
                    DescriptionRepository.InsertDescriptions(newTileId, "Tile", currentTile.Description, connection, transaction);
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
                ? $"{currentTile.BaseRender.CharData.MainChar} at ({currentTile.RootGridX},{currentTile.RootGridY},{currentTile.RootLocalX},{currentTile.RootLocalY})"
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
            INSERT OR REPLACE INTO TileObject (RootGridX, RootGridY, RootLocalX, RootLocalY, TileType, BaseRender)
            VALUES (@RootGridX, @RootGridY, @RootLocalX, @RootLocalY, @TileType, @BaseRender);
            SELECT last_insert_rowid();";

        using (var command = new SqliteCommand(sql, connection, transaction))
        {
            // 1. Map scalar properties
            command.Parameters.AddWithValue(@"RootGridX", tile.RootGridX);
            command.Parameters.AddWithValue(@"RootGridY", tile.RootGridY);
            command.Parameters.AddWithValue(@"RootLocalX", tile.RootLocalX);
            command.Parameters.AddWithValue(@"RootLocalY", tile.RootLocalY);
            command.Parameters.AddWithValue(@"TileType", tile.TileType.ToString());

            // 2. Map the Metadata (JSON BLOB)
            // Convert the C# object back to a UTF-8 byte array (the BLOB format)

            byte[] jsonString = JsonLoader.SerializeJsonBlob<TileRenderProfile>(tile.BaseRender);
            command.Parameters.AddWithValue(@"BaseRender", jsonString);

            object result = command.ExecuteScalar();
            if (result == null || result == DBNull.Value)
            {
                BugHunter.Log(DebugType.TILEREPOSITORY, new InvalidOperationException("Failed to get last insert ID"), DebugLogSeverity.FATAL);
            }
            
            long newId = (long)result;
            return (int)newId;
        }
    }
    private void InsertComponents(int tileId, List<TileComponents> components, SqliteConnection connection, SqliteTransaction transaction)
    { 
        string sql = @"
        INSERT INTO TileComponents (TileID, ComponentTypeName, SerializedData)
        VALUES (@TileID, @TypeName, @Data);";

        using (var command = new SqliteCommand(sql, connection, transaction))
        {
            command.Parameters.Add("@TileID", SqliteType.Integer);
            command.Parameters.Add("@TypeName", SqliteType.Text);
            command.Parameters.Add("@Data", SqliteType.Blob);

            foreach (var component in components)
            {
                if (component.TileComponent == null) continue;

                byte[] componentData = JsonLoader.SerializeJsonBlob(component.TileComponent);

                command.Parameters["@TileID"].Value = tileId;
                command.Parameters["@TypeName"].Value = component.TileComponent.GetType().Name;
                command.Parameters["@Data"].Value = (object)componentData ?? DBNull.Value;

                command.ExecuteNonQuery();
            }
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