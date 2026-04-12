using MyGame.Controls;
using System;
using System.Security.Cryptography.X509Certificates;
using The_Game;

namespace The_Game;


public class MovementManager
{
	public MovementManager()
	{
		
	}
	public static int Move(ColorTextBox ctbTheMap, char e)
	{

        BugHunter.Log(DebugType.MOVEMENT, $"Movement Key '{e}'", DebugLogSeverity.telemetry);
        int result = 0;

        #region === Save the player position===
        BugHunter.Log(DebugType.MOVEMENT, $"Saving {Form1.Game.Player.PlayerCharacter.Name}'s position to the cache", DebugLogSeverity.telemetry);
        int gridX = Form1.Game.Player.PlayerCharacter.Root.GridX;
        int gridY = Form1.Game.Player.PlayerCharacter.Root.GridY;
        int localX = Form1.Game.Player.PlayerCharacter.Root.LocalX;
        int localY = Form1.Game.Player.PlayerCharacter.Root.LocalY;

        BugHunter.Log(DebugType.MOVEMENT, $"Saving Grid position to the cache", DebugLogSeverity.telemetry);
        GridObject currentGrid = GridRepository.Query(new { GridX = gridX, GridY = gridY });

        BugHunter.Log(DebugType.MOVEMENT, $"Saving Current Tile to the cache", DebugLogSeverity.telemetry);
        TileObject currentTile = TileRepository.Query(new { GridX = gridX, GridY = gridY, LocalX = localX, LocalY = localY });
        TileObject newTile = new();
        BugHunter.Log(DebugType.MOVEMENT, "Position cached.", DebugLogSeverity.telemetry);

        #endregion

        #region === Log KeyPress ===
        BugHunter.Log(DebugType.MOVEMENT, $"Translating movement Delta: {e}", DebugLogSeverity.telemetry); // Holding shift, or having caps lock breaks movement. 
        // Movement calculation
        int deltaX = 0, deltaY = 0;
        switch (e)
        {
            case 'w':
                deltaY = 1;
                break;
            case 's':
                deltaY = -1;
                break;
            case 'd':
                deltaX = 1;
                break;
            case 'a':
                deltaX = -1;
                break;
            default: return result; // Stops the block if an invalid response is found
        }
        BugHunter.Log(DebugType.MOVEMENT, $"Movement cached. Delta x = {deltaX}, delta Y = {deltaY}", DebugLogSeverity.telemetry);
        #endregion

        #region === Calculate new Position ===
        BugHunter.Log(DebugType.MAPRENDERING, "Calculating new position...", DebugLogSeverity.telemetry);
        if (currentGrid == null)
        {
            BugHunter.Log(DebugType.MAPRENDERING, "CurrentGrid is Null, nothing to print!!!", DebugLogSeverity.ERROR);
            return result; // or handle appropriately
        }
        // Tentative new position
        int newLocalX = localX + deltaX;
        int newLocalY = localY + deltaY;
        int newGridX = gridX;
        int newGridY = gridY;

        // Wrap around if needed, then grab potential new tile
        BugHunter.Log(DebugType.MOVEMENT, $"Checking new coordinets for new grid", DebugLogSeverity.telemetry);
        if (newLocalX < 0) { newLocalX = 50; newGridX--; }
        if (newLocalX > 50) { newLocalX = 0; newGridX++; }
        if (newLocalY < 0) { newLocalY = 24; newGridY--; }
        if (newLocalY > 24) { newLocalY = 0; newGridY++; }
        BugHunter.Log(DebugType.MOVEMENT, $"New potential coordinet information - {newGridX}, {newGridY}, {newLocalX}, {newLocalY}", DebugLogSeverity.telemetry);
        newTile = TileRepository.Query(new { GridX = newGridX, GridY = newGridY, LocalX = newLocalX, LocalY = newLocalY });
        // Find new tile try/catch block
        try
        {
            if (newLocalX != localX || newLocalY != localY)
            {

                BugHunter.Log(DebugType.MOVEMENT, "New Tile found!", DebugLogSeverity.telemetry);
            }
            else
            {
                BugHunter.Log(DebugType.MOVEMENT, "Old and New position the same, no new tile. MOVEMENT STOPPED", DebugLogSeverity.WARN);
                return result;
            }
        }
        catch
        {
            BugHunter.Log(DebugType.MOVEMENT, $"ERROR!! Failed to find new tile - {newGridX}, {newGridY}, {newLocalX}, {newLocalY}", DebugLogSeverity.FATAL);
        }
        BugHunter.Log(DebugType.MOVEMENT, "Movement alcultation is complete", DebugLogSeverity.telemetry);
        #endregion

        #region === Check for Tile Conditions === TEMP
        BugHunter.Log(DebugType.MOVEMENT, $"Checking {newTile.Components.Count} components...", DebugLogSeverity.telemetry);

        bool canWalk = false;

        foreach (var wrapper in newTile.Components)
        {
            // 1. Is the component itself null? (Switch statement failure)
            if (wrapper.TileComponent == null)
            {
                BugHunter.Log(DebugType.MOVEMENT, $"Wrapper for {wrapper.ComponentTypeName} has a NULL TileComponent!", DebugLogSeverity.WARN);
                continue;
            }

            // 2. Is it the right type?
            if (wrapper.TileComponent is IsWalkableComponent walkable)
            {
                BugHunter.Log(DebugType.MOVEMENT, $"Found Walkable Component. IsWalkable Value: {walkable.IsWalkable}", DebugLogSeverity.telemetry);
                if (walkable.IsWalkable)
                {
                    canWalk = true;
                    break;
                }
            }
            else
            {
                BugHunter.Log(DebugType.MOVEMENT, $"Component {wrapper.ComponentTypeName} is type {wrapper.TileComponent.GetType().Name}, not IsWalkableComponent", DebugLogSeverity.telemetry);
            }
        }

        if (canWalk)
        {
            BugHunter.Log(DebugType.MOVEMENT, "New tile is walkable.", DebugLogSeverity.telemetry);
        }
        else
        {
            BugHunter.Log(DebugType.MOVEMENT, "Movement blocked: No active WalkableComponent found.", DebugLogSeverity.INFO, true);
            return result;
        }
        #endregion
        

        #region === Move the Player ===
        Form1.Game.Player.PlayerCharacter.Root = new RootComponent(newGridX, newGridY, newLocalX, newLocalY);


        BugHunter.Log(DebugType.MOVEMENT, $" Updated player positional data - {Form1.Game.Player.PlayerCharacter.Root.GridX}, {Form1.Game.Player.PlayerCharacter.Root.GridY}, {Form1.Game.Player.PlayerCharacter.Root.LocalX}, {Form1.Game.Player.PlayerCharacter.Root.LocalY}", DebugLogSeverity.telemetry);
        #endregion

        #region === Update the map ===

        if (gridX != newGridX || gridY != newGridY) // Else rerender the old and new tile
        {
            BugHunter.Log(DebugType.MOVEMENT, $"Player movement outside of bounds of current grid. Printing world at new grid location", DebugLogSeverity.telemetry);

            if (!Form1.Game.Map.PrintWorld(Form1.Game.Player.PlayerCharacter, ctbTheMap))
            {
                Form1.Game.Player.PlayerCharacter.Root = new RootComponent(gridX, gridY, localX, localY);
            }

        }
        TileRepository tileDB = new TileRepository();
        List<TileObject> tiles = new();
        currentTile.Occupant = null;
        newTile.Occupant = Form1.Game.Player.PlayerCharacter;
        tiles.Add(currentTile);
        tiles.Add(newTile);
        Form1.Game.Map.Append(currentTile, ctbTheMap);
        Form1.Game.Map.Append(newTile, ctbTheMap);
        tileDB.SaveAllTilesToDatabase(tiles);
        #endregion
        return result;
    }

   
}
