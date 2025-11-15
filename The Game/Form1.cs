using System.Security.Cryptography.Xml;
using System.ComponentModel;

namespace The_Game
{
    public partial class Form1 : Form
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public GameEngine Game { get; set; }
        public Form1()
        {
            // Builds the game. 
            Game = GameFactory.Create();
            Game.Assembly();
            // Done! ??
            InitializeComponent();
            Game.Player.PlayerCharacter.Root.GridX = 0;
            Game.Player.PlayerCharacter.Root.GridY = 0;
            Game.Player.PlayerCharacter.Root.LocalX = 13;
            Game.Player.PlayerCharacter.Root.LocalY = 8;

        }


        private void ctbTheMap_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Save the player position
            int gridX = Game.Player.PlayerCharacter.Root.GridX;
            int gridY = Game.Player.PlayerCharacter.Root.GridY;
            int localX = Game.Player.PlayerCharacter.Root.LocalX;
            int localY = Game.Player.PlayerCharacter.Root.LocalY;

            // Save copy of data
            int oldGridX = gridX;
            int oldGridY = gridY;
            int oldLocalX = localX;
            int oldLocalY = localY;

            // Movement calculation
            int deltaX = 0, deltaY = 0;
            switch (e.ToString())
            {
                case "w":
                    deltaY = -1;
                    break;
                case "s":
                    deltaY = 1;
                    break;
                case "d":
                    deltaX = 1;
                    break;
                case "a":
                    deltaX = -1;
                    break;
            }

            GridObject currentGrid = (GridObject)MapManager.Query(predicate: a => a.GridX == gridX && a.GridY == gridY);
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

            GridObject newGrid = (GridObject)MapManager.Query(predicate: a => a.GridX == newGridX && a.GridY == newGridY);
            if (currentGrid == newGrid)
            {
                TileObject currentTile = (TileObject)MapManager.Query((TileObject a) => a.Root.GridX == gridX && a.Root.GridY == gridY && a.Root.LocalX == localX && a.Root.LocalY == localY);
                ctbTheMap.Select();

            }
        }
    }
}
