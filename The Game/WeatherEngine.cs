using System;

public interface WeatherEngine
{
    (int, WeatherData) UpdateWeather(int WeeatherCountDown, WeatherData CurrentWeather, SeasonData Season, GridBiomeType BiomeType);

}
