using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using The_Game;
namespace The_Game
{
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
    public class ActionObjectConverter : JsonConverter<ActionObject>
    {
        private static readonly Dictionary<string, Type> _typeMap = new()
        {
            ["CorruptingTouch"] = typeof(CorruptingTouch),
            ["HealAction"] = typeof(HealAction),
            ["ApplyStatus"] = typeof(ApplyStatusAction),
            ["Fireball"] = typeof(FireBall),
        };

        public override ActionObject? Read(
            ref Utf8JsonReader reader,
            Type typeToConvert,
            JsonSerializerOptions options)
        {
            using var doc = JsonDocument.ParseValue(ref reader);
            var root = doc.RootElement;

            if (!root.TryGetProperty("actionType", out var typeProp))
                throw new JsonException("Missing 'actionType' discriminator.");

            var typeName = typeProp.GetString()!;

            if (!_typeMap.TryGetValue(typeName, out var targetType))
                throw new JsonException($"Unknown action type: '{typeName}'");

            return (ActionObject?)JsonSerializer.Deserialize(
                root.GetRawText(), targetType, options);
        }

        public override void Write(
            Utf8JsonWriter writer,
            ActionObject value,
            JsonSerializerOptions options)
        {
            JsonSerializer.Serialize(writer, (object)value, options);
        }
    }
}