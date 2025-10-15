using GameNamespace;
using System.Diagnostics.Metrics;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

/// <summary>
/// Summary description for Class1
/// </summary>
public class Map
{
    public static Dictionary<(int, int, (int, int)), Tile> MapKey = new Dictionary<(int, int, (int, int)), Tile>();
    public static Dictionary<(int, int), Grid>GridKey = new Dictionary<(int, int), Grid>();
    private const int GridY = 25;
    private const int GridX = 51;
    public static Dictionary<char, Func<int, int, int, int, char, Tile>>? _tileHandlers;



    public Map()
    {
        _tileHandlers = new Dictionary<char, Func<int, int, int, int, char, Tile>>
        {
            #region === Spawn Points, Characters, and Triggers ===
            { '@', TileProcessor.OnPlayerSpawn },
            { '&', TileProcessor.OnNPCSpawn },
            { '!', TileProcessor.OnQuestMarker },
            { '?', TileProcessor.OnQuestionMark },
            { '♘', TileProcessor.OnKnightSpawn },
            { '♖', TileProcessor.OnGuard },
            { '€', TileProcessor.OnRandomEventSpawn },
            { 'ɤ', TileProcessor.OnPlantSpawn },
            { '♥', TileProcessor.OnHeartSpawn },
            { '♡', TileProcessor.OnHalfHeartSpawn },
            { '♬', TileProcessor.OnSoundIndicator },
            { '☠', TileProcessor.OnPhysicalTrap },
            { '☢', TileProcessor.OnMagicTrap },
            { '☣', TileProcessor.OnLogicTrap },
            { 'ÿ', TileProcessor.OnMarketStand },
            #endregion

            #region === Shops ===
            { '⚔', TileProcessor.OnWeaponsShop },
            { '⚒', TileProcessor.OnCraftingShop },
            { '⛏', TileProcessor.OnRangerShop },
            { '⛓', TileProcessor.OnMatieralShop },
            #endregion

            #region === Faction Related ===
            { '♦', TileProcessor.OnFactionQuest },
            #endregion

            #region === Terrain & Walkable Areas ===
            { '.', TileProcessor.OnPath },
            { ',', TileProcessor.OnGrass },
            { '\'', TileProcessor.OnInsideWalkablePath },
            { '…', TileProcessor.OnRoad },
            { '♣', TileProcessor.OnTree },
            { '♤', TileProcessor.OnDeadTree },
            #endregion
            
            #region === Accent Pieces ===
            { '◎', TileProcessor.OnFairyRing },
            { '◉', TileProcessor.OnBullseye },
            { '●', TileProcessor.OnSmallStone1 },
            { '○', TileProcessor.OnSmallStone2 },
            { '▫', TileProcessor.OnGroundDetail },
            { '▪', TileProcessor.OnSmallStone3 },
            #endregion

            #region === Terrain & Non-Walkable Areas ===
            { '▒', TileProcessor.OnImpassableTerrain },
            { 'f', TileProcessor.OnThickForest },
            { 'F', TileProcessor.OnDenseForest },
            { '■', TileProcessor.OnLargeStone },
            { '□', TileProcessor.OnObstaclesgrid },
            { '%', TileProcessor.OnBush },
            #endregion

            #region === Structures & Furnishings ===
            { '˄', TileProcessor.OnTroughUpper },
            { '˅', TileProcessor.OnTroughLower },
            { '<', TileProcessor.OnFurnaceLeft },
            { '>', TileProcessor.OnFurnaceRight },
            { '✎', TileProcessor.OnWritingDesk },
            { '✏', TileProcessor.OnNote },
            { 'π', TileProcessor.OnCounter },
            { '▭', TileProcessor.OnBed },
            { '☩', TileProcessor.OnCross1 },
            { '†', TileProcessor.OnCross2 },
            { '‡', TileProcessor.OnReligiousSymbol },
            { 'ɦ', TileProcessor.OnPew },
            { '⛬', TileProcessor.OnAlter},
            { '▣', TileProcessor.OnChest },
            { '⬜', TileProcessor.OnBarrel },
            { 'Ṡ', TileProcessor.OnSecretChest },
            { '█', TileProcessor.OnContainer },
            { 'Ǒ', TileProcessor.OnWoodenPillar },

            #endregion
            #region === Walls ===
            // ┌, ┐, └, ┘, ─, │, ┼, ┬, ┴, ├, ┤  -> OnWallCharacter
            { '┌', TileProcessor.OnWallCharacter },
            { '┐', TileProcessor.OnWallCharacter },
            { '└', TileProcessor.OnWallCharacter },
            { '┘', TileProcessor.OnWallCharacter },
            { '─', TileProcessor.OnWallCharacter },
            { '│', TileProcessor.OnWallCharacter },
            { '┼', TileProcessor.OnWallCharacter },
            { '┬', TileProcessor.OnWallCharacter },
            { '┴', TileProcessor.OnWallCharacter },
            { '├', TileProcessor.OnWallCharacter },
            { '┤', TileProcessor.OnWallCharacter },

            // #, +, -, |, /, \, _  -> OnSpecialBuilders
            { '#', TileProcessor.OnSpecialBuilders },
            { '+', TileProcessor.OnSpecialBuilders },
            { '-', TileProcessor.OnSpecialBuilders },
            { '|', TileProcessor.OnSpecialBuilders },
            { '/', TileProcessor.OnSpecialBuilders },
            { '\\', TileProcessor.OnSpecialBuilders }, // escape needed for backslash
            { '_', TileProcessor.OnSpecialBuilders },
            #endregion 

            #region === Doors & Entrances ===
            { '=', TileProcessor.OnDoor },
            { '▶', TileProcessor.OnRightEntrance },
            { '◀', TileProcessor.OnLeftEntrance },
            #endregion

            #region === UI Decorations & Symbols ===
            { '★', TileProcessor.OnSolidStar },
            { '☆', TileProcessor.OnEmptyStar },
            { '✓', TileProcessor.OnCheckMark1 },
            { '✗', TileProcessor.OnCrossPatch1 },
            { '✘', TileProcessor.OnCrossPatch2 },
            { '✔', TileProcessor.OnCheckMark2 },
            { '↑', TileProcessor.OnUpArrow1 },
            { '↓', TileProcessor.OnDownArrow1 },
            { '←', TileProcessor.OnLeftArrow1 },
            { '→', TileProcessor.OnRightArrow1 },
            { '↔', TileProcessor.OnSideSideArrow },
            { '↕', TileProcessor.OnUpDownArrow },
            { '⇦', TileProcessor.OnLeftArrow2 },
            { '⇩', TileProcessor.OnDownArrow2 },
            { '⇨', TileProcessor.OnRightArrow2 },
            { '⇧', TileProcessor.OnUpArrow2 },
            { '^', TileProcessor.OnUpArrow3 },
            { '☻', TileProcessor.OnCharacter },
            { '☺', TileProcessor.OnSmileFace },
            { '⚙', TileProcessor.OnMachinery },
            { '‼', TileProcessor.OnAlert },
            { '♫', TileProcessor.OnMusicalInstruments }
            #endregion
        };
    }
    // Updated LoadMapFromJson (always add tile)
    public static Map LoadMapFromJson()
    {
        Map map = new Map();

        // Use the generic loader
        List<Locations> Locations = JsonLoader.LoadFromJson<List<Locations>>(FileManager.TheGridFilePath)
                          ?? new List<Locations>();

        foreach (var Location in Locations)
        {
            foreach (var grid in Location.Map)
            {
                GridKey[(grid.GridX, grid.GridY)] = grid;
                if (grid.GridMap == null) continue;

                for (int LocalY = 0; LocalY < grid.GridMap.Count; LocalY++)  // Vertical (Y/rows, 0=top)
                {
                    string line = grid.GridMap[LocalY];
                    for (int LocalX = 0; LocalX < line.Length; LocalX++)  // Horizontal (X/columns)
                    {
                        char c = line[LocalX];
                        Tile tile = map.ProcessTile(c, grid.GridX, grid.GridY, LocalX, LocalY);  // Non-null
                        MapKey[(grid.GridX, grid.GridY, (LocalX, LocalY))] = tile;
                    }
                }
            }
        }

        map.FinalizeTiles();
        return map;
    }
    public static Grid? FindGrid(Map map, int x, int y)// Does the map really need to be tied into this??
    {

        if (GridKey.TryGetValue((x, y), out Grid grid))
        {
            return grid;
        }
        return null; // or throw an exception if you prefer
    }
    public static Dictionary<(int, int), Tile> LocalTiles(Grid currentGrid)
    {
        Dictionary<(int, int), Tile> result = new();
        for (int x = 0; x < GridX; x++)
        {
            for (int y = 0; y < GridY; y++)
            {
                if (MapKey.TryGetValue((currentGrid.GridX, currentGrid.GridY, (x, y)), out Tile tile))
                {
                    result.Add((x, y), tile);
                }
            }
        }
        return result;
    }
    
