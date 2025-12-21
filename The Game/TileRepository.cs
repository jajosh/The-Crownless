using Microsoft.Data.Sqlite;
using System;
using System.Data.SqlClient;
using System.Reflection;
using System.Text;
using Newtonsoft.Json;
using Microsoft.Data.Sqlite;
using System.Linq; // Needed for Select to get IDs

public class TileRepository : GameDataBase
{
    public TileRepository()
    {
    }
    public static TileObject Query(object criteria)
    {
        if (criteria == null) // Null check
        {
            BugHunter.LogException(new ArgumentNullException(nameof(criteria)));
            return null;
        }

        Type criteriaType = criteria.GetType();
        var whereClauses = new List<string>();
        var parameters = new List<SqliteParameter>();

        foreach (PropertyInfo prop in criteriaType.GetProperties())
        {
            object value = prop.GetValue(criteria);
            if (value != null && !IsDefaultValue(value))
            {
                string columnName = prop.Name;
                whereClauses.Add($"{columnName} = @{columnName}");
                parameters.Add(new SqliteParameter($"@{columnName}", value));
            }
        }

        if (whereClauses.Count == 0)
        {
            BugHunter.LogException(new InvalidOperationException("No criteria provided for query"));
        }

        string whereClause = string.Join(" AND ", whereClauses);
        string query = $@"
            SELECT * 
            FROM TileObject 
            LEFT JOIN DescriptionEntry ON TileObject.TileID = DescriptionEntry.TypeID 
            AND DescriptionEntry.DescriptionType = 'Tile'
            LEFT JOIN TileComponents ON TileObject.TileID = TileComponents.TileID
            LEFT JOIN TileProperties ON TileObject.TileID = TileProperties.TileID
            WHERE {whereClause} 
            LIMIT 1";

        using (var connection = MyConnection) // Assuming MyConnection is SqliteConnection
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
    private static bool IsDefaultValue(object value)
    {
        // Check if value is default for its type (e.g., 0 for int, null for string)
        if (value == null) return true;
        Type type = value.GetType();
        if (type.IsValueType)
        {
            return value.Equals(Activator.CreateInstance(type));
        }
        return false;
    }
    public TileObject GetTileFullObject(int tileId, string connectionstring)
    {
        TileObject tile = null;

        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            // -------------------------
            // --- Get the Main Tile ---
            // -------------------------
            string tileSql = "SELECT *, RenderProfiles FROM TileObject WHERE TileID = @TileID";
            using (var command = new SqliteCommand(tileSql, connection))
            {
                command.Parameters.AddWithValue("@Id", tileId);
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
            string compSql = "SELECT * FROM TileComonents WHERE TileId = @tileId";
            using (var command = new SqliteCommand(compSql, connection))
            {
                command.Parameters.AddWithValue("@tileId", tileId);
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
            compSql = "SELECT * FROM TileProperties WHERE TileId = @tileId";
            using (var command = new SqliteCommand(compSql, connection))
            {
                command.Parameters.AddWithValue("@tileId", tileId);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        tile.Properties.Add(GameDataBaseReader.MapToProperties(reader));
                    }
                }
            }
        }
        return tile;
    }

    public void SavetileToDatabase(TileObject tile, string connectionString)
    {
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            // Start a transaction to ensure all three insertions succeed or fail together.
            using (var transaction = connection.BeginTransaction())
            {
                try
                {
                    // A. Insert the Main Tile and retrieve the new ID
                    int newTileId = InsertMainTile(tile, connection, transaction);

                    // B. Insert the Components using the new ID
                    InsertComponents(newTileId, tile.Components, connection, transaction);

                    // C. Insert the Properties using the new ID
                    InsertProperties(newTileId, tile.Properties, connection, transaction);

                    // If all steps succeeded, commit the transaction
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    // If any step fails, roll back the changes
                    transaction.Rollback();
                    BugHunter.LogException(new Exception($"Error saving Tile to Database: {tile.BaseRender.CharData.Char}, {tile.RootGridX},{tile.RootGridY},{tile.RootLocalX},{tile.RootLocalY}"));
                }
            }
        }
    }

    private int InsertMainTile(TileObject tile, SqliteConnection connection, SqliteTransaction transaction)
    {
        string sql = @"
            INSERT INTO TileObject (RootGridX, RootGridY, RootLocalX, RootLocalY, TileType, Cover, IsWalkable, IsRoofed, BurnAmount
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

            long newId = (long)command.ExecuteScalar();
            return (int)newId;
        }
    }
    private void InsertComponents(int tileId, List<TileComponents> components, SqliteConnection connection, SqliteTransaction transaction)
    {
        string sql = @"
        INSERT INTO TileComponents (TileId, ComponentTypeName, SerializedData)
        VALUES (@TileId, @TypeName, @Data);";

        // Use a single command object and reset parameters for efficiency
        using (var command = new SqliteCommand(sql, connection, transaction))
        {
            // Define parameters once
            command.Parameters.Add("@TileId", SqliteType.Integer);
            command.Parameters.Add("@TypeName", SqliteType.Text);
            command.Parameters.Add("@Data", SqliteType.Blob);

            foreach (var component in components)
            {
                // 1. Serialize the complex object into a byte array (BLOB)
                byte[] componentData = JsonLoader.SerializeJsonBlob(component.TileComponent);

                // 2. Set the parameter values
                command.Parameters["@TileId"].Value = tileId;
                command.Parameters["@TypeName"].Value = component.TileComponent.GetType().Name; // e.g., "CuttablePlantComponent"
                command.Parameters["@Data"].Value = componentData;

                command.ExecuteNonQuery();
            }
        } // Command is disposed here
    }
    private void InsertProperties(int tileId, List<TileProperties> properties, SqliteConnection connection, SqliteTransaction transaction)
    {
        string sql = @"
            INSERT INTO TileProperties (TileId, TileProperties)
            values (@TileId, @TileProperties);";
        using (var command = new SqliteCommand(sql, connection, transaction))
        {
            foreach (var property in properties)
            {
                command.Parameters.AddWithValue(@"TileId", tileId);
                command.Parameters.AddWithValue(@"TileProp", property.TileProperty);
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