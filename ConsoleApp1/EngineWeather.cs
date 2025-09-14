using System;
public enum WeatherData
{

    Any,
    Clear,
    Overcast,
    Cloudy,
    Snowy,
    Blizzard,
    Rainy,
    HeavyRain,
    Thunderstorm,
    Foggy,

}
public enum SeasonData
{
    Any,
    Spring,
    Summer,
    Fall,
    Winter
}
public class EngineWeather
{
    public WeatherData Weather { get; set; } 
    public SeasonData Seasons { get; set; }
    public int WeatherCount { get; set; }
    public int SeasonCount { get; set; }
    public int Day {  get; set; }
    public EngineWeather()
	{
        Weather = WeatherData.Clear;   // default starting weather
        Seasons = SeasonData.Spring;   // default starting season
        WeatherCount = 1;
        SeasonCount = 1;
        Day = 1;                       // start on day 1
    }
    public static (int, WeatherData) UpdateWeather(int WeatherCountDown, WeatherData currentWeather, SeasonData Seasons)
    {
        Random rng = new Random();

        if (Seasons == SeasonData.Spring)
        {
            List<WeatherData> list = new();
            list.Add(WeatherData.Clear);
            list.Add(WeatherData.Clear);
            list.Add(WeatherData.Rainy);
            list.Add(WeatherData.Rainy);
            list.Add(WeatherData.Rainy);
            list.Add(WeatherData.Rainy);
            list.Add(WeatherData.Rainy);
            list.Add(WeatherData.Rainy);
            list.Add(WeatherData.HeavyRain);
            list.Add(WeatherData.HeavyRain);
            list.Add(WeatherData.Thunderstorm);
            list.Add(WeatherData.Thunderstorm);
            list.Add(WeatherData.Thunderstorm);
            list.Add(WeatherData.Thunderstorm);
            list.Add(WeatherData.Thunderstorm);
            list.Add(WeatherData.Overcast);
            list.Add(WeatherData.Overcast);
            list.Add(WeatherData.Overcast);
            WeatherData NewWeather = list[rng.Next(list.Count)];
            int countDown = rng.Next(5);
            return (countDown, NewWeather);
        }
        else if (Seasons == SeasonData.Summer)
        {
            List<WeatherData> list = new();
            list.Add(WeatherData.Clear);
            list.Add(WeatherData.Clear);
            list.Add(WeatherData.Clear);
            list.Add(WeatherData.Clear);
            list.Add(WeatherData.Clear);
            list.Add(WeatherData.Clear);
            list.Add(WeatherData.Clear);
            list.Add(WeatherData.Clear);
            list.Add(WeatherData.Rainy);
            list.Add(WeatherData.Rainy);
            list.Add(WeatherData.Rainy);
            list.Add(WeatherData.Rainy);
            list.Add(WeatherData.Cloudy);
            list.Add(WeatherData.Cloudy);
            list.Add(WeatherData.Cloudy);
            list.Add(WeatherData.Cloudy);
            list.Add(WeatherData.Cloudy);
            WeatherData NewWeather = list[rng.Next(list.Count)];
            int countDown = rng.Next(5);
            return (countDown, NewWeather);
        }
        else if (Seasons == SeasonData.Fall)
        {
            List<WeatherData> list = new();
            list.Add(WeatherData.Clear);
            list.Add(WeatherData.Clear);
            list.Add(WeatherData.Clear);
            list.Add(WeatherData.Rainy);
            list.Add(WeatherData.Rainy);
            list.Add(WeatherData.Rainy);
            list.Add(WeatherData.Rainy);
            list.Add(WeatherData.Rainy);
            list.Add(WeatherData.Rainy);
            list.Add(WeatherData.Foggy);
            list.Add(WeatherData.Foggy);
            list.Add(WeatherData.Foggy);
            list.Add(WeatherData.Foggy);
            list.Add(WeatherData.Foggy);
            list.Add(WeatherData.Foggy);
            list.Add(WeatherData.Overcast);
            list.Add(WeatherData.Overcast);
            list.Add(WeatherData.Overcast);
            list.Add(WeatherData.Overcast);
            list.Add(WeatherData.Overcast);
            WeatherData NewWeather = list[rng.Next(list.Count)];
            int countDown = rng.Next(5);
            return (countDown, NewWeather);
        }
        else if (Seasons == SeasonData.Winter)
        {
            List<WeatherData> list = new();
            list.Add(WeatherData.Clear);
            list.Add(WeatherData.Clear);
            list.Add(WeatherData.Clear);
            list.Add(WeatherData.Clear);
            list.Add(WeatherData.Snowy);
            list.Add(WeatherData.Snowy);
            list.Add(WeatherData.Snowy);
            list.Add(WeatherData.Snowy);
            list.Add(WeatherData.Snowy);
            list.Add(WeatherData.Snowy);
            list.Add(WeatherData.Snowy);
            list.Add(WeatherData.Overcast);
            list.Add(WeatherData.Overcast);
            list.Add(WeatherData.Overcast);
            list.Add(WeatherData.Overcast);
            list.Add(WeatherData.Overcast);
            list.Add(WeatherData.Overcast);
            list.Add(WeatherData.Overcast);
            list.Add(WeatherData.Overcast);
            list.Add(WeatherData.Blizzard);
            list.Add(WeatherData.Blizzard);
            list.Add(WeatherData.Blizzard);
            list.Add(WeatherData.Blizzard);
            WeatherData NewWeather = list[rng.Next(list.Count)];
            int countDown = rng.Next(5);
            return (countDown, NewWeather);
        }
        return (WeatherCountDown, currentWeather);

    }
}
public struct WeatherRef
{
    //var desc1 = new BiomeRef(GridBiomeType.Forest);
    //var desc2 = new BiomeRef(GridBiomeSubType.DarkForest);
    //var desc3 = BiomeRef.Any;  // matches everything
    public WeatherData? Main { get; }
    public SeasonData? Sub { get; }
    public bool IsAny { get; }   // <--- wildcard flag

    // Constructors
    public WeatherRef(WeatherData type)
    {
        Main = type;
        Sub = null;
        IsAny = false;
    }

    public WeatherRef(SeasonData subtype)
    {
        Main = null;
        Sub = subtype;
        IsAny = false;
    }

    private WeatherRef(bool any)   // special ctor for Any
    {
        Main = null;
        Sub = null;
        IsAny = any;
    }

    public static WeatherRef Any => new WeatherRef(true);

    public override string ToString()
    {
        if (IsAny) return "Any";
        return Main?.ToString() ?? Sub?.ToString() ?? "Unknown";
    }
}

