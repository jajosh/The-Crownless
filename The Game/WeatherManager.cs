using System;

public class WeatherManager : WeatherEngine
{
    public SeasonData CurrentSeason { get; set; }
    public WeatherData CurrentWeather { get; set; }
    public WeatherManager()
    {
        
    }

    public (int, WeatherData) UpdateWeather(int WeatherCountDown, WeatherData CurrentWeather, SeasonData Seasons, GridBiomeType BiomeType)
    {
        Random rng = new Random(); // Consider seeding or injecting for reproducibility

        // Persist weather if countdown remains
        if (WeatherCountDown > 0)
        {
            return (WeatherCountDown - 1, CurrentWeather);
        }

        // Generate new: random countdown + weighted weather
        int countDown = rng.Next(5); // 0-4 ticks

        var weights = GetWeatherWeights(Seasons, BiomeType);
        WeatherData newWeather = WeightedRandom.WeightedRandomPicker(weights, rng);

        return (countDown, newWeather);
    }
    /// <summary>
    /// The weather for the game and the weight for each option
    /// </summary>
    /// <param name="seasons"></param>
    /// <param name="biomeType"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    private static Dictionary<WeatherData, int> GetWeatherWeights(SeasonData seasons, GridBiomeType biomeType)
    {
        return (seasons, biomeType) switch
        {
            (_, GridBiomeType.BorelForest) => seasons switch
            {
                SeasonData.Spring => new() { 
                    { WeatherData.Clear, 2 },
                    { WeatherData.RainLight, 2 },
                    { WeatherData.RainMedium, 6 }, 
                    { WeatherData.RainHeavy, 2 }, 
                    { WeatherData.Thunderstorm, 5 }, 
                    { WeatherData.Overcast, 3 } 
                },
                SeasonData.Summer => new() { 
                    { WeatherData.Clear, 8 }, 
                    { WeatherData.RainMedium, 4 }, 
                    { WeatherData.Cloudy, 5 } 
                },
                SeasonData.Fall => new() { 
                    { WeatherData.Clear, 3 }, 
                    { WeatherData.RainMedium, 6 }, 
                    { WeatherData.Foggy, 6 }, 
                    { WeatherData.Overcast, 5 } 
                },
                SeasonData.Winter => new() { 
                    { WeatherData.Clear, 2 }, 
                    { WeatherData.Snowy, 9 }, 
                    { WeatherData.Overcast, 6 }, 
                    { WeatherData.Blizzard, 3 } 
                },
                _ => throw new ArgumentOutOfRangeException(nameof(seasons))
            },
            (_, GridBiomeType.TemperateBroadleafForest) => seasons switch
            {
                SeasonData.Spring => new() { 
                    { WeatherData.Clear, 3 }, 
                    { WeatherData.RainLight, 5 }, 
                    { WeatherData.RainHeavy, 2 }, 
                    { WeatherData.Thunderstorm, 4 }, 
                    { WeatherData.Overcast, 2 } 
                },
                SeasonData.Summer => new() { 
                    { WeatherData.Clear, 9 },
                    { WeatherData.RainLight, 3 },
                    { WeatherData.Cloudy, 4 } 
                },
                SeasonData.Fall => new() { 
                    { WeatherData.Clear, 4 },
                    { WeatherData.RainLight, 5 },
                    { WeatherData.Foggy, 5 }, 
                    { WeatherData.Overcast, 4 } 
                },
                SeasonData.Winter => new() { 
                    { WeatherData.Clear, 3 }, 
                    { WeatherData.Snowy, 6 }, 
                    { WeatherData.Overcast, 7 }, 
                    { WeatherData.Blizzard, 2 } 
                },
                _ => throw new ArgumentOutOfRangeException(nameof(seasons))
            },
            _ => new() { { WeatherData.Clear, 10 } } // Default/fallback for other biomes
        };
    }
}
