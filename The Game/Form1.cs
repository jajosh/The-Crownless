using System.Security.Cryptography.Xml;
using System.ComponentModel;

namespace The_Game
{
    public partial class Form1 : Form
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public static GameEngine Game { get; set; }
        public Form1()
        {

            SQLitePCL.Batteries.Init();
            Game = GameFactory.Create();
            Game.Assembly();
            // Done! ??
            InitializeComponent();
            Game.Player.PlayerCharacter.Root.GridX = 0;
            Game.Player.PlayerCharacter.Root.GridY = 0;
            Game.Player.PlayerCharacter.Root.LocalX = 13;
            Game.Player.PlayerCharacter.Root.LocalY = 8;
            Game.Map.PrintWorld(Game.Player.PlayerCharacter, ctbTheMap);

        }
        private void ctbTheMap_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Save the player position
            int gridX = Game.Player.PlayerCharacter.Root.GridX;
            int gridY = Game.Player.PlayerCharacter.Root.GridY;
            int localX = Game.Player.PlayerCharacter.Root.LocalX;
            int localY = Game.Player.PlayerCharacter.Root.LocalY;
            GridObject currentGrid = GridRepository.Query(new { GridX = gridX, GridY = gridY });
            BugHunter.Log(DebugType.MOVEMENT, $"Old grid information - {gridX}, {gridY}, {localX}, {localY}", DebugLogSeverity.telemetry);

            BugHunter.Log(DebugType.MOVEMENT, $"Key press event - KEY: {e.KeyChar}", DebugLogSeverity.telemetry);
            // Movement calculation
            int deltaX = 0, deltaY = 0;
            switch (e.KeyChar)
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
            }
            BugHunter.Log(DebugType.MOVEMENT, $"Movement Delta x = {deltaX}, deltaY = {deltaY}", DebugLogSeverity.telemetry);

            if (currentGrid == null)
            {
                BugHunter.Log(DebugType.MAPRENDERING, "CurrentGrid is Null, nothing to print!!!", DebugLogSeverity.ERROR);
                return; // or handle appropriately
            }
            // Tentative new position
            int newLocalX = localX + deltaX;
            int newLocalY = localY + deltaY;
            int newGridX = gridX;
            int newGridY = gridY;
            BugHunter.Log(DebugType.MOVEMENT, $"Old coordinet information - {newGridX}, {newGridY}, {newLocalX}, {newLocalY}");


            // Wrap around if needed
            if (newLocalX < 0) { newLocalX = 50; newGridX--; }
            if (newLocalX > 50) { newLocalX = 0; newGridX++; }
            if (newLocalY < 0) { newLocalY = 24; newGridY--; }
            if (newLocalY > 24) { newLocalY = 0; newGridY++; }

            BugHunter.Log(DebugType.MOVEMENT, $"Checking new coordinets for new grid", DebugLogSeverity.telemetry);
            BugHunter.Log(DebugType.MOVEMENT, $"New coordinet information - {newGridX}, {newGridY}, {newLocalX}, {newLocalY}", DebugLogSeverity.telemetry);
            // Check if newGrid x and y matches currentGrid x and y and pulls the new grid if it isnt
            // If there is a new grid, reprint world
            Game.Player.PlayerCharacter.Root = new RootComponent(newGridX, newGridY, newLocalX, newLocalY);


            BugHunter.Log(DebugType.MOVEMENT, $" Updated player positional data - {Game.Player.PlayerCharacter.Root.GridX}, {Game.Player.PlayerCharacter.Root.GridY}, {Game.Player.PlayerCharacter.Root.LocalX}, {Game.Player.PlayerCharacter.Root.LocalY}", DebugLogSeverity.telemetry);
            if (gridX != newGridX || gridY != newGridY) // Else rerender the old and new tile
            {
                BugHunter.Log(DebugType.MOVEMENT, $"Player movement outside of bounds of current grid. Printing world at new grid location", DebugLogSeverity.telemetry);
                
                if (!Game.Map.PrintWorld(Game.Player.PlayerCharacter, ctbTheMap))
                {
                    Game.Player.PlayerCharacter.Root = new RootComponent(gridX, gridY, localX, localY);
                }

            }
            else if (newLocalX != localX || newLocalY !=localY )
                {
                BugHunter.Log(DebugType.MOVEMENT, $"Player movement within bounds of current grid", DebugLogSeverity.telemetry);
                // Find the new tile
                TileObject newtile = TileRepository.Query(new { RootGridX = newGridX, RootGridY = newGridY, RootLocalX = newLocalX, RootLocalY = newLocalY });
                newtile.Occupant = Game.Player.PlayerCharacter;
                // -- Manual Occupant changing for now
                // Find old tile
                TileObject oldTile = TileRepository.Query(new { RootGridX = gridX, RootGridY = gridY, RootLocalX = localX, RootLocalY = localY });

                //Updates the render progile of the new tile to match the player
                newtile.BaseRender = Game.Player.PlayerCharacter.Render;
                Game.Map.Append(newtile, ctbTheMap);
                Game.Map.Append(oldTile, ctbTheMap);
                }
            

        }

        private void pnlMessageBoard_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
