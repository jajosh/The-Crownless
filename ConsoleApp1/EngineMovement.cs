using GameNamespace;
using Spectre.Console;
using System;
using System.Threading.Tasks;

public class EngineMovement
{
    private EngineGUI ui = new();
	public EngineMovement()
	{
	}
    /// <summary>
    /// Handles movement for the player.
    /// </summary>
    // Updated Move (with prior fixes integrated)
    public void Move(SaveGame saveGame)
    {
        if (saveGame?.PlayerCharacter?.Root == null) return;  // Null guard

        ConsoleKeyInfo keyInfo = Console.ReadKey(true);

        // Current player position
        int gridX = saveGame.PlayerCharacter.Root.GridX;
        int gridY = saveGame.PlayerCharacter.Root.GridY;
        int localX = saveGame.PlayerCharacter.Root.LocalX;  // Horizontal (columns/X)
        int localY = saveGame.PlayerCharacter.Root.LocalY;  // Vertical (rows/Y, 0=top)

        // Movement delta
        int deltaX = 0, deltaY = 0;  // X horiz, Y vert
        switch (keyInfo.Key)
        {
            case ConsoleKey.W: deltaY = -1; break;  // Up: decrease Y (to top)
            case ConsoleKey.S: deltaY = 1; break;   // Down: increase Y (to bottom)
            case ConsoleKey.A: deltaX = -1; break;  // Left: decrease X
            case ConsoleKey.D: deltaX = 1; break;   // Right: increase X
            case ConsoleKey.Escape: MainClass.isRunning = false; return;
        }

        // Fetch current grid for dynamic sizes
        Grid currentGrid = Map.FindGrid(saveGame.GameMap, gridX, gridY);
        if (currentGrid?.GridMap == null || currentGrid.GridMap.Count == 0) return;

        int gridHeight = currentGrid.GridMap.Count;  // Vertical (rows/Y) = 25
        int gridWidth = 51;
        foreach (var rowStr in currentGrid.GridMap)
        {
            gridWidth = Math.Max(gridWidth, rowStr?.Length ?? 0);  // Horizontal (columns/X) = 51
        }

        // Tentative new position
        int newLocalX = localX + deltaX;
        int newLocalY = localY + deltaY;
        int newGridX = gridX;
        int newGridY = gridY;

        // Wrap around
        if (newLocalX < 0) { newLocalX = 50; newGridX--; }
        if (newLocalX > 50) { newLocalX = 0; newGridX++; }
        if (newLocalY < 0) { newLocalY = 24; newGridY++; }
        if (newLocalY > 24) { newLocalY = 0; newGridY--; }

        // If grid changed, fetch new grid
        Grid newGrid = Map.FindGrid(saveGame.GameMap, newGridX, newGridY);
        if (newGrid?.GridMap == null) return;

        // Lookup tile at new position
        Tile tileToMoveTo = Map.FindTile(saveGame.GameMap, newGridX, newGridY, newLocalX, newLocalY);

        if (tileToMoveTo == null || !tileToMoveTo.IsWalkable)
        {
            _ = EngineGUI.WriteMessageToPlayer("Tile not Walkable");
            //ssreturn;
        }

        // Save old position
        int oldGridX = gridX;
        int oldGridY = gridY;
        int oldLocalX = localX;
        int oldLocalY = localY;

        // Commit new position
        saveGame.PlayerCharacter.Root.GridX = newGridX;
        saveGame.PlayerCharacter.Root.GridY = newGridY;
        saveGame.PlayerCharacter.Root.LocalX = newLocalX;
        saveGame.PlayerCharacter.Root.LocalY = newLocalY;
        EngineGUI.UpdateStats(saveGame.PlayerCharacter);  // Fixed typo

        if (oldGridX == newGridX && oldGridY == newGridY)
        {
            // Redraw old tile (same grid)
            Tile currentTile = Map.FindTile(saveGame.GameMap, oldGridX, oldGridY, oldLocalX, oldLocalY);
            Console.SetCursorPosition(GetConsoleCol(oldLocalX), GetConsoleRow(oldLocalY));
            if (currentTile.Color == default)
                currentTile.Color = Color.White;
            string line = $"[{currentTile.Color.ToMarkup()}]{currentTile.AsciiToShow}[/]";
            AnsiConsole.Markup(line);

            // Draw player at new location
            Console.SetCursorPosition(GetConsoleCol(newLocalX), GetConsoleRow(newLocalY));
            Console.Write('@');
        }
        else
        {
            EngineGUI.Render(saveGame);
        }

        // Random tile description
        Random random = new();
        if (random.Next(10) < 2)
        {
            string text = Tile.PickADescription(tileToMoveTo, saveGame.Weather, newGrid.Biome, newGrid.SubBiome);
            _ = EngineGUI.WriteMessageToPlayer(text);
        }
        Console.SetCursorPosition(30, 0);
    }
    public static async Task<bool> Move(SaveGame saveGame, NPC npc, int direction) // 1, 2, 3, 4, North, South, East, West
    {
        // Tags position, logic affects these before saving to the NPC
        int gridX = npc.Root.GridX;
        int gridY = npc.Root.GridY;
        int localX = npc.Root.LocalX;
        int localY = npc.Root.LocalY;
        // Save old position
        int oldGridX = gridX;
        int oldGridY = gridY;
        int oldLocalX = localX;
        int oldLocalY = localY;

       switch(direction)
        {
            case 1:
                {
                    if (localY == 50) { localY = 0; gridY++; } else localY++;
                }
                break;
            case 2:
                {
                    if (localY == 0) { localY = 50; gridY--; } else localY--;
                }
                break;
            case 3:
                {
                    if (localX == 25) { localX = 0; gridX++; } else localX++;
                }
                break;
            case 4:
                {
                    if (localX == 0) { localX = 25; gridX--; } else localX--;
                }
                break;
        }
        Tile newTile = Map.FindTile(saveGame.GameMap, gridX, gridY, localX, localY);
        Tile oldtile = Map.FindTile(saveGame.GameMap, oldGridX, oldGridY, oldLocalX, oldLocalY);
        if (newTile.IsWalkable)
        {
            // Redraw old tile (same grid)
            Tile currentTile = Map.FindTile(saveGame.GameMap, oldGridX, oldGridY, oldLocalX, oldLocalY);
            Console.SetCursorPosition(GetConsoleCol(oldLocalX), GetConsoleRow(oldLocalY));
            if (currentTile.Color == null)
                currentTile.Color = Color.White;
            string line = $"[{currentTile.Color.ToMarkup()}]{currentTile.AsciiToShow}[/]";
            AnsiConsole.Markup(line);

            // Draw NPC at new location
            Console.SetCursorPosition(GetConsoleCol(localX), GetConsoleRow(localY));
            AnsiConsole.Markup(npc.Ascii);

            // Saves the location
            npc.Root.GridX = gridX;
            npc.Root.GridY = gridY;
            npc.Root.LocalX = localX;
            npc.Root.LocalY = localY;
            return true;
        }
        else
        {
            await EngineGUI.WriteMessageToPlayer("Error: NPC trying to move to a non-walkable tile");
            return false; // Changes are not saved, NPC does not move.
        }
    }

    // --- Helper methods for console positioning ---
    private static int GetConsoleRow(int localY)  // Rename param to localY (vertical)
    {
        int mapPanelTop = 1;      // offset for top border of box
        int mapPanelHeight = 25;  // rows inside the box (unused in calc now)
        int row = mapPanelTop + localY;  // No flip: localY=0 -> top row (1), increases downward
        return Math.Clamp(row, 0, Console.WindowHeight - 1);
    }

    private static int GetConsoleCol(int localX)  // Rename param to localX (horizontal)
    {
        int mapPanelLeft = 1; // offset for left border of box
        return Math.Clamp(mapPanelLeft + localX, 0, Console.WindowWidth - 1);
    }

}
