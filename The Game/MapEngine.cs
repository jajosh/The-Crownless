using MyGame.Controls;
using System;
using System.Linq.Expressions;
using System.Numerics;

namespace The_Game
{
    public interface MapEngine
    {
        public LocationObject LocationCache { get; set; }
        public GridObject CurrentGridCache { get; set; }
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
}