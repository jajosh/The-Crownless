using Microsoft.VisualBasic.Logging;
using System.Text.Json;
using System;
using System.ComponentModel.DataAnnotations.Schema;
[Flags]
public enum DescriptionFlag : long // use long for >32 flags
{
    None = 0,
    TileBurned = 1 << 0,
    PlayerWieldingSword = 1 << 1,
    PlayerWearingMetal = 1 << 2,
    PlayerInWater = 1 << 3,
    // Add more as needed...
}
public enum ObjectDeffinitionType
{
    Item,
    Tile,
    Grid,
    Location,
    Action,
}
public class DescriptionEntry
{
    public int ID { get; set; }
    public string TextEntry { get; set; }
    public ObjectDeffinitionType DescriptionType { get; set; }
    public int TypeID { get; set; }
    public int DescriptionWeight { get; set; }
    public GridBiomeType? Biome { get; set; }
    public GridBiomeSubType? SubBiome { get; set; }
    public SeasonData? Season { get; set; }
    public WeatherData? Weather { get; set; }


    // In your DescriptionEntry class
    public IReadOnlyList<DescriptionEntryFlag>? RequiredFlags { get; set; }
    public IReadOnlyList<DescriptionEntryFlag>? ForbiddenFlags { get; set; }


    public DescriptionEntry(
        string text,
        int weight = 1,
        GridBiomeType? biome = null,
        GridBiomeSubType? subBiome = null,
        SeasonData? season = null,
        WeatherData? weather = null,
        IReadOnlyList<DescriptionEntryFlag>? requiredFlags = null,
        IReadOnlyList<DescriptionEntryFlag>? forbiddenFlags = null)
    {
        DescriptionWeight = weight;
        TextEntry = text ?? throw new ArgumentNullException(nameof(text));

        Biome = biome;
        SubBiome = subBiome;
        Season = season;
        Weather = weather;
        RequiredFlags = requiredFlags;
        ForbiddenFlags = forbiddenFlags;
    }

    // Bonus: nice static factory that reads like English
    public static DescriptionEntry Any(
        string text,
        int weight = 1,
        GridBiomeType? biome = null,
        GridBiomeSubType? subBiome = null,
        SeasonData? season = null,
        WeatherData? weather = null,
        List<DescriptionEntryFlag>? requiredFlags = null,
        List<DescriptionEntryFlag>? forbiddenFlags = null)
        => new DescriptionEntry(
            text,
            weight,
            biome,
            subBiome,
            season,
            weather,
            requiredFlags,
            forbiddenFlags);

    public bool Matches(GameEngine engine, DescriptionEntryFlag f)
    {
        if (engine == null) return false;

        // Extract current state from GameEngine
        var currentBiome = engine.Map.CurrentBiome();           // e.g.
        var currentSubBiome = engine.Map.CurrentSubBiome();
        var currentSeason = engine.Weather.CurrentSeason;      // or wherever stored
        var currentWeather = engine.Weather.CurrentWeather;

        var player = engine.Player.PlayerCharacter;

        // Pull the current tile
        TileObject? tile = null;
        try
        {
            tile = TileRepository.Query(new { RootGridX = player.Root.GridX, RootGridY = player.Root.GridY, RootLocalX = player.Root.LocalX, RootLocalY = player.Root.LocalY });
        }
        catch (Exception ex)
        {
            BugHunter.LogException(ex);
        }

        // === Biome / Environment Checks ===
        bool biomeMatch = !Biome.HasValue || Biome.Value == currentBiome;
        bool subBiomeMatch = !SubBiome.HasValue || SubBiome.Value == currentSubBiome;
        bool seasonMatch = !Season.HasValue || Season.Value == currentSeason;
        bool weatherMatch = !Weather.HasValue || Weather.Value == currentWeather;

        // === Flag Checks (using bitmask or list) ===
        DescriptionFlag currentFlags = DescriptionFlag.None;

        if (tile?.IsBurned() == true)
            currentFlags |= DescriptionFlag.TileBurned;
        if (player.IsWieldingSword() == true)
            currentFlags |= DescriptionFlag.PlayerWieldingSword;
        if (player.IsWearingMetal())
            currentFlags |= DescriptionFlag.PlayerWearingMetal;
        // Add more as needed...

        // Check required flags: all must be present in currentFlags (true if list is null/empty)
        bool requiredFlagsMatch = RequiredFlags == null ||
                                   RequiredFlags.Count == 0 ||  // Optional: explicit empty check
                                   RequiredFlags.All(entry => currentFlags.HasFlag(entry.Flag));

        // Check forbidden flags: none should be present in currentFlags (true if list is null/empty)
        bool forbiddenFlagsMatch = ForbiddenFlags == null ||
                                   ForbiddenFlags.Count == 0 ||  // Optional: explicit empty check
                                   !ForbiddenFlags.Any(entry => currentFlags.HasFlag(entry.Flag));

        // === Custom Condition (optional) ===
        //bool customMatch = CustomCondition == null ||
        //    CustomCondition(engine); // Now passes full engine!

        return biomeMatch && subBiomeMatch && seasonMatch && weatherMatch &&
               requiredFlagsMatch && forbiddenFlagsMatch /*&& customMatch*/;
    }

}