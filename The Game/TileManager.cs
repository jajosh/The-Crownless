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
        if (engine == null || descriptions == null || descriptions.Count == 0)
            return null;

        DescriptionEntry? selected = null;
        int totalWeight = 0;
        int matchCount = 0;

        foreach (var entry in descriptions)
        {
            if (entry.Matches(engine))
            {
                matchCount++;
                int weight = Math.Max(0, entry.Weight);
                totalWeight += weight;

                // If weights are present, use weighted selection logic
                if (totalWeight > 0)
                {
                    if (Random.Shared.Next(totalWeight) < weight)
                    {
                        selected = entry;
                    }
                }
                // Fallback: Uniform selection if all weights encountered so far are 0
                else
                {
                    if (Random.Shared.Next(matchCount) == 0)
                    {
                        selected = entry;
                    }
                }
            }
        }

        return selected;
    }

    /// <summary>
    /// Async version (if you're doing DB calls or anything)
    /// </summary>
    public static async Task<DescriptionEntry?> GetRandomMatchingDescriptionAsync(
     GameEngine engine,
     List<DescriptionEntry> descriptions,
     CancellationToken ct = default)
    {
        if (engine == null || descriptions == null || descriptions.Count == 0)
            return null;

        DescriptionEntry? selected = null;
        int totalWeight = 0;
        int matchCount = 0;

        foreach (var entry in descriptions)
        {
            // Stop processing if the cancellation token is triggered
            ct.ThrowIfCancellationRequested();

            // Assuming MatchesAsync is the new async version of your check
            if (await entry.MatchesAsync(engine))
            {
                matchCount++;
                int weight = Math.Max(0, entry.Weight);
                totalWeight += weight;

                if (totalWeight > 0)
                {
                    if (Random.Shared.Next(totalWeight) < weight)
                        selected = entry;
                }
                else
                {
                    if (Random.Shared.Next(matchCount) == 0)
                        selected = entry;
                }
            }
        }

        return selected;
    }
    public void FinalizeTiles(MapManager map, List<TileObject> tiles, List<GridObject> grids)
    {

        foreach (var tile in tiles)
        {

            if (tile.DeferredChecks.Contains(TileCheckType.NeighborRoofed))
            {
                bool HasRoofedNeighbor = TileHelpers.HasRoofedNeighbor(map, tile);
                if (HasRoofedNeighbor)
                {
                    tile.Components.Add(
                        new TileComponents
                        {
                            TileID = tile.TileId,
                            ComponentTypeName = "Roofed",
                            TileComponent = new IsRoofedComponent(
                                true)
                        });
                }
            }

            if (tile.DeferredChecks.Contains(TileCheckType.NeighborWalkable))
            {
                bool HasNeighborWalkable = TileHelpers.HasWalkableNeighbor(map, tile);
                if (HasNeighborWalkable)
                {
                    tile.Components.Add(
                        new TileComponents
                        {
                            TileID = tile.TileId,
                            ComponentTypeName = "Roofed",
                            TileComponent = new IsWalkableComponent(
                                true,
                                1)
                        });
                }
            }

            // Clear after processing
            tile.DeferredChecks.Clear();
            // BugHunter.Log(DebugType.LOG, $" | Tile Processed {tile.BaseRender.CharData.MainChar}, {tile.GridX},{tile.GridY},{tile.LocalX},{tile.LocalY}) | ", DebugLogSeverity.telemetry);
        }
        BugHunter.Log(DebugType.GENERICPROCESSING, "Tiles have been processed, starting SQL Transfer...", DebugLogSeverity.telemetry);
        TileRepository repository = new TileRepository();
        repository.SaveAllTilesToDatabase(tiles);
        GridRepository gridRepository = new GridRepository();
        gridRepository.SaveGridToDataBase(grids);
    }
    public TileObject ProcessTile(string ascii, int gridX, int gridY, int LocalX, int LocalY)
    {
        if (TileProcessor._tileHandlers.TryGetValue(ascii, out var handler))
        {
            // Only log if it's a rare/important tile, or if you are in a verbose debug mode
            // Otherwise, your log file will grow by megabytes per second.
            if (ascii == "$")
            {
                BugHunter.Log(DebugType.TILEPROCESSING, $"Processed special tile '{ascii}' at ({gridX},{gridY})", DebugLogSeverity.INFO);
            }
            try
            {
                return handler(gridX, gridY, LocalX, LocalY, ascii);
            }
            catch
            {
                BugHunter.Log(DebugType.ERROR, $" Error processing tile. Tile Handler not found {ascii}", DebugLogSeverity.FATAL);
            }
        }


        // Letters handled dynamically
        if (ascii.Length == 1 && char.IsLetter(ascii[0]))
        {
            // 2. Fall back to reflection method
            string methodName = "On" + ascii;
            Type type = typeof(TileProcessor);
            var method = type.GetMethod(methodName, BindingFlags.Public | BindingFlags.Static);
            if (method != null)
            {
                var processedTile = (TileObject)method.Invoke(null, new object[] { gridX, gridY, LocalX, LocalY, ascii });
                return processedTile;
            }

            // 3. If neither exist, warn and fall through to default
            BugHunter.Log(DebugType.TILEPROCESSING, $"No handler found for letter '{ascii}' (expected {methodName})", DebugLogSeverity.WARN);
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
        };
    }
    public static ICharacter TileStateUpdater(ICharacter character, TileObject tile)
    {
        foreach (TileComponents component in tile.Components)
        {
            switch (component.ComponentTypeName)
            {
                case "IsRoofedComponent":

                    break;
                case "CuttablePlantComponent":
                    break;
                case "HarvestablePlantComponent":
                    break;
                case "TileInventoryComponent":
                    break;
                case "IsFlammableComponent":
                    break;
                case "IsWalkableComponent":
                    break;
                case "CoverComponent":
                    break;
                case "DestructibleComponent":
                    break;
                case "OpenableComonent":
                    break;
                case "ChestComponent":
                    break;
                case "TrapComponent":
                    break;
                case "Respawnable":
                    break;
                case "TiledEffectComponent":
                    break;
            }
        }
        return character;
    }
}
