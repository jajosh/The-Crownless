using System;

public interface ITileEngine
{
    string PickADescription(TileObject tile, SeasonData Season, WeatherData Weather, GridBiomeType CurrentBiome, GridBiomeSubType CurrentSubBiome);

    void FinalizeTiles(MapManager map);
    TileObject ProcessTile(char ascii, int gridX, int gridY, int LocalX, int LocalY);
}
