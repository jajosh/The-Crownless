using Microsoft.VisualBasic.Logging;
using MyGame.Controls;
using System.CodeDom;
using System.Linq.Expressions;
using System.Text.Json;
using System.Text.Json.Serialization;
using static System.Windows.Forms.Design.AxImporter;

public class MapManager : MapEngine
{
    // Explicit interface property implementations for MapEngine
    Dictionary<(int, int, (int, int)), TileObject> MapEngine.MapKey => MapKey;
    Dictionary<(int, int), GridObject> MapEngine.GridKey => GridKey;
    Dictionary<int, LocationObject> MapEngine.LocationKey => LocationKey;

    public static Dictionary<(int, int, (int, int)), TileObject> MapKey = new Dictionary<(int, int, (int, int)), TileObject>();
    public static Dictionary<(int, int), GridObject> GridKey = new Dictionary<(int, int), GridObject>();
    public static Dictionary<int, LocationObject> LocationKey = new Dictionary<int, LocationObject>();

    public MapManager()
    {
        BugHunter.Log("Check point: Tile Processing process has started.");
        TileManager tiler = new TileManager();
        // Use the generic loader

        List<LocationObject> Locations = JsonLoader.LoadFromJson<List<LocationObject>>(FilePaths.GridFilePath);
        BugHunter.Log(Locations.Count.ToString());


        foreach (var Location in Locations)
        {

            foreach (var grid in Location.LocationMap)
            {
                GridKey[(grid.GridX, grid.GridY)] = grid;
                if (grid.GridMapKey == null) continue;

                int height = grid.GridMapKey.Count;

                for (int row = 0; row < height; row++)  // top to bottom
                {
                    //BugHunter.Log(height.ToString(), DebugLogSeverity.Debug);
                    string line = grid.GridMapKey[row];
                    for (int col = 0; col < line.Length; col++)
                    {
                        //BugHunter.Log(line.Length.ToString(), DebugLogSeverity.Debug);

                        char c = line[col];

                        // Flip Y so row 0 (top) becomes topmost visually
                        int localY = height - 1 - row;
                        int localX = col;

                        TileObject tile = tiler.ProcessTile(c, grid.GridX, grid.GridY, localX, localY);
                        MapKey.Add((grid.GridX, grid.GridY, (localX, localY)), tile);
                    }
                }
            }
            
        }

       tiler.FinalizeTiles(this);
       BugHunter.Log("Tiles have been Finalized");
    }
    #region === Query functions === 
    /// <summary>
    /// Queries LocationObjects by applying the predicate to each object.
    /// Returns an IQueryable for deferred execution and further chaining.
    /// </summary>
    public static IQueryable<LocationObject> Query(Expression<Func<LocationObject, bool>> predicate)
    {
        return LocationKey.Values.AsQueryable().Where(predicate);
    }

    /// <summary>
    /// Queries GridObjects by applying the predicate to each object.
    /// Returns an IQueryable for deferred execution and further chaining.
    /// Note: Since GridObjects lack IDs, results are based solely on object properties.
    /// </summary>
    public static IQueryable<GridObject> Query(Expression<Func<GridObject, bool>> predicate)
    {
        return GridKey.Values.AsQueryable().Where(predicate);
    }

    /// <summary>
    /// Queries TileObjects by applying the predicate to each object.
    /// Returns an IQueryable for deferred execution and further chaining.
    /// Note: Since TileObjects lack IDs, results are based solely on object properties.
    /// </summary>
    public static IQueryable<TileObject> Query(Expression<Func<TileObject, bool>> predicate)
    {
        return MapKey.Values.AsQueryable().Where(predicate);
    }

    #endregion
    public void AddDescription(int weight, string text, GridBiomeType? biome = null, GridBiomeSubType? subBiome = null, SeasonData? season = null, WeatherData? weather = null)
    {

    }
    public string PickADescription(TileObject tile, SeasonData? Season = null, WeatherData? WeatherSeason = null, GridBiomeType? CurrentBiomeSeason = null, GridBiomeSubType? CurrentSubBiomeSeason = null)
    {
        return string.Empty;
    }

    public void PrintWorld(PlayerObject player, ColorTextBox ctb)
    {
        //ctb.Clear();  // Clear previous content
        ctb.SuspendLayout();  // Optional: Improve performance for large updates

        int startPos = 0;

        int height = MapKey.Where(k => k.Key.Item1 == player.Root.GridX && k.Key.Item2 == player.Root.GridY)
                   .Max(k => k.Key.Item3.Item2) + 1;
        int width = MapKey.Where(k => k.Key.Item1 == player.Root.GridX && k.Key.Item2 == player.Root.GridY)
                          .Max(k => k.Key.Item3.Item1) + 1;


        for (int localY = height - 1; localY >= 0; localY--)
        {
            for (int localX = 0; localX < width; localX++)
            {
                var key = (player.Root.GridX, player.Root.GridY, (localX, localY));
                char displayChar = MapKey.TryGetValue(key, out var tile) ? tile.CharData.Char : ' ';

                if (localX == player.Root.LocalX && localY == player.Root.LocalY)
                    displayChar = '@';

                ctb.Append(tile.CharData);

                startPos++;
            }
            //ctb.AppendText(Environment.NewLine);
            startPos += Environment.NewLine.Length;
        }

        ctb.ResumeLayout();  // Optional: Resume after update
                             // row++;  // If you still need this for external tracking, keep it; otherwise remove
    }

}
