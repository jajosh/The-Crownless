using System;

public partial class TileProcessor
{
    public static Dictionary<char, Func<int, int, int, int, char, TileObject>> _tileHandlers { get; set; }
    public TileProcessor()
	{
        _tileHandlers = new Dictionary<char, Func<int, int, int, int, char, TileObject>>
        {
            #region === Spawn Points, Characters, and Triggers ===
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
}
