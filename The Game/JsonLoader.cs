
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using The_Game;

public static class JsonLoader
{
    private static readonly JsonSerializerOptions Options = new()
    {
        Converters = { new JsonStringEnumConverter(JsonNamingPolicy.CamelCase) },
        PropertyNameCaseInsensitive = true,
        WriteIndented = true
    };
    public static T LoadFromJson<T>(string filePath)
    {
        var options = new JsonSerializerOptions
        {

            PropertyNameCaseInsensitive = true,
            Converters = { new JsonStringEnumConverter(JsonNamingPolicy.CamelCase), new ColorJsonConverter() }

        };

        string json = File.ReadAllText(filePath);
        return JsonSerializer.Deserialize<T>(json, options);
    }
    /// <summary>
    /// Saves an object to json
    /// </summary>
    /// <typeparam name="T"> This is a placeholder for the actually object</typeparam>
    /// <param name="filePath"> Path to the save location </param>
    /// <param name="obj"> object to save</param>
    public static void SaveToJson<T>(string filePath, T obj)
    {
        var options = new JsonSerializerOptions
        {
            WriteIndented = true,
            ReferenceHandler = ReferenceHandler.Preserve, // Handles circular references if any
            Converters = { new JsonStringEnumConverter(JsonNamingPolicy.CamelCase), new ColorJsonConverter() }// 👈 this lets enum keys serialize as strings
        };
        
        string json = JsonSerializer.Serialize(obj, options);
        File.WriteAllText(filePath, json);
    }
    public static T DescerializeJsonBlob<T>(SqliteDataReader reader, string columnName)
    {
        // 1. Get the column index
        int ordinal = reader.GetOrdinal(columnName);

        // 2. Read the BLOB data as a raw byte array
        if (reader.IsDBNull(ordinal))
        {
            return default; // Handle null case if column is nullable
        }
        byte[] jsonBytes = (byte[])reader[ordinal];

        // 3. Decode the byte array into a string using UTF-8 encoding
        string jsonString = Encoding.UTF8.GetString(jsonBytes);

        // 4. Deserialize the JSON string int the target object type (T)
        T result = JsonSerializer.Deserialize<T>(jsonString);
        return result;
    }
    public static byte[] SerializeJsonBlob<T>(T data)
    {
        // 1. Handle null input: If the object is null, we return null 
        //    to represent a DB NULL value.
        if (data == null)
        {
            return null;
        }

        // 2. Serialize the target object type (T) into a JSON string
        //    Note: You can pass JsonSerializerOptions here if needed (e.g., camelCase)
        string jsonString = JsonSerializer.Serialize<T>(data);

        // 3. Encode the JSON string into a raw byte array (BLOB) using UTF-8 encoding
        byte[] jsonBytes = Encoding.UTF8.GetBytes(jsonString);

        // 4. Return the byte array, ready for SQLite insertion
        return jsonBytes;
    }
}
