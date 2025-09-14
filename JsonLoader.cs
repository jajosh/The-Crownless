using System;

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
        string path = FileManager.ItemFilePath;
        if (!File.Exists(path))
        {
            Console.WriteLine($"File not found");
        }
        else if (File.Exists(path))
        {
            Console.WriteLine("Json found!");
        }
        var options = new JsonSerializerOptions
        {
            Converters = { new JsonStringEnumConverter(JsonNamingPolicy.CamelCase) },
            PropertyNameCaseInsensitive = true
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
            ReferenceHandler = ReferenceHandler.Preserve // Handles circular references if any
        };
        options.Converters.Add(new JsonStringEnumConverter());// 👈 this lets enum keys serialize as strings
        string json = JsonSerializer.Serialize(obj, options);
        File.WriteAllText(filePath, json);
    }
}