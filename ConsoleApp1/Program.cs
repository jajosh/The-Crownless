using Microsoft.VisualBasic;
using Newtonsoft;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Metrics;
using System.Globalization;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using Windows.ApplicationModel.DataTransfer;
using Windows.Media.Capture;
using Windows.Media.Devices;
using Windows.Perception.Spatial;
using static System.Net.Mime.MediaTypeNames;
using Spectre.Console;

namespace GameNamespace
{
    /// <summary>
    /// The main entry point for the game. 
    /// Handles initialization, menu navigation, the main game loop, 
    /// rendering, and environmental updates such as weather and seasons.
    /// </summary>
    public class MainClass
    {
        
        public static SaveGame saveGame { get; set; }
        // public bools that can be accessed outside of the main class. 
        public static bool needRedraw = true;
        public static bool isRunning = true;

        public static void Main(string[] args)
        {
            AppDomain.CurrentDomain.UnhandledException += (sender, e) =>
            {
                using (StreamWriter writer = new StreamWriter("unhandled_errors.txt", append: true))
                {
                    writer.WriteLine($"Unhandled Exception: {e.ExceptionObject}");
                }
            };

            EngineGame GameEngine = new EngineGame();
            Menu menu = new Menu();


            saveGame = Menu.StartMenu(saveGame);

            #region === Initialization Data ===
            Console.SetBufferSize(Console.WindowWidth, Console.WindowHeight);
            int lastWidth = Console.WindowWidth;
            int lastHeight = Console.WindowHeight;
            //Gets the initial screen height
            int bottomRow = lastHeight - 1; // last valid row index
            int bottomLine = lastHeight - 4; // bottom line of the UI
            // Allows the console to print unicode
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            #endregion
            Console.Clear();
            Console.WriteLine("\x1b[3J");

            EngineGUI.DrawLayout(saveGame.PlayerCharacter);
            EngineGUI.UpdateStats(saveGame.PlayerCharacter);
            Map.PrintWorld(saveGame.GameMap, saveGame.PlayerCharacter, 0);
            while (isRunning)
            {
                var stopwatch = new Stopwatch();
                stopwatch.Start();

                // The timed game loop, handles movement and trigger actions
                while (stopwatch.Elapsed < TimeSpan.FromMilliseconds(5))
                {
                    if (needRedraw)
                    {
                        EngineGUI.Render(saveGame);
                        needRedraw = false;
                    }
                    else
                    {
                        EngineGUI.DrawLayout(saveGame.PlayerCharacter);
                    }
                    GameEngine.Mover.Move(saveGame);
                    Console.SetCursorPosition(30, 1);
                    NPC npc = DataNPC.GetNamedNPCByID(1);
                    //GameEngine.AI.ExecuteTurn(npc, saveGame.PlayerCharacter, saveGame);
                    EngineGUI.WriteMessageToPlayer($"{npc.Root.LocalX}, {npc.Root.LocalY}");
                }

            }
            #region === Exit code === 
            Console.SetCursorPosition(3, bottomRow);  // column 0, bottom row
            Console.WriteLine("\nThanks for playing!");

            JsonLoader.SaveToJson<SaveGame>(FileManager.SavesFolder + saveGame.PlayerCharacter.Name, saveGame);
            #endregion

        }



    }
}