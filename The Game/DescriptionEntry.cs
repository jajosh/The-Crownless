using System;

public class DescriptionEntry
{
    public int Weight { get; set; }
    public string Text { get; set; }

    // Use nullable enums to allow "Any"
    public GridBiomeType? Biome { get; set; } = null; // null = any
    public GridBiomeSubType? SubBiome { get; set; } = null; // null = any
    public SeasonData? Season { get; set; } = null; // null = any
    public WeatherData? Weather { get; set; } = null; // null = any

    public DescriptionEntry() { } // Needed for JSON deserialization

    public DescriptionEntry(int weight, string text, GridBiomeType? biome = null, GridBiomeSubType? subBiome = null,
                            SeasonData? season = null, WeatherData? weather = null)
    {
        Weight = weight;
        Text = text;
        Biome = biome;
        SubBiome = subBiome;
        Season = season;
        Weather = weather;
    }


    /// <summary>
    /// Determines whether the specified season, weather, biome, and sub-biome match the criteria defined by this
    /// instance.
    /// </summary>
    /// <remarks>A parameter is considered a match if the corresponding criterion is not set or if it equals
    /// the provided value. This allows for partial matching when some criteria are unspecified.</remarks>
    /// <param name="currentSeason">The current season to evaluate against the matching criteria.</param>
    /// <param name="weather">The current weather conditions to evaluate against the matching criteria.</param>
    /// <param name="currentBiome">The current biome type to evaluate against the matching criteria.</param>
    /// <param name="currentSubBiome">The current biome sub-type to evaluate against the matching criteria.</param>
    /// <returns>true if all specified parameters match the criteria defined by this instance; otherwise, false.</returns>
    public bool Matches(SeasonData currentSeason, WeatherData weather, GridBiomeType currentBiome, GridBiomeSubType currentSubBiome)
    {
        bool biomeMatch = !Biome.HasValue || Biome.Value == currentBiome;
        bool subBiomeMatch = !SubBiome.HasValue || SubBiome.Value == currentSubBiome;
        bool seasonMatch = !Season.HasValue || Season.Value == currentSeason;
        bool weatherMatch = !Weather.HasValue || Weather.Value == weather;

        return biomeMatch && subBiomeMatch && seasonMatch && weatherMatch;
    }
    /// <summary>
    /// Determines whether the specified season, weather, and biome match the criteria defined by this instance.
    /// </summary>
    /// <remarks>If a criterion (season, weather, or biome) is not specified in this instance, it is
    /// considered a match for any value.</remarks>
    /// <param name="currentSeason">The current season to evaluate against the matching criteria.</param>
    /// <param name="weather">The current weather conditions to evaluate against the matching criteria.</param>
    /// <param name="currentBiome">The current biome to evaluate against the matching criteria.</param>
    /// <returns>true if all specified criteria match the provided season, weather, and biome; otherwise, false.</returns>
    public bool Matches(SeasonData currentSeason, WeatherData weather, GridBiomeType currentBiome)
    {
        bool biomeMatch = !Biome.HasValue || Biome.Value == currentBiome;
        bool seasonMatch = !Season.HasValue || Season.Value == currentSeason;
        bool weatherMatch = !Weather.HasValue || Weather.Value == weather;

        return biomeMatch && seasonMatch && weatherMatch;
    }
    /// <summary>
    /// Determines whether the specified season, weather, and sub-biome match the criteria defined by this instance.
    /// </summary>
    /// <remarks>If a criterion (season, weather, or sub-biome) is not specified in this instance, it is
    /// considered a match for any value of that criterion.</remarks>
    /// <param name="currentSeason">The current season to evaluate against the matching criteria.</param>
    /// <param name="weather">The current weather conditions to evaluate against the matching criteria.</param>
    /// <param name="currentSubBiome">The current sub-biome to evaluate against the matching criteria.</param>
    /// <returns>true if all specified criteria match the provided season, weather, and sub-biome; otherwise, false.</returns>
    public bool Matches(SeasonData currentSeason, WeatherData weather, GridBiomeSubType currentSubBiome)
    {
        bool subBiomeMatch = !SubBiome.HasValue || SubBiome.Value == currentSubBiome;
        bool seasonMatch = !Season.HasValue || Season.Value == currentSeason;
        bool weatherMatch = !Weather.HasValue || Weather.Value == weather;

        return subBiomeMatch && seasonMatch && weatherMatch;
    }
    /// <summary>
    /// Determines whether the specified season and weather match the criteria defined by this instance.
    /// </summary>
    /// <param name="currentSeason">The current season to evaluate against the season criteria.</param>
    /// <param name="weather">The current weather to evaluate against the weather criteria.</param>
    /// <returns>true if both the season and weather match the criteria; otherwise, false.</returns>
    public bool Matches(SeasonData currentSeason, WeatherData weather)
    {
        bool seasonMatch = !Season.HasValue || Season.Value == currentSeason;
        bool weatherMatch = !Weather.HasValue || Weather.Value == weather;

        return seasonMatch && weatherMatch;
    }
    public bool Matches(SeasonData currentSeason)
    {
        bool seasonMatch = !Season.HasValue || Season.Value == currentSeason;

        return seasonMatch;
    }
    public bool Matches(WeatherData weather)
    {
        bool weatherMatch = !Weather.HasValue || Weather.Value == weather;

        return weatherMatch;
    }

}

