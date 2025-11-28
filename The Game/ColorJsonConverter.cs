using System;
using System.Text.Json;
using System.Text.Json.Serialization;

//public class ColorJsonConverter : JsonConverter<Color>
//{
//    public override Color Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
//    {
//        string hex = reader.GetString()!;
//        return ColorTranslator.FromHtml(hex); // #RRGGBB -> Color
//    }

//    public override void Write(Utf8JsonWriter writer, Color value, JsonSerializerOptions options)
//    {
//        // Convert Color to #RRGGBB
//        writer.WriteStringValue($"#{value.R:X2}{value.G:X2}{value.B:X2}");
//    }
//}wwd