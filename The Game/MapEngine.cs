using MyGame.Controls;
using System;
using System.Linq.Expressions;
using System.Numerics;

public interface MapEngine
{
    string PickADescription(TileObject tile, SeasonData? Season = null, WeatherData? WeatherSeason = null, GridBiomeType? CurrentBiomeSeason = null, GridBiomeSubType? CurrentSubBiomeSeason = null);

    bool PrintWorld(PlayerObject player, ColorTextBox ctb);
    bool PrintWorld(PlayerObject player, ColorTextBox ctb, GridObject grid);
    void Append(TileObject tile, ColorTextBox ctb);
    #region === Map searching === // Tile, grid, and location
    GridBiomeType CurrentBiome();
    GridBiomeSubType CurrentSubBiome();
    SeasonData CurrentSeason();
    #endregion


}
