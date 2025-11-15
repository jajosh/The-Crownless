using System;
public partial class TileProcessor
{
    public static TileObject OnTree(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new TileObject(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    public static TileObject OnTree(int GridX, int GridY, int LocalX, int LocalY, char ascii)
    {
        TileObject tile = new TileObject(GridX, GridY, LocalX, LocalY, TileTypes.Tree, false, false, new(), ascii);
        tile.IsVegitation = true;
        tile.IsHarvestable = true;
        tile.HarvestableItem.Add()
    }



    #region === SPAWNS ===
    // @
    public static TileObject OnPlayerSpawn(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new TileObject(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    // &
    public static TileObject OnNPCSpawn(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new TileObject(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    // !
    public static TileObject OnQuestMarker(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new TileObject(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    // ♘
    public static TileObject OnKnightSpawn(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new TileObject(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    // ♖
    public static TileObject OnGuard(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new TileObject(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    // €
    public static TileObject OnRandomEventSpawn(int gridX, int gridY, int localX, int localY, char ascii)
    {
        return new TileObject(gridX, gridY, localX, localY, TileTypes.empty, true, false, new(), ascii)
        {
            DeferredChecks = TileCheckType.NeighborRoofed
        };
    }
    // ɤ
    public static TileObject OnPlantSpawn(int GridX, int GridY, int LocalX, int LocalY, char ascii)
    {
        TileObject tile = new TileObject(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), '.');
        return tile;
    }
    // ♥
    public static TileObject OnHeartSpawn(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new TileObject(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    // ♡
    public static TileObject OnHalfHeartSpawn(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new TileObject(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    #endregion
    #region === Shops ===
    // ⚔
    public static TileObject OnWeaponsShop(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new TileObject(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    // ⚒
    public static TileObject OnCraftingShop(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new TileObject(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    // ⛏
    public static TileObject OnRangerShop(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new TileObject(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    // ⛓
    public static TileObject OnMatieralShop(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new TileObject(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    // ÿ
    public static TileObject OnMarketStand(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new TileObject(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    #endregion
    #region === Faction ===
    // ♦
    public static TileObject OnFactionQuest(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new TileObject(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    // ♣
    public static TileObject OnFactionLeader(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new TileObject(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    // ♠
    public static TileObject OnFactionLegiet(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new TileObject(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    #endregion
    #region === Indicators ===
    // ♬
    public static TileObject OnSoundIndicator(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new TileObject(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    // !
    public static TileObject OnQuestMaker(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new TileObject(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    // ?
    public static TileObject OnQuestionMark(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new TileObject(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    // ☠
    public static TileObject OnPhysicalTrap(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new TileObject(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    // ☢
    public static TileObject OnMagicTrap(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new TileObject(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    // ☣
    public static TileObject OnLogicTrap(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new TileObject(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    #endregion
    #region === Walkable Terrain and Areas ===
    #region = Path =
    // .
    public static TileObject OnPath(int GridX, int GridY, int LocalX, int LocalY, char ascii)
    {
        TileObject tile = new TileObject(GridX, GridY, LocalX, LocalY, TileTypes.DirtPath, true, false, new(), ascii);
        tile.AddDescription(3, "Packed Earth");
        tile.AddDescription(3, "Patches of green grass litter the packed ground", GridBiomeType.BorelForest);
        tile.AddDescription(3, "You step over puddles of water", null, null, null, WeatherData.RainMedium);
        return tile;
    }
    // '
    public static TileObject OnInsideWalkablePath(int GridX, int GridY, int LocalX, int LocalY, char ascii)
    {
        TileObject tile = new TileObject(GridX, GridY, LocalX, LocalY, TileTypes.DirtPath, true, false, new(), ascii);
        tile.AddDescription(3, "Your footsteps echo across the room");
        return tile;
    }
    // …
    public static TileObject OnRoad(int GridX, int GridY, int LocalX, int LocalY, char ascii)
    {
        TileObject tile = new TileObject(GridX, GridY, LocalX, LocalY, TileTypes.DirtPath, true, false, new(), ascii);
        tile.AddDescription(3, "Packed Earth");
        return tile;
    }
    // ,
    public static TileObject OnGrass(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new TileObject(GridX, GridY, LocalX, LocalY, TileTypes.empty, true, false, new(), ascii);
    #endregion
    #region = PlantLife =
    // ♣
    public static TileObject OnTree(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new TileObject(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    // ,
    public static TileObject OnDeadTree(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new TileObject(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    #endregion
    #region Accent Pieces
    // ◎
    public static TileObject OnFairyRing(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new TileObject(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    // ◉
    public static TileObject OnBullseye(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new TileObject(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    // ●
    public static TileObject OnSmallStone1(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new TileObject(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    // ○
    public static TileObject OnSmallStone2(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new TileObject(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    // ▪
    public static TileObject OnSmallStone3(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new TileObject(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    // ▫
    public static TileObject OnGroundDetail(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new TileObject(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    // □
    public static TileObject OnObstaclesgrid(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new TileObject(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    #endregion
    #endregion
    #region === Non-Walkable Terrain and areas ===
    // 🌲
    public static TileObject OnForest(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new TileObject(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    // ▒
    public static TileObject OnImpassableTerrain(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new TileObject(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    // f
    public static TileObject OnThickForest(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new TileObject(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    // F
    public static TileObject OnDenseForest(int GridX, int GridY, int LocalX, int LocalY, char ascii)
    {
        TileObject tile = new TileObject(GridX, GridY, LocalX, LocalY, TileTypes.DirtPath, false, false, new(), ascii);
        tile.AddDescription(15, "Theres a rustling in the underbrush, a rabbit jumps out and scampers away", GridBiomeType.BorelForest);
        return tile;
    }
    // ■
    public static TileObject OnLargeStone(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new TileObject(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    // %
    public static TileObject OnBush(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new TileObject(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    #endregion
    #region === Design and Decorations ===
    #region = Outdoor Furnishings =
    // ˄
    public static TileObject OnTroughUpper(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new TileObject(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    // ˅
    public static TileObject OnTroughLower(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new TileObject(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    #endregion
    #region = Infoor furnishings =
    // <
    public static TileObject OnFurnaceLeft(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new TileObject(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    // >
    public static TileObject OnFurnaceRight(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new TileObject(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    // ✎
    public static TileObject OnWritingDesk(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new TileObject(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    // ✏
    public static TileObject OnNote(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new TileObject(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    // π
    public static TileObject OnCounter(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new TileObject(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    // ▭
    public static TileObject OnBed(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new TileObject(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    // ☩
    public static TileObject OnCross1(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new TileObject(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    // †
    public static TileObject OnCross2(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new TileObject(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    // ‡
    public static TileObject OnReligiousSymbol(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new TileObject(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    // ɦ
    public static TileObject OnPew(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new TileObject(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), 'ɦ');
    // ⛬
    public static TileObject OnAlter(int GridX, int GridY, int LocalX, int LocalY, char ascii)
    {
        TileObject tile = new TileObject(GridX, GridY, LocalX, LocalY, TileTypes.DirtPath, true, false, new(), ascii);
        tile.AddDescription(3, "Packed Earth");
        return tile;
    }
    #endregion
    #region = Containers =
    // █
    public static TileObject OnContainer(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new TileObject(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    // ▣
    public static TileObject OnChest(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new TileObject(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    // ⬜
    public static TileObject OnBarrel(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new TileObject(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    // Ṡ
    public static TileObject OnSecretChest(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new TileObject(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    #endregion
    #region = Box Drawing (UI or Structure Outlines) =
    // ┌, ┐, └, ┘, ─, │, ┼, ┬, ┴, ├, ┤
    public static TileObject OnWallCharacter(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new TileObject(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    // #, +, -, |, /, \, _
    public static TileObject OnSpecialBuilders(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new TileObject(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    // Ǒ
    public static TileObject OnWoodenPillar(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new TileObject(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    // = Doors and Entrances =
    // =
    public static TileObject OnDoor(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new TileObject(GridX, GridY, LocalX, LocalY, TileTypes.empty, true, true, new(), ascii);
    // ▶
    public static TileObject OnRightEntrance(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new TileObject(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    // ◀
    public static TileObject OnLeftEntrance(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new TileObject(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    #endregion
    #endregion
    #region === UI Decorations & Symbols ===
    // ★
    public static TileObject OnSolidStar(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new TileObject(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    // ☆
    public static TileObject OnEmptyStar(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new TileObject(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    // ✓
    public static TileObject OnCheckMark1(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new TileObject(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    // ✗
    public static TileObject OnCrossPatch1(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new TileObject(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    // ✘
    public static TileObject OnCrossPatch2(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new TileObject(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    // ✔
    public static TileObject OnCheckMark2(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new TileObject(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    // ↑
    public static TileObject OnUpArrow1(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new TileObject(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    // ↓
    public static TileObject OnDownArrow1(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new TileObject(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    // ←
    public static TileObject OnLeftArrow1(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new TileObject(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    // →
    public static TileObject OnRightArrow1(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new TileObject(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    // ↔
    public static TileObject OnSideSideArrow(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new TileObject(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    // ↕
    public static TileObject OnUpDownArrow(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new TileObject(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    // ⇦
    public static TileObject OnLeftArrow2(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new TileObject(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    // ⇩
    public static TileObject OnDownArrow2(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new TileObject(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    // ⇨
    public static TileObject OnRightArrow2(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new TileObject(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    // ⇧
    public static TileObject OnUpArrow2(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new TileObject(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    // ^
    public static TileObject OnUpArrow3(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new TileObject(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    // ☻
    public static TileObject OnCharacter(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new TileObject(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    // ☺
    public static TileObject OnSmileFace(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new TileObject(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    // ⚙
    public static TileObject OnMachinery(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new TileObject(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    // ‼
    public static TileObject OnAlert(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new TileObject(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    #endregion
    #region === Items ===
    // ♫
    public static TileObject OnMusicalInstruments(int GridX, int GridY, int LocalX, int LocalY, char ascii) =>
        new TileObject(GridX, GridY, LocalX, LocalY, TileTypes.empty, false, false, new(), ascii);
    #endregion
}