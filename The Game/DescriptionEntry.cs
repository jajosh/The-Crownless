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
public enum DialogCategory
{
    Ambient,
    Combat,
    Ally,
    Friendly,
    Description,
    Rare
}
public enum DialogSpeaker
{
    NPC,
    Player,
    Narration
}
public record DescriptionEntry
{
    public int ID { get; set; }
    public string TextEntry { get; set; }
    public ObjectDeffinitionType DescriptionType { get; set; } // E.G. Item, Tile, Grid
    public int TypeID { get; set; }
    public int DescriptionWeight { get; set; }
    public int Weight { get; set; }
    public GridBiomeType? Biome { get; set; }
    public GridBiomeSubType? SubBiome { get; set; }
    public SeasonData? Season { get; set; }
    public WeatherData? Weather { get; set; }


    // In your DescriptionEntry class
    public IReadOnlyList<DescriptionEntryFlag>? RequiredFlags { get; set; }
    public IReadOnlyList<DescriptionEntryFlag>? ForbiddenFlags { get; set; }


    // Categorization
    public List<DialogCategory> Categories { get; set; } // E.G Combat, Ally, Ambient
    public DialogSpeaker Speaker { get; set; } // E.G. NPC, player, ally


    public DescriptionEntry(
        string textEntry,                    
        int descriptionWeight = 1,           
        GridBiomeType? biome = null,
        GridBiomeSubType? subBiome = null,
        SeasonData? season = null,
        WeatherData? weather = null,
        IReadOnlyList<DescriptionEntryFlag>? requiredFlags = null,
        IReadOnlyList<DescriptionEntryFlag>? forbiddenFlags = null)
    {
        
        TextEntry = textEntry;
        DescriptionWeight = descriptionWeight;

        Biome = biome;
        SubBiome = subBiome;
        Season = season;
        Weather = weather;
        RequiredFlags = requiredFlags;
        ForbiddenFlags = forbiddenFlags;
    }

    // Bonus: nice static factory that reads like English
    public static DescriptionEntry Any(
        string textEntry,
        int descriptionWeight = 1,
        GridBiomeType? biome = null,
        GridBiomeSubType? subBiome = null,
        SeasonData? season = null,
        WeatherData? weather = null,
        List<DescriptionEntryFlag>? requiredFlags = null,
        List<DescriptionEntryFlag>? forbiddenFlags = null)
        => new DescriptionEntry(
            textEntry,
            descriptionWeight,
            biome,
            subBiome,
            season,
            weather,
            requiredFlags,
            forbiddenFlags);
    public bool Matches(GameEngine engine)
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
            tile = TileRepository.Query(new { GridX = player.Root.GridX, GridY = player.Root.GridY, LocalX = player.Root.LocalX, LocalY = player.Root.LocalY });
        }
        catch (Exception ex)
        {
            string context = $"Failed to query tile at ({player.Root.GridX}, {player.Root.GridY})";
            BugHunter.Log(DebugType.TILEPROCESSING, $"{context} | {ex.Message}", DebugLogSeverity.FATAL);

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
    public async Task<bool> MatchesAsync(GameEngine engine, CancellationToken ct = default)
    {
        if (engine == null) return false;

        // 1. Extract synchronous state (Cheap operations)
        var currentBiome = engine.Map.CurrentBiome();
        var currentSubBiome = engine.Map.CurrentSubBiome();
        var currentSeason = engine.Weather.CurrentSeason;
        var currentWeather = engine.Weather.CurrentWeather;
        var player = engine.Player.PlayerCharacter;

        // 2. Perform Async I/O (The "Heavy" part)
        TileObject? tile = null;
        try
        {
            // Assuming your repository has an Async version of Query
            tile = await TileRepository.QueryAsync(new
            {
                GridX = player.Root.GridX,
                GridY = player.Root.GridY,
                LocalX = player.Root.LocalX,
                LocalY = player.Root.LocalY
            }, ct);
        }
        catch (OperationCanceledException)
        {
            // Handle cancellation gracefully
            return false;
        }
        catch (Exception ex)
        {
            string context = $"Cancellation failed: Must continue - ";
            BugHunter.Log(DebugType.TILEPROCESSING, $"{context} | {ex.Message}", DebugLogSeverity.FATAL);
        }

        // 3. Environment Checks
        bool biomeMatch = !Biome.HasValue || Biome.Value == currentBiome;
        bool subBiomeMatch = !SubBiome.HasValue || SubBiome.Value == currentSubBiome;
        bool seasonMatch = !Season.HasValue || Season.Value == currentSeason;
        bool weatherMatch = !Weather.HasValue || Weather.Value == currentWeather;

        // 4. Flag Checks
        DescriptionFlag currentFlags = DescriptionFlag.None;

        if (tile?.IsBurned() == true)
            currentFlags |= DescriptionFlag.TileBurned;
        if (player.IsWieldingSword() == true)
            currentFlags |= DescriptionFlag.PlayerWieldingSword;
        if (player.IsWearingMetal())
            currentFlags |= DescriptionFlag.PlayerWearingMetal;

        bool requiredFlagsMatch = RequiredFlags == null ||
                                  RequiredFlags.Count == 0 ||
                                  RequiredFlags.All(entry => currentFlags.HasFlag(entry.Flag));

        bool forbiddenFlagsMatch = ForbiddenFlags == null ||
                                   ForbiddenFlags.Count == 0 ||
                                   !ForbiddenFlags.Any(entry => currentFlags.HasFlag(entry.Flag));

        return biomeMatch && subBiomeMatch && seasonMatch && weatherMatch &&
               requiredFlagsMatch && forbiddenFlagsMatch;
    }

}