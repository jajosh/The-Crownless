using System.CodeDom;
using System.Linq.Expressions;
using System.Numerics;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic.Logging;
using MyGame.Controls;
using SQLitePCL;
using The_Game;
using static System.Windows.Forms.Design.AxImporter;

public class MapManager : MapEngine
{

    public MapManager()
    {
        ProcessTiles();

    }

    public void ProcessTiles()
    {
        TileRepository tileDB = new TileRepository();
        // Initialize lists locally
        List<TileObject> tiles = new List<TileObject>();
        List<GridObject> grids = new List<GridObject>();

        BugHunter.Log(DebugType.TILEPROCESSING, "Check point: Tile Processing process has started.", DebugLogSeverity.INFO);
        TileManager tiler = new TileManager();
        // Use the generic loader

        List<LocationObject> Locations = JsonLoader.LoadFromJson<List<LocationObject>>(FilePaths.LocationsFilePath);
        BugHunter.Log(DebugType.LOCATIONPROCESSING, $"Total Locations preloaded == {Locations.Count}");

        foreach (var Location in Locations)
        {
            BugHunter.Log(DebugType.LOCATIONPROCESSING, $"Loading new location. Location ID == {Location.ID}"); // !!! Debug !!!
            foreach (var grid in Location.LocationMap)
            {
                // Set LocationID FIRST before any early exits
                grid.LocationID = Location.ID;
                
                if (grid.GridMapKey == null) continue;
                
                grids.Add(grid);
                int height = grid.GridMapKey.Count;

                for (int row = 0; row < height; row++)  // top to bottom
                {
                    string line = grid.GridMapKey[row];
                    int localX = 0;
                    for (int col = 0; col < line.Length; col++)
                    {
                        string character;
                        if (char.IsHighSurrogate(line[col]) && col + 1 < line.Length && char.IsLowSurrogate(line[col + 1]))
                        {
                            character = line.Substring(col, 2);
                            col++; // Skip the next char as it's part of the surrogate pair
                        }
                        else
                        {
                            character = line[col].ToString();
                        }

                        int localY = height - 1 - row;
                        
                        TileObject tile = tiler.ProcessTile(character, grid.GridX, grid.GridY, localX, localY);
                        tiles.Add(tile);
                        // BugHunter.Log(DebugType.TILEPROCESSING, $"Tile Processed - {character}", DebugLogSeverity.telemetry);
                        localX++;
                    }
                }
            }

        }
        tiler.FinalizeTiles(this, tiles, grids);
        BugHunter.Log(DebugType.TILEPROCESSING, "Tiles have been Finalized", DebugLogSeverity.INFO);
    }
    public string PickADescription(TileObject tile, SeasonData? Season = null, WeatherData? WeatherSeason = null, GridBiomeType? CurrentBiomeSeason = null, GridBiomeSubType? CurrentSubBiomeSeason = null)
    {
        return string.Empty;
    }

    public bool PrintWorld(PlayerObject player, ColorTextBox ctb)
    {
        BugHunter.Log(DebugType.MAPRENDERING, "Starting MapRendering.");
        ctb.SuspendLayout();
        GridObject? currentGrid = GridRepository.Query(new { GridX = player.Root.GridX, GridY = player.Root.GridY });

        if (currentGrid == null)
        {
            BugHunter.Log(DebugType.TILEPROCESSING, $"Could not find grid at ({player.Root.GridX}, {player.Root.GridY}). Map rendering aborted.", DebugLogSeverity.ERROR);
            ctb.ResumeLayout();
            return false;
        }
        List<TileObject> CurrentTileList = TileRepository.QueryGridTiles(currentGrid.GridX, currentGrid.GridY);



        BugHunter.Log(DebugType.MAPRENDERING, $"Starting Rendering loop. CurrentTileList length = {CurrentTileList.Count()}");
        foreach (var tile in CurrentTileList)
        {
            // Check if there is a player or NPC over this
            if (tile.Occupant != null || (player.Root.LocalX == tile.RootLocalX && player.Root.LocalY == tile.RootLocalY))
            {
                tile.Occupant = player;
                tile.BaseRender = tile.Occupant.Render;
                BugHunter.Log(DebugType.MAPRENDERING, $" ICharacter Tile detected. Unicode = {tile.BaseRender.CharData.MainChar}");
                Append(tile, ctb);
            }
            else if (tile.Occupant == null)
            {
                Append(tile, ctb);
            }

        }

        ctb.ResumeLayout();
        return true;
    }
    public bool PrintWorld(PlayerObject player, ColorTextBox ctb, GridObject grid)
    {
        if (grid == null)
        {
            BugHunter.Log(DebugType.TILEPROCESSING, $"Could not find grid at ({player.Root.GridX}, {player.Root.GridY}). Map rendering aborted.", DebugLogSeverity.ERROR);
            ctb.ResumeLayout();
            return false;
        }
        foreach (var tile in grid.RenderCell)
        {
            Append(tile, ctb);
        }
        return true;
    }
    /// <summary>
    /// Parses the final char so that it takes into account the different tags allowed by the colortextbox 
    /// </summary>
    /// <param name="tile"></param>
    /// <param name="ctb"></param>
    /// <param name="x"></param>
    /// <param name="y"></param>
    public void Append(TileObject tile, ColorTextBox ctb)
    {
        string result = tile.BaseRender.CharData.MainChar;

        if (tile.BaseRender.CharData.ShakeIntensity > 0f)
        {
            result += "[/shake]";
            result = "[shake]" + result;
        }
        if (tile.BaseRender.CharData.ShimmerIntensity > 0f)
        {
            result += "[/shimmer]";
            result = $"[shimmer{tile.BaseRender.CharData.ShimmerIntensity}{tile.BaseRender.CharData.ShimmerColor.A}{tile.BaseRender.CharData.ShimmerColor.R}{tile.BaseRender.CharData.ShimmerColor.G}{tile.BaseRender.CharData.ShimmerColor.B}]" + result;
        }
        if (tile.BaseRender.CharData.WaveIntensity > 0f)
        {
            result += "[/wave]";
            result = $"[wave{tile.BaseRender.CharData.WaveIntensity}]" + result;
        }
        if (tile.BaseRender.CharData.IsFlicker == true)
        {
            result += "[/wave]";
            result = "[wave]" + result;
        }
        ctb.WriteFormattedString(tile.RootLocalX, (24 - tile.RootLocalY), result, tile.BaseRender.CharData.MainColor.ToColor());

    }

    // Scafold methods for the weather season and biome data
    public SeasonData CurrentSeason()
    {

        return SeasonData.Any;
    }
    public GridBiomeSubType CurrentSubBiome()
    {

        return GridBiomeSubType.Any;
    }
    public GridBiomeType CurrentBiome()
    {

        return GridBiomeType.Any;
    }
    public WeatherData CurrentWeather()
    {

        return WeatherData.Any;
    }
    

}
