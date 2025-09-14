using System;

public class EngineRandomDialog
{
    public int ID { get; set; }
    public WeatherData? WeatherMain { get; set; }
    public SeasonData? SeasonMain { get; set; }
    public GridBiomeType? BiomeMain { get; set; }
    public GridBiomeSubType? BiomeSub { get; set; }
    public CreatureType? CreatureType { get; set; }
    public bool IsCombat { get; set; }
    public bool IsNonCombat { get; set; }
    public bool IsAllie { get; set; }
    public bool IsFriendly { get; set; }
    public EngineRandomDialog()
    {

    }
}
