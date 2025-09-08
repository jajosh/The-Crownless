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
        static GUIEngine UI = new GUIEngine();
        // The Game loop
        public static bool isRunning = true;

        public static SaveGame saveGame = new SaveGame();

        public Player player = new Player();
        public List<TriggerCoordinets> triggers = new List<TriggerCoordinets>();
        public WeatherEngine weather = new();
        public StatusFlags flags = new StatusFlags();
        public List<Quest> completedQuests = new List<Quest>();
        public List<Quest> activeQuests = new List<Quest>();
        public List<Quest> quests = new List<Quest>();
        public List<Item> items = new List<Item>();

        /// <summary>
        /// Application entry point. Initializes the game state, 
        /// displays the main menu, and starts the game loop.
        /// </summary>
        /// <param name="args">Command-line arguments (not currently used).</param>
        public static void Main(string[] args)
        {
            /// --- Shit that gets saved to the save json
            Player player = new Player();
            List<TriggerCoordinets> triggers = new List<TriggerCoordinets>();
            WeatherEngine weather = new();
            StatusFlags flags = new StatusFlags();
            List<Quest> completedQuests = new List<Quest>();
            List<Quest> activeQuests = new List<Quest>();
            List<Quest> quests = new List<Quest>();
            List<Item> items = JsonLoader.LoadFromJson<List<Item>>(FileManager.ItemFilePath);


            #region === Initialization Data ===
            Console.SetBufferSize(Console.WindowWidth, Console.WindowHeight);
            int lastWidth = Console.WindowWidth;
            int lastHeight = Console.WindowHeight;
            //Gets the initial screen height
            int bottomRow = lastHeight - 1; // last valid row index
            int bottomLine = lastHeight - 4; // bottom line of the UI
            // Allows the console to print unicode
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            Map gameMap = Map.LoadMapFromJson();
            List<TriggerCoordinets> triggerCoordinets = new List<TriggerCoordinets>();


            Item item = new Item();
            FileManager filemanager = new FileManager();
            #endregion
            Map dataSetMap = new();
            Player dataSetPlayer = new();


            //  A check if the game needs a redraw
            bool needRedraw = true;

            
            while (isRunning)
            {
                var stopwatch = new Stopwatch();
                stopwatch.Start();

                // The timed game loop, handles movement and trigger actions
                while (stopwatch.Elapsed < TimeSpan.FromMilliseconds(5))
                {
                    if (needRedraw)
                    {                     
                        GUIEngine.UpdataStats(player);
                        Map.PrintWorld(gameMap, player, 0);
                        GUIEngine.DrawLayout(player);
                        needRedraw = false;
                    }
                    int consoleHeightChange = Console.WindowHeight - lastHeight;
                    //Writes the players location
                    GUIEngine.WriteInPanel(8, 0, $"{player.Gridx}, {player.GridY}, {player.LocalX}, {player.LocalY}");
                    Console.SetCursorPosition(3, bottomRow);

                    ConsoleKeyInfo keyInfo = Console.ReadKey(true);

                    int GridX = player.Gridx;
                    int GridY = player.GridY;
                    int LocalX = player.LocalX;
                    int LocalY = player.LocalY;

                    // Decide movement deltas
                    if (keyInfo.Key == ConsoleKey.W) LocalX++;
                    else if (keyInfo.Key == ConsoleKey.S) LocalX--;
                    else if (keyInfo.Key == ConsoleKey.A) LocalY--;
                    else if (keyInfo.Key == ConsoleKey.D) LocalY++;
                    else if (keyInfo.Key == ConsoleKey.Escape)
                    {
                        isRunning = false;
                        break;
                    }

                    // Wrap or adjust bounds BEFORE tile lookup
                    if (LocalX > 24)
                    {
                        LocalX = 0; GridX++;
                        needRedraw = true;
                        Grid currentGrid = Map.FindGrid(gameMap, GridX, GridY);
                        Grid.NewGridDescription(currentGrid, weather);
                    }
                    if (LocalX < 0)
                    {
                        LocalX = 24; GridX--;
                        needRedraw = true;
                        Grid currentGrid = Map.FindGrid(gameMap, GridX, GridY);
                        Grid.NewGridDescription(currentGrid, weather);
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


                        if (random.Next(10)<2)
                        {
                            Grid currentGrid = Map.FindGrid(gameMap, GridX, GridY);
                            string text = Tile.PickADescription(tileToMoveTo, weather, currentGrid.Biome, currentGrid.SubBiome);
                            GUIEngine.WriteWithClearAnimation(0, 27, text);

                            Console.SetWindowPosition(0, 0);
                        }
                    }
                    // Checks if a tile has a trigger

                    else
                    {
                        consoleHeightChange = Console.WindowHeight - lastHeight;
                        Console.SetCursorPosition(3, bottomLine);  // column 0, bottom row
                        GUIEngine.WriteInPanel(2,26,"Tile not Walkable");
                        System.Threading.Thread.Sleep(750);
                        needRedraw = true;

                    }
                    Console.SetCursorPosition(3, bottomRow - consoleHeightChange);
                    //Map.PrintWorld(gameMap, player, 0);
                }

                // Tracks the days of the game and handles the weather and seasons. As of now this is purely fluff
                

            }
            #region === Exit code === 
            Console.SetCursorPosition(3, bottomRow);  // column 0, bottom row
            Console.WriteLine("\nThanks for playing!");

            TriggerCoordinets.SaveToJson("triggers.json", triggerCoordinets);
            #endregion
        }
        
        /// <summary>
        /// Randomly updates the weather based on the current season.
        /// </summary>
        /// <param name="WeatherCountDown">Remaining turns before weather changes.</param>
        /// <param name="currentWeather">The current weather condition.</param>
        /// <param name="Seasons">The current season.</param>
        /// <returns>A tuple containing the new countdown and new weather.</returns>
        
        
    }

    
   
}
