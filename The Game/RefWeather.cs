using System;
public enum WeatherData
{

    Any,
    Clear,
    Overcast,
    Cloudy,
    Snowy,
    Blizzard,
    RainLight,
    RainMedium,
    RainHeavy,
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
public struct RefWeather
{
    //var desc1 = new BiomeRef(GridBiomeType.Forest);
    //var desc2 = new BiomeRef(GridBiomeSubType.DarkForest);
    //var desc3 = BiomeRef.Any;  // matches everything
    public WeatherData? Main { get; }
    public SeasonData? Sub { get; }
    public bool IsAny { get; }   // <--- wildcard flag

    // Constructors
    public RefWeather(WeatherData type)
    {
        Main = type;
        Sub = null;
        IsAny = false;
    }

    public RefWeather(SeasonData subtype)
    {
        Main = null;
        Sub = subtype;
        IsAny = false;
    }

    private RefWeather(bool any)   // special ctor for Any
    {
        Main = null;
        Sub = null;
        IsAny = any;
    }

    public static RefWeather Any => new RefWeather(true);

    public override string ToString()
    {
        if (IsAny) return "Any";
        return Main?.ToString() ?? Sub?.ToString() ?? "Unknown";
    }
}