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
	public bool Move(SaveGame saveGame)
	{
        ConsoleKeyInfo keyInfo = Console.ReadKey(true);

        int GridX = saveGame.player.Gridx;
        int GridY = saveGame.player.GridY;
        int LocalX = saveGame.player.LocalX;
        int LocalY = saveGame.player.LocalY;

        // Decide movement deltas
        if (keyInfo.Key == ConsoleKey.W) LocalX++;
        else if (keyInfo.Key == ConsoleKey.S) LocalX--;
        else if (keyInfo.Key == ConsoleKey.A) LocalY--;
        else if (keyInfo.Key == ConsoleKey.D) LocalY++;
        else if (keyInfo.Key == ConsoleKey.Escape)
        {
            return false;
        }

        // Wrap or adjust bounds BEFORE tile lookup
        if (LocalX > 24)
        {
            LocalX = 0; GridX++;
            needRedraw = true;
            Grid currentGrid = Map.FindGrid(saveGame.gameMap, GridX, GridY);
            Grid.NewGridDescription(currentGrid, saveGame.weather);
        }
        if (LocalX < 0)
        {
            LocalX = 24; GridX--;
            needRedraw = true;
            Grid currentGrid = Map.FindGrid(gameMap, GridX, GridY);
            Grid.NewGridDescription(currentGrid, saveGame.weather);
        }
        if (LocalY > 50)
        {
            LocalY = 0; GridY++;
            needRedraw = true;
            Grid currentGrid = Map.FindGrid(gameMap, GridX, GridY);
            Grid.NewGridDescription(currentGrid, weather);
        }
        if (LocalY < 0)
        {
            LocalY = 50; GridY--;
            needRedraw = true;
            Grid currentGrid = Map.FindGrid(gameMap, GridX, GridY);
            Grid.NewGridDescription(currentGrid, weather);
        }

        // Lookup tile
        Tile tileToMoveTo = Map.FindTile(gameMap, GridX, GridY, LocalX, LocalY);
        Tile currentTile = Map.FindTile(gameMap, player.Gridx, player.GridY, player.LocalX, player.LocalY);

        // Safety check (in case lookup failed)
        if (tileToMoveTo == null)
        {
            Console.WriteLine("Tile lookup failed!");
            continue;
        }

        if (tileToMoveTo.IsWalkable)
        {
            Random random = new();
            #region --- Player movement and rendering ---
            // Saves the old location data
            int oldGridX = player.Gridx;
            int oldGridY = player.GridY;
            int oldLocalX = player.LocalX;
            int oldLocalY = player.LocalY;
            // Updates the player location with the current location data
            player.Gridx = GridX;
            player.GridY = GridY;
            player.LocalX = LocalX;
            player.LocalY = LocalY;
            //Redraws just the tile the player is on and the tile they left
            int height = Console.WindowHeight - 4;
            consoleHeightChange = Console.WindowHeight - lastHeight;
            Console.SetCursorPosition(oldLocalY + 1, height - oldLocalX - 1 - consoleHeightChange);
            Console.Write(currentTile.AsciiToShow);
            Console.SetCursorPosition(LocalY + 1, height - LocalX - 1 - consoleHeightChange);
            Console.Write('@');
            #endregion


            if (random.Next(10) < 2)
            {
                Grid currentGrid = Map.FindGrid(gameMap, GridX, GridY);
                string text = Tile.PickADescription(tileToMoveTo, weather, currentGrid.Biome, currentGrid.SubBiome);
                EngineGUI.WriteWithClearAnimation(0, 27, text);

                Console.SetWindowPosition(0, 0);
            }
        }
        // Checks if a tile has a trigger

        else
        {
            consoleHeightChange = Console.WindowHeight - lastHeight;
            Console.SetCursorPosition(3, bottomLine);  // column 0, bottom row
            EngineGUI.WriteInPanel(2, 26, "Tile not Walkable");
            System.Threading.Thread.Sleep(750);
            needRedraw = true;

        }
        Console.SetCursorPosition(3, bottomRow - consoleHeightChange);
        //Map.PrintWorld(gameMap, player, 0);
    }

    // Tracks the days of the game and handles the weather and seasons. As of now this is purely fluff


}
    }
	/// <summary>
	/// Handles Movement for an NPC
	/// </summary>
	/// <param NPC> The NPC that is going to be moving. </param>
	public void Move(NPC)
	{

	}
}
