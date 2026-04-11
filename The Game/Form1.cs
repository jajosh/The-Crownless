using System.Security.Cryptography.Xml;
using System.ComponentModel;
using System.Linq;

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
			BugHunter.Log(DebugType.GENERICPROCESSING, "KeyPress Event Found...", DebugLogSeverity.telemetry);
			switch(e.KeyChar)
			{
				case 'w':
				case 'a':
				case 's':
				case 'd':
					MovementManager.Move(ctbTheMap, e.KeyChar);
					break;
				case 'm':
					List<TileObject> TestList = new List<TileObject>();
					TestList.Add( TileRepository.Query(new { GridX = Game.Player.PlayerCharacter.Root.GridX, GridY = Game.Player.PlayerCharacter.Root.GridY, LocalX = Game.Player.PlayerCharacter.Root.LocalX +1, LocalY = Game.Player.PlayerCharacter.Root.LocalY }));
                    TestList.Add(TileRepository.Query(new { GridX = Game.Player.PlayerCharacter.Root.GridX, GridY = Game.Player.PlayerCharacter.Root.GridY, LocalX = Game.Player.PlayerCharacter.Root.LocalX - 1, LocalY = Game.Player.PlayerCharacter.Root.LocalY }));
                    TestList.Add(TileRepository.Query(new { GridX = Game.Player.PlayerCharacter.Root.GridX, GridY = Game.Player.PlayerCharacter.Root.GridY, LocalX = Game.Player.PlayerCharacter.Root.LocalX, LocalY = Game.Player.PlayerCharacter.Root.LocalY +1 }));
                    TestList.Add(TileRepository.Query(new { GridX = Game.Player.PlayerCharacter.Root.GridX, GridY = Game.Player.PlayerCharacter.Root.GridY, LocalX = Game.Player.PlayerCharacter.Root.LocalX, LocalY = Game.Player.PlayerCharacter.Root.LocalY -1}));
					foreach (TileObject tile in TestList)
					{
						int x = 0;
						foreach (TileComponents component in tile.Components)
						{
							x++;
                            BugHunter.Log(DebugType.GENERICPROCESSING, $"Component Type: {component.TileComponent?.GetType().Name}", DebugLogSeverity.telemetry);
                        }
						BugHunter.Log(DebugType.GENERICPROCESSING, $"tile found at {tile.Root.LocalX}, {tile.Root.LocalY} with {x} number of components", DebugLogSeverity.telemetry);
                        
                    }
					BugHunter.Log(DebugType.MOVEMENT, $"");
                    break;
			}
		}

		private void pnlMessageBoard_Paint(object sender, PaintEventArgs e)
		{

		}
	}
}
