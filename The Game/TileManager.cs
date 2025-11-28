using System;
using System.Reflection;

public class TileManager : ITileEngine
{
    public Random random = new Random();
    public TileManager()
	{
	}
    // Picks a random description based on current conditions
    /// <summary>
    /// Finds a random DescriptionEntry that matches current game conditions, weighted by Weight
    /// </summary>
    public static DescriptionEntry? GetRandomMatchingDescription(GameEngine engine, List<DescriptionEntry> descriptions)
    {
        if (engine == null || descriptions == null || !descriptions.Any())
            return null;

        // Step 1: Filter matching entries
        var matching = descriptions
            .Where(entry => entry.Matches(engine, f))
            .ToList();

        if (!matching.Any())
            return null;

        // Step 2: Weighted random selection
        int totalWeight = matching.Sum(entry => entry.Weight);
        if (totalWeight == 0)
            return matching[Random.Shared.Next(matching.Count)]; // fallback uniform

        int randomWeight = Random.Shared.Next(totalWeight);
        int currentWeight = 0;

        foreach (var entry in matching)
        {
            currentWeight += entry.Weight;
            if (randomWeight < currentWeight)
                return entry;
        }

        return matching.Last(); // fallback
    }

    /// <summary>
    /// Async version (if you're doing DB calls or anything)
    /// </summary>
    public static async Task<DescriptionEntry?> GetRandomMatchingDescriptionAsync(
        GameEngine engine,
        List<DescriptionEntry> descriptions,
        CancellationToken ct = default)
    {
        if (engine == null || descriptions == null || !descriptions.Any())
            return null;

        // Filter matching (async if needed)
        var matching = descriptions
            .Where(entry => entry.Matches(engine, f))
            .ToList();

        if (!matching.Any())
            return null;

        int totalWeight = matching.Sum(entry => entry.Weight);
        if (totalWeight == 0)
            return matching[Random.Shared.Next(matching.Count)];

        int randomWeight = Random.Shared.Next(totalWeight);
        int currentWeight = 0;

        foreach (var entry in matching)
        {
            currentWeight += entry.Weight;
            if (randomWeight < currentWeight)
                return entry;
        }

        return matching.Last();
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
