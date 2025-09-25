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

namespace GameNamespace
{
    /// <summary>
    /// The main entry point for the game. 
    /// Handles initialization, menu navigation, the main game loop, 
    /// rendering, and environmental updates such as weather and seasons.
    /// </summary>
    public class MainClass
    {
        public static FileManager fileManager = new FileManager();
        public static SaveGame saveGame { get; set; }
        // public bools that can be accessed outside of the main class. 
        public static bool needRedraw = true;
        public static bool isRunning = true;

        /// <summary>
        /// Application entry point. Initializes the game state, 
        /// displays the main menu, and starts the game loop.
        /// </summary>
        /// <param name="args">Command-line arguments (not currently used).</param>
        public static void Main(string[] args)
        {
            /// <summary>
            /// The following is the car of the game. All of the engines are initalized here.
            /// </summary>
            EngineGUI UIEngine = new EngineGUI();
            EngineGame GameEngine = new EngineGame();
            Menu menu = new Menu();


            saveGame = Menu.StartMenu(saveGame);
            /// --- Shit that gets saved to the save json




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
            EngineGUI.DrawLayout(saveGame.PlayerCharacter);
            EngineGUI.UpdataStats(saveGame.PlayerCharacter);
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
                        EngineGUI.DrawLayout(saveGame.PlayerCharacter);
                        EngineGUI.UpdataStats(saveGame.PlayerCharacter);
                        Map.PrintWorld(saveGame.GameMap, saveGame.PlayerCharacter, 0);
                        needRedraw = false;
                    }
                    GameEngine.Mover.Move(saveGame);
                    


                }
                
            }
            #region === Exit code === 
            Console.SetCursorPosition(3, bottomRow);  // column 0, bottom row
            Console.WriteLine("\nThanks for playing!");

            TriggerCoordinets.SaveToJson("triggers.json", saveGame.Triggers);
            #endregion

        }



    }
}