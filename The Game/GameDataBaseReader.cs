using Microsoft.Data.Sqlite;
using System;
using System.Data.SqlClient;
using System.Reflection;
using System.Text;
using Newtonsoft.Json;


public class GameDataBaseReader
{
    public static TileObject MapToTileObject(SqliteDataReader reader)
    {
        // Manually map columns to properties; for simplicity, assuming column names match property names
        var tile = new TileObject
        {
            TileId = reader.GetInt32(reader.GetOrdinal("Id")),
            RootGridX = reader.GetInt32(reader.GetOrdinal("RootGridX")),
            RootGridY = reader.GetInt32(reader.GetOrdinal("RootGridY")),
            RootLocalX = reader.GetInt32(reader.GetOrdinal("RootLocalX")),
            RootLocalY = reader.GetInt32(reader.GetOrdinal("RootLocalY")),
            TileType = (TileTypes)Enum.Parse(typeof(TileTypes), reader.GetString(reader.GetOrdinal("TileType")), ignoreCase: true),
            // Add more mappings as needed
        };
        // --- Json Blob Mapping ---
        #region
        const string RenderProfileColumnName = "BaseRender";
        int ordinal = reader.GetOrdinal(RenderProfileColumnName);

        if (!reader.IsDBNull(ordinal))
        {
            // 1. Read the BLOB data as byte array
            byte[] jsonBytes = (byte[])reader[ordinal];

            // 2. Decode the byte array into a string using UTF-8
            string jsonString = Encoding.UTF8.GetString(jsonBytes);

            // 3. Deserialize the JSON string and assign it to the new property
            tile.BaseRender = JsonLoader.DescerializeJsonBlob<TileRenderProfile>(reader, jsonString);
        }
        //--------------------------------------
        //--------------------------------------
        const string TriggerActionsColumnName = "TriggerActions";
        ordinal = reader.GetOrdinal(TriggerActionsColumnName);

        if (!reader.IsDBNull(ordinal))
        {
            // 1. Read the BLOB data as byte array
            byte[] jsonBytes = (byte[])reader[ordinal];

            // 2. Decode the byte array into a string using UTF-8
            string jsonString = Encoding.UTF8.GetString(jsonBytes);

            // 3. Deserialize the JSON string and assign it to the new property
            tile.BaseRender = JsonLoader.DescerializeJsonBlob<TileRenderProfile>(reader, jsonString);
        }
        #endregion
        // ----------------------------

        return tile;
    }
    public static TileComponents MapToComponents(SqliteDataReader reader)
    {
        return new TileComponents
        {
            TileID = reader.GetInt32(reader.GetOrdinal("TileId")),
            TileComponent = (TileComponent)Enum.Parse(typeof(TileComponent), reader.GetString(reader.GetOrdinal("TileComponent")), ignoreCase: true)
        };
    }

    public static TileProperties MapToProperties(SqliteDataReader reader)
    {
        return new TileProperties
        {
            TileID = reader.GetInt32(reader.GetOrdinal("TileId")),
            TileProperty = (TileProperty)Enum.Parse(typeof(TileProperty), reader.GetString(reader.GetOrdinal("TileProperties")), ignoreCase: true)
        };
    }
}