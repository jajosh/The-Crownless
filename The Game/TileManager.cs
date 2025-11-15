using System;
using System.Reflection;

public class TileManager : ITileEngine
{
    public Random random = new Random();
    public TileManager()
	{
	}
    // Picks a random description based on current conditions
    public string PickADescription(TileObject tile, SeasonData Season, WeatherData Weather, GridBiomeType CurrentBiome, GridBiomeSubType CurrentSubBiome)
    {
        if (tile?.Description == null) return "A wonderful time to walk"; BugHunter.Log($"[Missing Data] - {tile.Root} - Mo Description"); // null guard
        var possible = tile.Description
                .Where(d => d.Matches(Season, Weather, CurrentBiome, CurrentSubBiome) || d.Matches(Season, Weather, CurrentBiome) || d.Matches(Season, Weather, CurrentSubBiome) || d.Matches(Season, Weather) || d.Matches(Season) || d.Matches(Weather))
                .ToList();

        if (possible.Count == 0)
        {
            BugHunter.Log($"[Missing Data] - [{tile.Root}|{Season}|{Weather}|{CurrentBiome}|{CurrentSubBiome}] - No Description");
            return null;
        }
            
        // Weighted random pick
        int totalWeight = possible.Sum(d => d.Weight);
        int roll = random.Next(totalWeight);

        int cumulative = 0;
        foreach (var entry in possible)
        {
            cumulative += entry.Weight;
            if (roll < cumulative)
                return entry.Text;
        }

        return possible.Last().Text; // Fallback if math error

    }
    public void FinalizeTiles(MapManager map)
    {
        foreach (var kvp in MapManager.MapKey)
        {
            TileObject tile = kvp.Value;

            if (tile.DeferredChecks.HasFlag(TileCheckType.NeighborRoofed))
            {
                tile.IsRoofed = TileHelpers.HasRoofedNeighbor(map, tile);
            }

            if (tile.DeferredChecks.HasFlag(TileCheckType.NeighborWalkable))
            {
                tile.IsWalkable = TileHelpers.HasWalkableNeighbor(map, tile);
            }

            // Clear after processing
            tile.DeferredChecks = TileCheckType.None;
        }
    }
    public TileObject ProcessTile(char ascii, int gridX, int gridY, int LocalX, int LocalY)
    {
        // 1. Check dictionary first
        if (TileProcessor._tileHandlers.TryGetValue(ascii, out var handler))
            return handler(gridX, gridY, LocalX, LocalY, ascii); BugHunter.Log(ascii.ToString(), DebugLogSeverity.Info);


        // Letters handled dynamically
        if (char.IsLetter(ascii))
        {
            // 2. Fall back to reflection method
            string methodName = "On" + ascii.ToString();
            Type type = typeof(TileProcessor);
            var method = type.GetMethod(methodName, BindingFlags.Public | BindingFlags.Static);
            if (method != null)
            {
                var processedTile = (TileObject)method.Invoke(null, new object[] { gridX, gridY, LocalX, LocalY, ascii }); 
                return processedTile;
            }

            // 3. If neither exist, warn and fall through to default
            BugHunter.Log($"Warning: No handler found for letter '{ascii}' (expected {methodName})");
        }
        RootComponent root = new RootComponent(gridX, gridY, LocalX, LocalY);
        // Non-letter characters always go to dictionary
        if (TileProcessor._tileHandlers.TryGetValue(ascii, out var fallbackHandler))
            return fallbackHandler(gridX, gridY, LocalX, LocalY, ascii);

        //Default for nothing found: e.g., empty space or unknown
        return new TileObject
        {
            Root = new RootComponent(gridX, gridY, LocalX, LocalY),
            TileType = TileTypes.empty,
            IsWalkable = true,
            IsFlammable = false  // Typo? Should be IsFlammable?
        };
    }
}
