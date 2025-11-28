using MyGame.Controls;
using System;
using System.Linq.Expressions;
using System.Numerics;

public interface MapEngine
{
    Dictionary<(int, int, (int, int)), TileObject> MapKey { get; }
    Dictionary<(int, int), GridObject> GridKey { get; }
    Dictionary<int, LocationObject> LocationKey { get; }
    void AddDescription(int weight, string text, GridBiomeType? biome = null, GridBiomeSubType? subBiome = null, SeasonData? season = null, WeatherData? weather = null);
    string PickADescription(TileObject tile, SeasonData? Season = null, WeatherData? WeatherSeason = null, GridBiomeType? CurrentBiomeSeason = null, GridBiomeSubType? CurrentSubBiomeSeason = null);

    void PrintWorld(PlayerObject player, ColorTextBox ctb);
    #region === Map searching === // Tile, grid, and location
    static abstract IQueryable<LocationObject> QueryLocation(Expression<Func<LocationObject, bool>> predicate);
    static abstract IQueryable<GridObject> QueryGrid(Expression<Func<GridObject, bool>> predicate);
    static abstract IQueryable<TileObject> QueryTile(Expression<Func<TileObject, bool>> predicate);
    GridBiomeType CurrentBiome();
    GridBiomeSubType CurrentSubBiome();
    SeasonData CurrentSeason();
    #endregion


}
