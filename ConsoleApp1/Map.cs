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
    public Dictionary<(int, int, (int, int)), Tile> MapKey = new Dictionary<(int, int, (int, int)), Tile>();
    public Dictionary<(int, int), Grid>GridKey = new Dictionary<(int, int), Grid>();
    private const int GridWidth = 51;
    private const int GridHeight = 25;
    public static Dictionary<char, Func<int, int, int, int, char, Tile>>? _tileHandlers;



    public Map()
    {
        _tileHandlers = new Dictionary<char, Func<int, int, int, int, char, Tile>>
        {
            #region === Spawn Points, Characters, and Triggers ===
            { '@', OnPlayerSpawn },
            { '&', OnNPCSpawn },
            { '!', OnQuestMarker },
            { '?', OnQuestionMark },
            { '♘', OnKnightSpawn },
            { '♖', OnGuard },
            { '€', OnRandomEventSpawn },
            { 'ɤ', OnPlantSpawn },
            { '♥', OnHeartSpawn },
            { '♡', OnHalfHeartSpawn },
            { '♬', OnSoundIndicator },
            { '☠', OnPhysicalTrap },
            { '☢', OnMagicTrap },
            { '☣', OnLogicTrap },
            { 'ÿ', OnMarketStand },
            #endregion

            #region === Shops ===
            { '⚔', OnWeaponsShop },
            { '⚒', OnCraftingShop },
            { '⛏', OnRangerShop },
            { '⛓', OnMatieralShop },
            #endregion

            #region === Faction Related ===
            { '♦', OnFactionQuest },
            #endregion

            #region === Terrain & Walkable Areas ===
            { '.', OnPath },
            { ',', OnGrass },
            { '\'', OnInsideWalkablePath },
            { '…', OnRoad },
            { '♣', OnTree },
            { '♤', OnDeadTree },
            #endregion
            
            #region === Accent Pieces ===
            { '◎', OnFairyRing },
            { '◉', OnBullseye },
            { '●', OnSmallStone1 },
            { '○', OnSmallStone2 },
            { '▫', OnGroundDetail },
            { '▪', OnSmallStone3 },
            #endregion

            #region === Terrain & Non-Walkable Areas ===
            { '▒', OnImpassableTerrain },
            { 'f', OnThickForest },
            { 'F', OnDenseForest },
            { '■', OnLargeStone },
            { '□', OnObstaclesgrid },
            { '%', OnBush },
            #endregion

            #region === Structures & Furnishings ===
            { '˄', OnTroughUpper },
            { '˅', OnTroughLower },
            { '<', OnFurnaceLeft },
            { '>', OnFurnaceRight },
            { '✎', OnWritingDesk },
            { '✏', OnNote },
            { 'π', OnCounter },
            { '▭', OnBed },
            { '☩', OnCross1 },
            { '†', OnCross2 },
            { '‡', OnReligiousSymbol },
            { 'ɦ', OnPew },
            { '⛬', OnAlter},
            { '▣', OnChest },
            { '⬜', OnBarrel },
            { 'Ṡ', OnSecretChest },
            { '█', OnContainer },
            { 'Ǒ', OnWoodenPillar },

            #endregion
            #region === Walls ===
            // ┌, ┐, └, ┘, ─, │, ┼, ┬, ┴, ├, ┤  -> OnWallCharacter
            { '┌', OnWallCharacter },
            { '┐', OnWallCharacter },
            { '└', OnWallCharacter },
            { '┘', OnWallCharacter },
            { '─', OnWallCharacter },
            { '│', OnWallCharacter },
            { '┼', OnWallCharacter },
            { '┬', OnWallCharacter },
            { '┴', OnWallCharacter },
            { '├', OnWallCharacter },
            { '┤', OnWallCharacter },

            // #, +, -, |, /, \, _  -> OnSpecialBuilders
            { '#', OnSpecialBuilders },
            { '+', OnSpecialBuilders },
            { '-', OnSpecialBuilders },
            { '|', OnSpecialBuilders },
            { '/', OnSpecialBuilders },
            { '\\', OnSpecialBuilders }, // escape needed for backslash
            { '_', OnSpecialBuilders },
            #endregion 

            #region === Doors & Entrances ===
            { '=', OnDoor },
            { '▶', OnRightEntrance },
            { '◀', OnLeftEntrance },
            #endregion

            #region === UI Decorations & Symbols ===
            { '★', OnSolidStar },
            { '☆', OnEmptyStar },
            { '✓', OnCheckMark1 },
            { '✗', OnCrossPatch1 },
            { '✘', OnCrossPatch2 },
            { '✔', OnCheckMark2 },
            { '↑', OnUpArrow1 },
            { '↓', OnDownArrow1 },
            { '←', OnLeftArrow1 },
            { '→', OnRightArrow1 },
            { '↔', OnSideSideArrow },
            { '↕', OnUpDownArrow },
            { '⇦', OnLeftArrow2 },
            { '⇩', OnDownArrow2 },
            { '⇨', OnRightArrow2 },
            { '⇧', OnUpArrow2 },
            { '^', OnUpArrow3 },
            { '☻', OnCharacter },
            { '☺', OnSmileFace },
            { '⚙', OnMachinery },
            { '‼', OnAlert },
            { '♫', OnMusicalInstruments }
            #endregion
        };
    }
    public static Map LoadMapFromJson()
    {
        Map map = new Map();

        // Use the generic loader
        List<Grid> grids = JsonLoader.LoadFromJson<List<Grid>>(FileManager.TheGridFilePath)
                          ?? new List<Grid>();

        foreach (var grid in grids)
        {
            map.GridKey[(grid.XPosition, grid.YPosition)] = grid;
            if (grid.GridMap == null) continue;

            for (int LocalX = 0; LocalX < grid.GridMap.Count; LocalX++)
            {
                string line = grid.GridMap[LocalX];
                int flippedX = (grid.GridMap.Count - 1) - LocalX;

                for (int LocalY = 0; LocalY < line.Length; LocalY++)
                {
                    char c = line[LocalY];
                    Tile? tile = map.ProcessTile(c, grid.XPosition, grid.YPosition, flippedX, LocalY);

                    if (tile != null)
                    {
                        map.MapKey[(grid.XPosition, grid.YPosition, (flippedX, LocalY))] = tile;
                    }
                }
            }
        }

        map.FinalizeTiles();
        return map;
    }
    public static Grid? FindGrid(Map map, int x, int y)
    {
        if (map.GridKey.TryGetValue((x, y), out Grid grid))
        {
            return grid;
        }
        return null; // or throw an exception if you prefer
    }
    public static Tile FindTile(Map mapKey, int GridX, int GridY, int LocalX, int LocalY)
    {
        if (mapKey.MapKey.TryGetValue((GridX, GridY, (LocalX, LocalY)), out Tile tile))
        {
            return tile;
        }

        return null;
    }
    public static void PrintGrid(int gridX, int gridY)
    {
        Map map = new Map();
        const int gridSize = 50;

        for (int row = 0; row < gridSize; row++)
        {
            StringBuilder line = new StringBuilder();

            for (int col = 0; col < gridSize; col++)
            {
                if (map.MapKey.TryGetValue((gridX, gridY, (row, col)), out Tile tile))
                {
                    line.Append(tile.AsciiToShow);
                }
                else
                {
                    line.Append('F'); // empty space if no tile exists
                }
            }

            Console.WriteLine(line.ToString());
        }
    }
    public static void PrintWorld(Map map, Player player, int reservedLines)
    {
        if (map.MapKey == null || map.MapKey.Count == 0)
        {
            Console.WriteLine("Map is empty! Nothing to print.");
            return;
        }

        // Calculate how many rows we can actually print
        int totalMapRows = GridHeight; // assuming one grid vertically, adjust if multiple
        int printableRows = Math.Max(0, totalMapRows - reservedLines);
        int row = 0;
        for (int LocalX = GridHeight  - 1; LocalX >= 0; LocalX--)
        {
            StringBuilder line = new StringBuilder();

            for (int LocalY = 0; LocalY < GridWidth; LocalY++)
            {
                // Compute absolute coordinates in the map
                var key = (player.Gridx, player.GridY, (LocalX, LocalY));
                if (key == (player.Gridx, player.GridY, (player.LocalX, player.LocalY)))
                {
                    line.Append('@');
                }
                else if (map.MapKey.TryGetValue(key, out Tile tile))
                {
                    line.Append(tile.AsciiToShow);
                }
                else
                {
                    line.Append(' '); // empty space for missing tiles
                }
            }
            Console.SetCursorPosition(1, row+1);
            row++;
            Console.Write(line.ToString());
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

    #region method for each char
    #region === SPAWNS ===
    // @
    private Tile OnPlayerSpawn(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new Tile(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    // &
    private Tile OnNPCSpawn(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new Tile(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    // !
    private Tile OnQuestMarker(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new Tile(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    // ♘
    private Tile OnKnightSpawn(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new Tile(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    // ♖
    private Tile OnGuard(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new Tile(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    // €
    private Tile OnRandomEventSpawn(int gridX, int gridY, int localX, int localY, char ascii)
    {
        return new Tile(gridX, gridY, localX, localY, TileTypes.empty, true, false, new(), ascii)
        {
            DeferredChecks = TileCheckType.NeighborRoofed
        };
    }

    // ɤ
    private Tile OnPlantSpawn(int GridX, int GridY, int LocalX, int LocalY, char ascii)
    {
        Tile tile = new Tile(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), '.');
        return tile;
    }
    // ♥
    private Tile OnHeartSpawn(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new Tile(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    // ♡
    private Tile OnHalfHeartSpawn(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new Tile(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    #endregion


    #region === Shops ===
    // ⚔
    private Tile OnWeaponsShop(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new Tile(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    // ⚒
    private Tile OnCraftingShop(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new Tile(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    // ⛏
    private Tile OnRangerShop(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new Tile(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    // ⛓
    private Tile OnMatieralShop(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new Tile(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    // ÿ
    private Tile OnMarketStand(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new Tile(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    #endregion


    #region === Faction ===
    // ♦
    private Tile OnFactionQuest(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new Tile(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    // ♣
    private Tile OnFactionLeader(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new Tile(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    // ♠
    private Tile OnFactionLegiet(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new Tile(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    #endregion


    #region === Indicators ===
    // ♬
    private Tile OnSoundIndicator(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new Tile(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    // !
    private Tile OnQuestMaker(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new Tile(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    // ?
    private Tile OnQuestionMark(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new Tile(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    // ☠
    private Tile OnPhysicalTrap(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new Tile(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    // ☢
    private Tile OnMagicTrap(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new Tile(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    // ☣
    private Tile OnLogicTrap(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new Tile(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    #endregion


    #region === Walkable Terrain and Areas ===

    #region = Path =
    // .
    private Tile OnPath(int GridX, int GridY, int LocalX, int LocalY, char ascii)
    {
        Tile tile = new Tile(GridX, GridY, LocalX, LocalY, TileTypes.DirtPath, true, false, new(), ascii);
        tile.AddDescription(3, "Packed Earth");
        tile.AddDescription(3, "Patches of green grass litter the packed ground", GridBiomeType.BorelForst);
        tile.AddDescription(3, "You step over puddles of water",null, null, null, WeatherData.Rainy);
        return tile;
    }
    // '
    private Tile OnInsideWalkablePath(int GridX, int GridY, int LocalX, int LocalY, char ascii)
    {
        Tile tile = new Tile(GridX, GridY, LocalX, LocalY, TileTypes.DirtPath, true, false, new(), ascii);
        tile.AddDescription(3, "Your footsteps echo across the room");
        return tile;
    }
    // …
    private Tile OnRoad(int GridX, int GridY, int LocalX, int LocalY, char ascii)
    {
        Tile tile = new Tile(GridX, GridY, LocalX, LocalY, TileTypes.DirtPath, true, false, new(), ascii);
        tile.AddDescription(3, "Packed Earth");
        return tile;
    }
    // ,
    private Tile OnGrass(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new Tile(GridX, GridY, LocalX, LocalY, TileTypes.empty, true, false, new(), ascii);
    #endregion

    #region = PlantLife =
    // ♣
    private Tile OnTree(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new Tile(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    // ,
    private Tile OnDeadTree(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new Tile(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    #endregion

    #region Accent Pieces
    // ◎
    private Tile OnFairyRing(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new Tile(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    // ◉
    private Tile OnBullseye(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new Tile(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    // ●
    private Tile OnSmallStone1(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new Tile(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    // ○
    private Tile OnSmallStone2(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new Tile(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    // ▪
    private Tile OnSmallStone3(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new Tile(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    // ▫
    private Tile OnGroundDetail(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new Tile(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    // □
    private Tile OnObstaclesgrid(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new Tile(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    #endregion

    #endregion


    #region === Non-Walkable Terrain and areas ===
    // 🌲
    private Tile OnForest(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new Tile(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    // ▒
    private Tile OnImpassableTerrain(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new Tile(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    // f
    private Tile OnThickForest(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new Tile(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    // F
    private Tile OnDenseForest(int GridX, int GridY, int LocalX, int LocalY, char ascii)
    {
        Tile tile = new Tile(GridX, GridY, LocalX, LocalY, TileTypes.DirtPath, false, false, new(), ascii);
        tile.AddDescription(15, "Theres a rustling in the underbrush, a rabbit jumps out and scampers away", GridBiomeType.BorelForest);
        return tile;
    }
    // ■
    private Tile OnLargeStone(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new Tile(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    // %
    private Tile OnBush(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new Tile(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    #endregion


    #region === Design and Decorations ===

    #region = Outdoor Furnishings =
    // ˄
    private Tile OnTroughUpper(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new Tile(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    // ˅
    private Tile OnTroughLower(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new Tile(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    #endregion

    #region = Infoor furnishings =
    // <
    private Tile OnFurnaceLeft(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new Tile(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    // >
    private Tile OnFurnaceRight(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new Tile(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    // ✎
    private Tile OnWritingDesk(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new Tile(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    // ✏
    private Tile OnNote(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new Tile(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    // π
    private Tile OnCounter(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new Tile(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    // ▭
    private Tile OnBed(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new Tile(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    // ☩
    private Tile OnCross1(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new Tile(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    // †
    private Tile OnCross2(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new Tile(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    // ‡
    private Tile OnReligiousSymbol(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new Tile(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    // ɦ
    private Tile OnPew(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new Tile(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), 'ɦ');
    // ⛬
    private Tile OnAlter(int GridX, int GridY, int LocalX, int LocalY, char ascii)
    {
        Tile tile = new Tile(GridX, GridY, LocalX, LocalY, TileTypes.DirtPath, true, false, new(), ascii);
        tile.AddDescription(3, "Packed Earth");
        return tile;
    }
    #endregion

    #region = Containers =
    // █
    private Tile OnContainer(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new Tile(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    // ▣
    private Tile OnChest(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new Tile(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    // ⬜
    private Tile OnBarrel(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new Tile(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    // Ṡ
    private Tile OnSecretChest(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new Tile(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    #endregion

    #region = Box Drawing (UI or Structure Outlines) =
    // ┌, ┐, └, ┘, ─, │, ┼, ┬, ┴, ├, ┤
    private Tile OnWallCharacter(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new Tile(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    // #, +, -, |, /, \, _
    private Tile OnSpecialBuilders(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new Tile(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    // Ǒ
    private Tile OnWoodenPillar(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new Tile(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);

    // = Doors and Entrances =
    // =
    private Tile OnDoor(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new Tile(GridX, GridY, LocalX, LocalY, TileTypes.empty, true, true, new(), ascii);
    // ▶
    private Tile OnRightEntrance(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new Tile(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    // ◀
    private Tile OnLeftEntrance(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new Tile(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    #endregion

    #endregion


    #region === UI Decorations & Symbols ===
    // ★
    private Tile OnSolidStar(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new Tile(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    // ☆
    private Tile OnEmptyStar(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new Tile(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    // ✓
    private Tile OnCheckMark1(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new Tile(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    // ✗
    private Tile OnCrossPatch1(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new Tile(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    // ✘
    private Tile OnCrossPatch2(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new Tile(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    // ✔
    private Tile OnCheckMark2(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new Tile(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    // ↑
    private Tile OnUpArrow1(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new Tile(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    // ↓
    private Tile OnDownArrow1(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new Tile(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    // ←
    private Tile OnLeftArrow1(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new Tile(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    // →
    private Tile OnRightArrow1(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new Tile(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    // ↔
    private Tile OnSideSideArrow(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new Tile(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    // ↕
    private Tile OnUpDownArrow(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new Tile(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    // ⇦
    private Tile OnLeftArrow2(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new Tile(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    // ⇩
    private Tile OnDownArrow2(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new Tile(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    // ⇨
    private Tile OnRightArrow2(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new Tile(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    // ⇧
    private Tile OnUpArrow2(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new Tile(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    // ^
    private Tile OnUpArrow3(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new Tile(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    // ☻
    private Tile OnCharacter(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new Tile(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    // ☺
    private Tile OnSmileFace(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new Tile(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    // ⚙
    private Tile OnMachinery(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new Tile(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    // ‼
    private Tile OnAlert(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new Tile(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    #endregion


    #region === Items ===
    // ♫
    private Tile OnMusicalInstruments(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new Tile(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    #endregion


    #region === Alphabet ===
    // Uppercase A-Z
    private Tile OnA(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
    new Tile(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    private Tile OnB(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new Tile(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    private Tile OnC(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new Tile(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    private Tile OnD(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new Tile(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    private Tile OnE(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new Tile(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    private Tile OnF(int GridX, int GridY, int LocalX, int LocalY, char ascii)
    {
        Tile tile = new Tile(GridX, GridY, LocalX, LocalY, TileTypes.Forest, false, false, new(), ascii);
        return tile;
    }
    private Tile OnG(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new Tile(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    private Tile OnH(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new Tile(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    private Tile OnI(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new Tile(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    private Tile OnJ(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new Tile(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    private Tile OnK(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new Tile(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    private Tile OnL(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new Tile(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    private Tile OnM(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new Tile(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    private Tile OnN(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new Tile(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    private Tile OnO(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new Tile(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    private Tile OnP(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new Tile(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    private Tile OnQ(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new Tile(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    private Tile OnR(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new Tile(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    private Tile OnS(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new Tile(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    private Tile OnT(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new Tile(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    private Tile OnU(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new Tile(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    private Tile OnV(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new Tile(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    private Tile OnW(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new Tile(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    private Tile OnX(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new Tile(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    private Tile OnY(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new Tile(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    private Tile OnZ(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new Tile(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);

    // Lowercase a-z
    private Tile Ona(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new Tile(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    private Tile Onb(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new Tile(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    private Tile Onc(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new Tile(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    private Tile Ond(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new Tile(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    private Tile One(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new Tile(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    private Tile Onf(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new Tile(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    private Tile Ong(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new Tile(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    private Tile Onh(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new Tile(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    private Tile Oni(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new Tile(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    private Tile Onj(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new Tile(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    private Tile Onk(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new Tile(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    private Tile Onl(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new Tile(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    private Tile Onm(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new Tile(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    private Tile Onn(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new Tile(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    private Tile Ono(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new Tile(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    private Tile Onp(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new Tile(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    private Tile Onq(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new Tile(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    private Tile Onr(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new Tile(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    private Tile Ons(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new Tile(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    private Tile Ont(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new Tile(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    private Tile Onu(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new Tile(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    private Tile Onv(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new Tile(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    private Tile Onw(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new Tile(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    private Tile Onx(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new Tile(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    private Tile Ony(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new Tile(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    private Tile Onz(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new Tile(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    #endregion

    #endregion

    private Tile? ProcessTile(char ascii, int gridX, int gridY, int LocalX, int LocalY)
    {
        
        // 1. Check dictionary first
        if (_tileHandlers.TryGetValue(ascii, out var handler))
            return handler(gridX, gridY, LocalX, LocalY, ascii);
        // Letters handled dynamically
        if (char.IsLetter(ascii))
        {
            // 2. Fall back to reflection method
            string methodName = "On" + ascii;
            var method = this.GetType().GetMethod(methodName, BindingFlags.NonPublic | BindingFlags.Instance);
            if (method != null)
                return (Tile)method.Invoke(this, new object[] { gridX, gridY, LocalX, LocalY, ascii });

            // 3. If neither exist, warn
            Console.WriteLine($"Warning: No handler found for letter '{ascii}' (expected {methodName})");
            return null;
        }

        // Non-letter characters always go to dictionary
        if (_tileHandlers.TryGetValue(ascii, out var fallbackHandler))
            return fallbackHandler(gridX, gridY, LocalX, LocalY, ascii);

        // Nothing found
        return null;
    }

}