using GameNamespace;
using System;

public class EngineMovement
{
    private EngineGUI ui = new();
	public EngineMovement()
	{
	}
    /// <summary>
    /// Handles movement for the player.
    /// </summary>
    public void Move(SaveGame saveGame)
    {
        ConsoleKeyInfo keyInfo = Console.ReadKey(true);

        // Current player position
        int gridX = saveGame.PlayerCharacter.Gridx;
        int gridY = saveGame.PlayerCharacter.GridY;
        int localX = saveGame.PlayerCharacter.LocalX;
        int localY = saveGame.PlayerCharacter.LocalY;

        // Movement delta
        int deltaX = 0, deltaY = 0;
        switch (keyInfo.Key)
        {
            case ConsoleKey.W: deltaX = 1; break;
            case ConsoleKey.S: deltaX = -1; break;
            case ConsoleKey.A: deltaY = -1; break;
            case ConsoleKey.D: deltaY = 1; break;
            case ConsoleKey.Escape: MainClass.isRunning = false; return;
        }

        // Tentative new position
        int newLocalX = localX + deltaX;
        int newLocalY = localY + deltaY;
        int newGridX = gridX;
        int newGridY = gridY;

        // Constants for map size
        const int maxLocalX = 24;  // 25 rows
        const int maxLocalY = 50;  // 51 columns

        // Wrap around
        if (newLocalX > maxLocalX) { newLocalX = 0; newGridX++; MainClass.needRedraw = true; }
        if (newLocalX < 0) { newLocalX = maxLocalX; newGridX--; MainClass.needRedraw = true; }
        if (newLocalY > maxLocalY) { newLocalY = 0; newGridY++; MainClass.needRedraw = true; }
        if (newLocalY < 0) { newLocalY = maxLocalY; newGridY--; MainClass.needRedraw = true; }

        // Clamp grids
        newGridX = Math.Max(0, newGridX);
        newGridY = Math.Max(0, newGridY);

        // Lookup tile at new position
        Tile tileToMoveTo = Map.FindTile(saveGame.GameMap, newGridX, newGridY, newLocalX, newLocalY);

        if (tileToMoveTo == null || !tileToMoveTo.IsWalkable)
        {
            // Fire-and-forget (no await)
            _ = EngineGUI.ShowTemporaryMessage(3, ui.bottomRow - ui.ConsoleHeightChange(), "Tile not Walkable", 1000);

            // Do not update player position
            return;
        }

        // Save old position
        int oldGridX = gridX;
        int oldGridY = gridY;
        int oldLocalX = localX;
        int oldLocalY = localY;

        // Commit new position
        saveGame.PlayerCharacter.Gridx = newGridX;
        saveGame.PlayerCharacter.GridY = newGridY;
        saveGame.PlayerCharacter.LocalX = newLocalX;
        saveGame.PlayerCharacter.LocalY = newLocalY;
        EngineGUI.UpdataStats(saveGame.PlayerCharacter);
        // Redraw old tile
        Tile currentTile = Map.FindTile(saveGame.GameMap, oldGridX, oldGridY, oldLocalX, oldLocalY);
        Console.SetCursorPosition(GetConsoleCol(oldLocalY), GetConsoleRow(oldLocalX));
        Console.Write(currentTile.AsciiToShow);

        // Draw player at new location
        Console.SetCursorPosition(GetConsoleCol(newLocalY), GetConsoleRow(newLocalX));
        Console.Write('@');

        // Random tile description
        Random random = new();
        if (random.Next(10) < 2)
        {
            Grid currentGrid = Map.FindGrid(saveGame.GameMap, newGridX, newGridY);
            string text = Tile.PickADescription(tileToMoveTo, saveGame.Weather, currentGrid.Biome, currentGrid.SubBiome);
            _ = EngineGUI.WriteWithClearAnimation(0, 27, text);
        }
        Console.SetCursorPosition(1, 1);
    }
    public bool Move(SaveGame saveGame, NPC npc, int direction) // 1, 2, 3, 4, North, South, East, West
    {
        // Tags position, logic affects these before saving to the NPC
        int gridX = npc.Gridx;
        int gridY = npc.GridY;
        int localX = npc.LocalX;
        int localY = npc.LocalY;
        // Save old position
        int oldGridX = gridX;
        int oldGridY = gridY;
        int oldLocalX = localX;
        int oldLocalY = localY;
        // The logic of the move
        switch (direction)
        {
            case 1: // North
                if (localX == 25) { localX = 0; gridX++; }
                else localX++;
                break;
            case 2: // South
                if (localX == 0) { localX = 25; gridX--; }
                else localX--;
                break;
            case 3: // East
                if (localY == 51) { localY = 0; gridY++; }
                else localY++;
                break;
            case 4: // West
                if (localY == 0) { localY = 51; gridY--; }
                else localY--;
                break;
        }

        // Checking if the tile is walk able
        Tile tileToMoveTo = Map.FindTile(saveGame.GameMap, gridX, gridY, localX, localY);
        if (tileToMoveTo.IsWalkable)
        {
            // Redraw old tile
            Tile currentTile = Map.FindTile(saveGame.GameMap, oldGridX, oldGridY, oldLocalX, oldLocalY);
            Console.SetCursorPosition(GetConsoleCol(oldLocalY), GetConsoleRow(oldLocalX));
            Console.Write(currentTile.AsciiToShow);

            // Draw NPC at new location
            Console.SetCursorPosition(GetConsoleCol(localY), GetConsoleRow(localX));
            Console.Write(npc.ascii);
            // Saves the position to the NPC
            npc.Gridx = gridX;
            npc.GridY = gridY;
            npc.LocalX = localX;
            npc.LocalY = localY;
            return true; // Signals movement was a success
        }
        else return false; // Signals movement failed
    }

    // --- Helper methods for console positioning ---
    private int GetConsoleRow(int localX)
    {
        int mapPanelTop = 1;      // offset for top border of box
        int mapPanelHeight = 25;  // rows inside the box
        int row = mapPanelTop + (mapPanelHeight - 1) - localX; // flip bottom-to-top
        return Math.Clamp(row, 0, Console.WindowHeight - 1);
    }

    private int GetConsoleCol(int localY)
    {
        int mapPanelLeft = 1; // offset for left border of box
        return Math.Clamp(mapPanelLeft + localY, 0, Console.WindowWidth - 1);
    }

}
