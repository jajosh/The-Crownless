using System;
public enum WeatherData
{

    Any = 1,
    Clear = 2,
    Overcast =3,
    Cloudy=4,
    Snowy=5,
    Blizzard=6,
    RainLight=7,
    RainMedium=8,
    RainHeavy=9,
    Thunderstorm=10,
    Foggy=11,

}
public enum SeasonData
{
    Any = 1,
    Spring = 2,
    Summer = 3,
    Fall= 4,
    Winter = 5
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