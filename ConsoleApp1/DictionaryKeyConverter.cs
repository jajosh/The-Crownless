using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

// Custom converters for dictionary keys that are objects

public class DictionaryKeyConverter<TK, TV> : JsonConverter<Dictionary<TK, TV>> where TK : class
{
    public override Dictionary<TK, TV> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var dict = new Dictionary<TK, TV>();
        var jsonDoc = JsonDocument.ParseValue(ref reader);

        foreach (var prop in jsonDoc.RootElement.EnumerateObject())
        {
            TK key;
            if (typeof(TK).IsEnum)
            {
                key = (TK)Enum.Parse(typeof(TK), prop.Name);
            }
            else
            {
                // Deserialize the key from JSON string
                key = JsonSerializer.Deserialize<TK>(prop.Name, options);
            }
            TV value = JsonSerializer.Deserialize<TV>(prop.Value.GetRawText(), options);
            dict[key] = value;
        }

        return dict;
    }

    public override void Write(Utf8JsonWriter writer, Dictionary<TK, TV> value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();
        foreach (var kvp in value)
        {
            string keyJson = JsonSerializer.Serialize(kvp.Key, options);
            writer.WritePropertyName(keyJson.Trim('"')); // remove quotes for valid JSON key
            JsonSerializer.Serialize(writer, kvp.Value, options);
        }
        writer.WriteEndObject();
    }
}