    // Updated PrintWorld (minor: use max width, clear with border offset)
    public static void PrintWorld(Map map, Player player, int reservedLines)
    {
        if (MapKey == null || MapKey.Count == 0)
        {
            Console.WriteLine("Map is empty! Nothing to print.");
            return;
        }

        if (player?.Root == null)
        {
            Console.WriteLine("Player or root position not initialized.");
            return;
        }

        // Fetch current grid dynamically
        if (!GridKey.TryGetValue((player.Root.GridX, player.Root.GridY), out var currentGrid) || currentGrid.GridMap == null)
        {
            Console.WriteLine("Current grid not found or empty.");
            return;
        }

        int height = currentGrid.GridMap.Count;  // Vertical rows (Y) = 25
        int width = 0;
        foreach (var rowStr in currentGrid.GridMap)
        {
            width = Math.Max(width, rowStr?.Length ?? 0);  // Horizontal columns (X) = 51
        }
        if (width == 0)
        {
            Console.WriteLine("Grid has no columns.");
            return;
        }

        // Calculate safe printable rows (assume fits 25)
        int availableVisibleRows = Console.WindowHeight - reservedLines - 1;
        int availableBufferRows = Console.BufferHeight - 1;
        int printableRows = Math.Min(height, Math.Min(availableVisibleRows, availableBufferRows));

        if (printableRows <= 0) return;

        // Optional: Extend buffer
        if (printableRows + 1 > Console.BufferHeight)
        {
            Console.BufferHeight = printableRows + reservedLines + 10;
        }

        // Clear the map area (start at col=1 for border)
        for (int r = 1; r <= availableVisibleRows; r++)
        {
            Console.SetCursorPosition(1, r);
            Console.Write(new string(' ', width));
        }

        int row = 0;
        for (int localY = 0; localY < printableRows; localY++)  // Top-down (Y=0 top)
        {
            StringBuilder line = new StringBuilder();

            for (int localX = 0; localX < width; localX++)
            {
                var key = (player.Root.GridX, player.Root.GridY, (localX, localY));
                if (localX == player.Root.LocalX && localY == player.Root.LocalY)
                {
                    line.Append('@');
                }
                else if (MapKey.TryGetValue(key, out Tile tile))
                {
                    line.Append(tile.AsciiToShow);
                }
                else
                {
                    line.Append(' ');  // Fallback
                }
            }

            Console.SetCursorPosition(1, row + 1);
            Console.Write(line.ToString());
            row++;
        }

        // Blank out unused rows
        for (int extraRow = printableRows; extraRow < availableVisibleRows; extraRow++)
        {
            Console.SetCursorPosition(1, extraRow + 1);
            Console.Write(new string(' ', width));
        }
    }
    public void FinalizeTiles()
    {
        foreach (var kvp in MapKey)
        {
            Tile tile = kvp.Value;

            if (tile.DeferredChecks.HasFlag(TileCheckType.NeighborRoofed))
            {
                tile.IsRoofed = TileHelpers.HasRoofedNeighbor(this, tile);
            }

            if (tile.DeferredChecks.HasFlag(TileCheckType.NeighborWalkable))
            {
                tile.IsWalkable = TileHelpers.HasWalkableNeighbor(this, tile);
            }

            // Clear after processing
            tile.DeferredChecks = TileCheckType.None;
        }
    }

