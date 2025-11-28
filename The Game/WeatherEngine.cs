using System;

public interface WeatherEngine
{
    public SeasonData CurrentSeason { get; set; }
    public WeatherData CurrentWeather { get; set; }
    (int, WeatherData) UpdateWeather(int WeeatherCountDown, WeatherData CurrentWeather, SeasonData Season, GridBiomeType BiomeType);

}
