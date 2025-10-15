using System;

public class TileProcessor
{
	public TileProcessor()
	{
	}
    #region === SPAWNS ===
    // @
    public static Tile OnPlayerSpawn(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new Tile(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    // &
    public static Tile OnNPCSpawn(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new Tile(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    // !
    public static Tile OnQuestMarker(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new Tile(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    // ♘
    public static Tile OnKnightSpawn(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new Tile(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    // ♖
    public static Tile OnGuard(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new Tile(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    // €
    public static Tile OnRandomEventSpawn(int gridX, int gridY, int localX, int localY, char ascii)
    {
        return new Tile(gridX, gridY, localX, localY, TileTypes.empty, true, false, new(), ascii)
        {
            DeferredChecks = TileCheckType.NeighborRoofed
        };
    }

    // ɤ
    public static Tile OnPlantSpawn(int GridX, int GridY, int LocalX, int LocalY, char ascii)
    {
        Tile tile = new Tile(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), '.');
        return tile;
    }
    // ♥
    public static Tile OnHeartSpawn(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new Tile(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    // ♡
    public static Tile OnHalfHeartSpawn(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new Tile(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    #endregion


    #region === Shops ===
    // ⚔
    public static Tile OnWeaponsShop(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new Tile(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    // ⚒
    public static Tile OnCraftingShop(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new Tile(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    // ⛏
    public static Tile OnRangerShop(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new Tile(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    // ⛓
    public static Tile OnMatieralShop(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new Tile(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    // ÿ
    public static Tile OnMarketStand(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new Tile(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    #endregion


    #region === Faction ===
    // ♦
    public static Tile OnFactionQuest(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new Tile(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    // ♣
    public static Tile OnFactionLeader(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new Tile(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    // ♠
    public static Tile OnFactionLegiet(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new Tile(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    #endregion


    #region === Indicators ===
    // ♬
    public static Tile OnSoundIndicator(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new Tile(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    // !
    public static Tile OnQuestMaker(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new Tile(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    // ?
    public static Tile OnQuestionMark(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new Tile(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    // ☠
    public static Tile OnPhysicalTrap(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new Tile(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    // ☢
    public static Tile OnMagicTrap(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new Tile(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    // ☣
    public static Tile OnLogicTrap(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new Tile(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    #endregion


    #region === Walkable Terrain and Areas ===

    #region = Path =
    // .
    public static Tile OnPath(int GridX, int GridY, int LocalX, int LocalY, char ascii)
    {
        Tile tile = new Tile(GridX, GridY, LocalX, LocalY, TileTypes.DirtPath, true, false, new(), ascii);
        tile.AddDescription(3, "Packed Earth");
        tile.AddDescription(3, "Patches of green grass litter the packed ground", GridBiomeType.BorelForest);
        tile.AddDescription(3, "You step over puddles of water", null, null, null, WeatherData.Rainy);
        return tile;
    }
    // '
    public static Tile OnInsideWalkablePath(int GridX, int GridY, int LocalX, int LocalY, char ascii)
    {
        Tile tile = new Tile(GridX, GridY, LocalX, LocalY, TileTypes.DirtPath, true, false, new(), ascii);
        tile.AddDescription(3, "Your footsteps echo across the room");
        return tile;
    }
    // …
    public static Tile OnRoad(int GridX, int GridY, int LocalX, int LocalY, char ascii)
    {
        Tile tile = new Tile(GridX, GridY, LocalX, LocalY, TileTypes.DirtPath, true, false, new(), ascii);
        tile.AddDescription(3, "Packed Earth");
        return tile;
    }
    // ,
    public static Tile OnGrass(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new Tile(GridX, GridY, LocalX, LocalY, TileTypes.empty, true, false, new(), ascii);
    #endregion

    #region = PlantLife =
    // ♣
    public static Tile OnTree(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new Tile(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    // ,
    public static Tile OnDeadTree(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new Tile(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    #endregion

    #region Accent Pieces
    // ◎
    public static Tile OnFairyRing(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new Tile(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    // ◉
    public static Tile OnBullseye(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new Tile(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    // ●
    public static Tile OnSmallStone1(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new Tile(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    // ○
    public static Tile OnSmallStone2(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new Tile(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    // ▪
    public static Tile OnSmallStone3(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new Tile(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    // ▫
    public static Tile OnGroundDetail(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new Tile(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    // □
    public static Tile OnObstaclesgrid(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new Tile(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    #endregion

    #endregion


    #region === Non-Walkable Terrain and areas ===
    // 🌲
    public static Tile OnForest(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new Tile(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    // ▒
    public static Tile OnImpassableTerrain(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new Tile(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    // f
    public static Tile OnThickForest(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new Tile(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    // F
    public static Tile OnDenseForest(int GridX, int GridY, int LocalX, int LocalY, char ascii)
    {
        Tile tile = new Tile(GridX, GridY, LocalX, LocalY, TileTypes.DirtPath, false, false, new(), ascii);
        tile.AddDescription(15, "Theres a rustling in the underbrush, a rabbit jumps out and scampers away", GridBiomeType.BorelForest);
        return tile;
    }
    // ■
    public static Tile OnLargeStone(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new Tile(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    // %
    public static Tile OnBush(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new Tile(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    #endregion


    #region === Design and Decorations ===

    #region = Outdoor Furnishings =
    // ˄
    public static Tile OnTroughUpper(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new Tile(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    // ˅
    public static Tile OnTroughLower(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new Tile(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    #endregion

    #region = Infoor furnishings =
    // <
    public static Tile OnFurnaceLeft(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new Tile(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    // >
    public static Tile OnFurnaceRight(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new Tile(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    // ✎
    public static Tile OnWritingDesk(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new Tile(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    // ✏
    public static Tile OnNote(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new Tile(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    // π
    public static Tile OnCounter(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new Tile(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    // ▭
    public static Tile OnBed(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new Tile(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    // ☩
    public static Tile OnCross1(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new Tile(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    // †
    public static Tile OnCross2(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new Tile(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    // ‡
    public static Tile OnReligiousSymbol(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new Tile(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    // ɦ
    public static Tile OnPew(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new Tile(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), 'ɦ');
    // ⛬
    public static Tile OnAlter(int GridX, int GridY, int LocalX, int LocalY, char ascii)
    {
        Tile tile = new Tile(GridX, GridY, LocalX, LocalY, TileTypes.DirtPath, true, false, new(), ascii);
        tile.AddDescription(3, "Packed Earth");
        return tile;
    }
    #endregion

    #region = Containers =
    // █
    public static Tile OnContainer(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new Tile(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    // ▣
    public static Tile OnChest(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new Tile(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    // ⬜
    public static Tile OnBarrel(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new Tile(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    // Ṡ
    public static Tile OnSecretChest(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new Tile(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    #endregion

    #region = Box Drawing (UI or Structure Outlines) =
    // ┌, ┐, └, ┘, ─, │, ┼, ┬, ┴, ├, ┤
    public static Tile OnWallCharacter(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new Tile(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    // #, +, -, |, /, \, _
    public static Tile OnSpecialBuilders(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new Tile(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    // Ǒ
    public static Tile OnWoodenPillar(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new Tile(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);

    // = Doors and Entrances =
    // =
    public static Tile OnDoor(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new Tile(GridX, GridY, LocalX, LocalY, TileTypes.empty, true, true, new(), ascii);
    // ▶
    public static Tile OnRightEntrance(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new Tile(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    // ◀
    public static Tile OnLeftEntrance(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new Tile(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    #endregion

    #endregion


    #region === UI Decorations & Symbols ===
    // ★
    public static Tile OnSolidStar(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new Tile(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    // ☆
    public static Tile OnEmptyStar(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new Tile(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    // ✓
    public static Tile OnCheckMark1(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new Tile(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    // ✗
    public static Tile OnCrossPatch1(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new Tile(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    // ✘
    public static Tile OnCrossPatch2(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new Tile(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    // ✔
    public static Tile OnCheckMark2(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new Tile(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    // ↑
    public static Tile OnUpArrow1(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new Tile(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    // ↓
    public static Tile OnDownArrow1(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new Tile(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    // ←
    public static Tile OnLeftArrow1(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new Tile(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    // →
    public static Tile OnRightArrow1(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new Tile(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    // ↔
    public static Tile OnSideSideArrow(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new Tile(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    // ↕
    public static Tile OnUpDownArrow(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new Tile(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    // ⇦
    public static Tile OnLeftArrow2(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new Tile(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    // ⇩
    public static Tile OnDownArrow2(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new Tile(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    // ⇨
    public static Tile OnRightArrow2(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new Tile(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    // ⇧
    public static Tile OnUpArrow2(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new Tile(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    // ^
    public static Tile OnUpArrow3(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new Tile(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    // ☻
    public static Tile OnCharacter(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new Tile(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    // ☺
    public static Tile OnSmileFace(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new Tile(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    // ⚙
    public static Tile OnMachinery(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new Tile(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    // ‼
    public static Tile OnAlert(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new Tile(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    #endregion


    #region === Items ===
    // ♫
    public static Tile OnMusicalInstruments(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new Tile(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    #endregion


    #region === Alphabet ===
    // Uppercase A-Z
    public static Tile OnA(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
    new Tile(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    public static Tile OnB(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new Tile(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    public static Tile OnC(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new Tile(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    public static Tile OnD(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new Tile(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    public static Tile OnE(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new Tile(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    public static Tile OnF(int GridX, int GridY, int LocalX, int LocalY, char ascii)
    {
        Tile tile = new Tile(GridX, GridY, LocalX, LocalY, TileTypes.Forest, false, false, new(), ascii);
        return tile;
    }
    public static Tile OnG(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new Tile(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    public static Tile OnH(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new Tile(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    public static Tile OnI(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new Tile(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    public static Tile OnJ(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new Tile(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    public static Tile OnK(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new Tile(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    public static Tile OnL(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new Tile(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    public static Tile OnM(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new Tile(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    public static Tile OnN(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new Tile(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    public static Tile OnO(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new Tile(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    public static Tile OnP(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new Tile(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    public static Tile OnQ(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new Tile(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    public static Tile OnR(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new Tile(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    public static Tile OnS(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new Tile(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    public static Tile OnT(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new Tile(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    public static Tile OnU(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new Tile(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    public static Tile OnV(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new Tile(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    private Tile OnW(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new Tile(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    public static Tile OnX(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new Tile(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    public static Tile OnY(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new Tile(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    public static Tile OnZ(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new Tile(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);

    // Lowercase a-z
    public static Tile Ona(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new Tile(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    public static Tile Onb(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new Tile(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    public static Tile Onc(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new Tile(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    public static Tile Ond(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new Tile(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    public static Tile One(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new Tile(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    public static Tile Onf(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new Tile(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    public static Tile Ong(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new Tile(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    public static Tile Onh(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new Tile(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    public static Tile Oni(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new Tile(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    public static Tile Onj(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new Tile(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    public static Tile Onk(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new Tile(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    public static Tile Onl(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new Tile(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    public static Tile Onm(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new Tile(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    public static Tile Onn(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new Tile(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    public static Tile Ono(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new Tile(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    public static Tile Onp(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new Tile(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    public static Tile Onq(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new Tile(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    public static Tile Onr(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new Tile(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    public static Tile Ons(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new Tile(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    public static Tile Ont(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new Tile(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    public static Tile Onu(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new Tile(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    public static Tile Onv(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new Tile(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    public static Tile Onw(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new Tile(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    public static Tile Onx(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new Tile(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    public static Tile Ony(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new Tile(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    public static Tile Onz(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new Tile(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    #endregion

}
