using System;
using System.Collections.Generic;
using System.Text.Json;
using Microsoft.Data.Sqlite;
using Newtonsoft.Json;

public class GameDataBaseReader
{
    public static TileObject MapToTileObject(SqliteDataReader reader)
    {
        // Manually map columns to properties; for simplicity, assuming column names match property names
        var tile = new TileObject
        {
            TileId = reader.GetInt32(reader.GetOrdinal("TileID")),
            RootGridX = reader.GetInt32(reader.GetOrdinal("RootGridX")),
            RootGridY = reader.GetInt32(reader.GetOrdinal("RootGridY")),
            RootLocalX = reader.GetInt32(reader.GetOrdinal("RootLocalX")),
            RootLocalY = reader.GetInt32(reader.GetOrdinal("RootLocalY")),
            TileType = (TileTypes)Enum.Parse(typeof(TileTypes), reader.GetString(reader.GetOrdinal("TileType")), ignoreCase: true),
        };

        // --- Json Blob Mapping ---
        tile.BaseRender = JsonLoader.DescerializeJsonBlob<TileRenderProfile>(reader, "BaseRender") ?? tile.BaseRender;

        // TriggerActions might not exist in the table yet, so we check if the column exists
        int triggerOrdinal = -1;
        try { triggerOrdinal = reader.GetOrdinal("TriggerActions"); } catch { }
        if (triggerOrdinal != -1)
        {
            tile.TriggerActions = JsonLoader.DescerializeJsonBlob<List<TileTriggerActions>>(reader, "TriggerActions") ?? tile.TriggerActions;
        }
        
        return tile;
    }

    public static GridObject MapToGridObject(SqliteDataReader reader)
    {
        // Manually map columns to properties; for simplicity, assuming column names match property names
        var grid = new GridObject
        {
            GridID = reader.GetInt32(reader.GetOrdinal("GridID")),
            GridX = reader.GetInt32(reader.GetOrdinal("GridX")),
            GridY = reader.GetInt32(reader.GetOrdinal("GridY")),
            Biome = Enum.Parse<GridBiomeType>(reader.GetString(reader.GetOrdinal("Biome")), ignoreCase: true),
            SubBiome = Enum.Parse<GridBiomeSubType>(reader.GetString(reader.GetOrdinal("SubBiome")), ignoreCase: true),
            RandomEventChance = reader.GetInt32(reader.GetOrdinal("RandomEventChance"))
        };

        // --- Json Blob Mapping ---
        grid.GridMapKey = JsonLoader.DescerializeJsonBlob<List<string>>(reader, "GridMapKey") ?? grid.GridMapKey;

        return grid;
    }

    public static TileComponents MapToComponents(SqliteDataReader reader)
    {
        string? typeName = reader.IsDBNull(reader.GetOrdinal("ComponentTypeName")) ? null : reader.GetString(reader.GetOrdinal("ComponentTypeName"));
        int tileId = reader.GetInt32(reader.GetOrdinal("TileID"));

        var result = new TileComponents
        {
            TileID = tileId,
            ComponentTypeName = typeName
        };

        if (!string.IsNullOrEmpty(typeName))
        {
            result.TileComponent = typeName switch
            {
                "IsRoofedComponent" => JsonLoader.DescerializeJsonBlob<IsRoofedComponent>(reader, "SerializedData"),
                "CuttablePlantComponent" => JsonLoader.DescerializeJsonBlob<CuttablePlantComponent>(reader, "SerializedData"),
                "HarvestablePlantComponent" => JsonLoader.DescerializeJsonBlob<HarvestablePlantComponent>(reader, "SerializedData"),
                "TileInventoryComponent" => JsonLoader.DescerializeJsonBlob<TileInventoryComponent>(reader, "SerializedData"),
                "IsFlammableComponent" => JsonLoader.DescerializeJsonBlob<IsFlammableComponent>(reader, "SerializedData"),
                "IsWalkableComponent" => JsonLoader.DescerializeJsonBlob<IsWalkableComponent>(reader, "SerializedData"),
                "CoverComponent" => JsonLoader.DescerializeJsonBlob<CoverComponent>(reader, "SerializedData"),
                "DestructibleComponent" => JsonLoader.DescerializeJsonBlob<DestructibleComponent>(reader, "SerializedData"),
                "OpenableComonent" => JsonLoader.DescerializeJsonBlob<OpenableComonent>(reader, "SerializedData"),
                "ChestComponent" => JsonLoader.DescerializeJsonBlob<ChestComponent>(reader, "SerializedData"),
                "TrapComponent" => JsonLoader.DescerializeJsonBlob<TrapComponent>(reader, "SerializedData"),
                "Respawnable" => JsonLoader.DescerializeJsonBlob<Respawnable>(reader, "SerializedData"),
                "TiledEffectComponent" => JsonLoader.DescerializeJsonBlob<TiledEffectComponent>(reader, "SerializedData"),
                _ => null
            };
        }

        return result;
    }

    public static TileProperties MapToProperties(SqliteDataReader reader)
    {
        return new TileProperties
        {
            TileID = reader.GetInt32(reader.GetOrdinal("TileID")),
            TilePropertyName = reader.GetString(reader.GetOrdinal("TilePropertyName")),
            TileProperty = (TileProperty)Enum.Parse(typeof(TileProperty), reader.GetString(reader.GetOrdinal("TileProperties")), ignoreCase: true)
        };
    }

    public static DescriptionEntry MapToDescription(SqliteDataReader reader)
    {
        var entry = new DescriptionEntry(
            textEntry: reader.GetString(reader.GetOrdinal("TextEntry")),
            descriptionWeight: reader.GetInt32(reader.GetOrdinal("DescriptionWeight")),
            biome: reader.IsDBNull(reader.GetOrdinal("Biome")) ? null : Enum.Parse<GridBiomeType>(reader.GetString(reader.GetOrdinal("Biome"))),
            subBiome: reader.IsDBNull(reader.GetOrdinal("SubBiome")) ? null : Enum.Parse<GridBiomeSubType>(reader.GetString(reader.GetOrdinal("SubBiome"))),
            season: reader.IsDBNull(reader.GetOrdinal("Season")) ? null : Enum.Parse<SeasonData>(reader.GetString(reader.GetOrdinal("Season"))),
            weather: reader.IsDBNull(reader.GetOrdinal("Weather")) ? null : Enum.Parse<WeatherData>(reader.GetString(reader.GetOrdinal("Weather")))
        );

        // Map IDs manually
        try
        {
            entry.ID = reader.GetInt32(reader.GetOrdinal("ID"));
            entry.DescriptionType = Enum.Parse<ObjectDeffinitionType>(reader.GetString(reader.GetOrdinal("DescriptionType")));
            entry.TypeID = reader.GetInt32(reader.GetOrdinal("TypeID"));
        }
        catch { }

        return entry;
    }
}