using MyGame.Controls;
using System;
using System.Linq.Expressions;
using System.Numerics;

public interface MapEngine
{
    void AddDescription(int weight, string text, GridBiomeType? biome = null, GridBiomeSubType? subBiome = null, SeasonData? season = null, WeatherData? weather = null);
    string PickADescription(TileObject tile, SeasonData? Season = null, WeatherData? WeatherSeason = null, GridBiomeType? CurrentBiomeSeason = null, GridBiomeSubType? CurrentSubBiomeSeason = null);

    void PrintWorld(PlayerObject player, ColorTextBox ctb);
    #region === Map searching === // Tile, grid, and location
    GridBiomeType CurrentBiome();
    GridBiomeSubType CurrentSubBiome();
    SeasonData CurrentSeason();
    #endregion


}
