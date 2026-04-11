using System;
using Microsoft.Data.Sqlite;

public class DescriptionRepository : GameDataBase
{
	public DescriptionRepository()
	{
	}
    public static void InsertDescriptions(int typeId, string descriptionType, List<DescriptionEntry> descriptions, SqliteConnection connection, SqliteTransaction transaction)
    {
        // 0. Cleanup old descriptions first to prevent duplication bloat
        DeleteExistingDescriptions(typeId, descriptionType, connection, transaction);

        string sql = @"
            INSERT INTO DescriptionEntry (TypeID, DescriptionType, TextEntry, DescriptionWeight, Biome, SubBiome, Season, Weather)
            VALUES (@TypeID, @DescriptionType, @TextEntry, @DescriptionWeight, @Biome, @SubBiome, @Season, @Weather);";

        using (var command = new SqliteCommand(sql, connection, transaction))
        {
            command.Parameters.Add("@TypeID", SqliteType.Integer);
            command.Parameters.Add("@DescriptionType", SqliteType.Text);
            command.Parameters.Add("@TextEntry", SqliteType.Text);
            command.Parameters.Add("@DescriptionWeight", SqliteType.Integer);
            command.Parameters.Add("@Biome", SqliteType.Text);
            command.Parameters.Add("@SubBiome", SqliteType.Text);
            command.Parameters.Add("@Season", SqliteType.Text);
            command.Parameters.Add("@Weather", SqliteType.Text);

            foreach (var entry in descriptions)
            {
                command.Parameters["@TypeID"].Value = typeId;
                command.Parameters["@DescriptionType"].Value = descriptionType;
                command.Parameters["@TextEntry"].Value = (object)entry.TextEntry ?? "Empty";
                command.Parameters["@DescriptionWeight"].Value = entry.DescriptionWeight;
                command.Parameters["@Biome"].Value = (object)entry.Biome?.ToString() ?? "Any";
                command.Parameters["@SubBiome"].Value = (object)entry.SubBiome?.ToString() ?? "Any";
                command.Parameters["@Season"].Value = (object)entry.Season?.ToString() ?? "Any";
                command.Parameters["@Weather"].Value = (object)entry.Weather?.ToString() ?? "Any";

                command.ExecuteNonQuery();
            }
        }
    }

    public static void DeleteExistingDescriptions(int typeId, string descriptionType, SqliteConnection connection, SqliteTransaction transaction)
    {
        string sql = "DELETE FROM DescriptionEntry WHERE TypeID = @TypeID AND DescriptionType = @DescriptionType;";
        using var command = new SqliteCommand(sql, connection, transaction);
        command.Parameters.AddWithValue("@TypeID", typeId);
        command.Parameters.AddWithValue("@DescriptionType", descriptionType);
        command.ExecuteNonQuery();
    }
}
