
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
}
public class ColorJsonConverter : JsonConverter<Color>
{
    public override Color Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        string hex = reader.GetString()!;
        return ColorTranslator.FromHtml(hex); // #RRGGBB -> Color
    }

    public override void Write(Utf8JsonWriter writer, Color value, JsonSerializerOptions options)
    {
        // Convert Color to #RRGGBB
        writer.WriteStringValue($"#{value.R:X2}{value.G:X2}{value.B:X2}");
    }
}