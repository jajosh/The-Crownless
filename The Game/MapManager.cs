using System.CodeDom;
using System.ComponentModel;
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
    public static LocationObject LocationCache { get; set; } // The active location
    public static GridObject CurrentGridCache { get; set; } // The active Grid
    public MapManager()
    {
        ProcessTiles();

    }
    public static async Task<(LocationObject, GridObject)?> LoadLocationAsync(PlayerObject player, CancellationToken ct)
    {
        try
        {
            // 1. Load the current grid the player is in and add it to the cache first.
            CurrentGridCache = await GridRepository.QueryAsync(new { GridX = player.Root.GridX, GridY = player.Root.GridY }, ct);
        }
        catch (Exception ex)
        {
            BugHunter.Log(DebugType.ERROR, $"Failed to get grid at {player.Root.GridX}.{player.Root.GridY} | Error: {ex.Message}", DebugLogSeverity.WARN);
            return null;
        }

        if (CurrentGridCache != null)
        {
            try
            {
                // 2. Load the Location metadata.
                LocationCache = await LocationRepository.QueryAsync(new { ID = CurrentGridCache.LocationID }, ct);
                
                if (LocationCache != null)
                {
                    // 3. Load ALL grids for this location to ensure the location is "loaded as a whole" in the cache.
                    var allGridsInLocation = await GridRepository.QueryAllAsync(new { LocationID = LocationCache.ID }, ct);
                    LocationCache.LocationMap = allGridsInLocation;
                    
                    BugHunter.Log(DebugType.LOCATIONPROCESSING, $"Loaded location '{LocationCache.Name}' with {LocationCache.LocationMap.Count} grids cached.", DebugLogSeverity.INFO);
                }
            }
            catch (Exception ex)
            {
                BugHunter.Log(DebugType.ERROR, $"Failed to load location data for Grid {CurrentGridCache.GridID} | Error: {ex.Message}", DebugLogSeverity.WARN);
            }
        }

        return (LocationCache, CurrentGridCache);
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
        foreach (TileObject tile in tiles)
        {
            foreach (TileComponents component in tile.Components)
            {
             
                if (component.TileComponent is IsWalkableComponent walkable)
                {
                   if (walkable.IsWalkable == false)
                    {
                        BugHunter.Log(DebugType.TILEPROCESSING, "WARN Tile is not walkable", DebugLogSeverity.DEBUG);
                    }
                }
            }

        }
    }
    public string PickADescription(TileObject tile, SeasonData? Season = null, WeatherData? WeatherSeason = null, GridBiomeType? CurrentBiomeSeason = null, GridBiomeSubType? CurrentSubBiomeSeason = null) // Scalfold Method
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
            // Transiently assign the player as an occupant for rendering if they are on this tile.
            // This does not mutate the persistent database state of the tile.
            if (player.Root.LocalX == tile.LocalX && player.Root.LocalY == tile.LocalY)
            {
                tile.Occupant = player;
            }

            Append(tile, ctb);
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
        foreach (var tile in grid.GridMap)
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
    public void Append(TileObject tile, ColorTextBox ctb)
    {
        // Determine which character data to use: Occupant's or the Tile's base appearance.
        // We use a local reference to avoid mutating the tile's permanent BaseRender profile.
        var activeCharData = tile.Occupant?.Render.CharData ?? tile.BaseRender.CharData;

        string result = activeCharData.MainChar;
        if (activeCharData.ShakeIntensity > 0f)
        {
            result += "[/shake]";
            result = "[shake]" + result;
        }
        if (activeCharData.ShimmerIntensity > 0f)
        {
            result += "[/shimmer]";
            result = $"[shimmer{activeCharData.ShimmerIntensity}{activeCharData.ShimmerColor.A}{activeCharData.ShimmerColor.R}{activeCharData.ShimmerColor.G}{activeCharData.ShimmerColor.B}]" + result;
        }
        if (activeCharData.WaveIntensity > 0f)
        {
            result += "[/wave]";
            result = $"[wave{activeCharData.WaveIntensity}]" + result;
        }
        if (activeCharData.IsFlicker == true)
        {
            result += "[/wave]";
            result = "[wave]" + result;
        }
        ctb.WriteFormattedString(tile.LocalX, (24 - tile.LocalY), result, activeCharData.MainColor.ToColor());

    }

    // Scafold methods for the weather season and biome data
    public SeasonData CurrentSeason()// Scalfold Method
    {

        return SeasonData.Any;
    }
    public GridBiomeSubType CurrentSubBiome()// Scalfold Method
    {

        return GridBiomeSubType.Any;
    }
    public GridBiomeType CurrentBiome()// Scalfold Method
    {

        return GridBiomeType.Any;
    }
    public WeatherData CurrentWeather()// Scalfold Method
    {

        return WeatherData.Any;
    }


}