    /// <summary>
    /// Checks if any neighboring tile around the given coordinates is roofed.
    /// </summary>


    // A method that returns a tile. Easy access to the MapKey
    public static Tile FindTile(Map mapKey, int GridX, int GridY, int LocalX, int LocalY)
    {
        if (MapKey.TryGetValue((GridX, GridY, (LocalX, LocalY)), out Tile tile))
        {
            return tile;
        }
        return new Tile { AsciiToShow = ' ', IsWalkable = false };  // Default non-walkable for missing (or make walkable if open world)
    }

    public Tile ProcessTile(char ascii, int gridX, int gridY, int LocalX, int LocalY)  // Removed ? (always return Tile)
    {
        // 1. Check dictionary first
        if (_tileHandlers.TryGetValue(ascii, out var handler))
            return handler(gridX, gridY, LocalX, LocalY, ascii);

        // Letters handled dynamically
        if (char.IsLetter(ascii))
        {
            // 2. Fall back to reflection method
            string methodName = "On" + ascii.ToString();
            Type type = typeof(TileProcessor);
            var method = type.GetMethod(methodName, BindingFlags.Public | BindingFlags.Static);
            if (method != null)
                return (Tile)method.Invoke(this, new object[] { gridX, gridY, LocalX, LocalY, ascii });

            // 3. If neither exist, warn and fall through to default
            Console.WriteLine($"Warning: No handler found for letter '{ascii}' (expected {methodName})");
        }

        // Non-letter characters always go to dictionary
        if (_tileHandlers.TryGetValue(ascii, out var fallbackHandler))
            return fallbackHandler(gridX, gridY, LocalX, LocalY, ascii);

        // Default for nothing found: e.g., empty space or unknown
        return new Tile
        {
            GridX = gridX,
            GridY = gridY,
            LocalX = LocalX,
            LocalY = LocalY,
            AsciiToShow = ascii,  // Or ' ' for unknowns
            IsWalkable = true  // Assume walkable unless border/wall; adjust per char (e.g., if (ascii == ' ') IsWalkable=true;)
        };
    }

}