using GameNamespace;
using System;
using System.Linq.Expressions;
using System.Security.Cryptography.X509Certificates;
using System.Xml.Linq;
using Windows.Security.EnterpriseData;
using Windows.Storage;
using static System.Net.Mime.MediaTypeNames;

public class Menu
{
    public TextHandler text = new TextHandler();
    public Menu()
    {
        
    }

    public static SaveGame StartMenu()
    {
        SaveGame saveGame = new SaveGame();
        GUIEngine UI = new GUIEngine();
        Menu menu = new Menu();
        TextHandler text = new TextHandler();
        Dictionary<string, Delegate> triggerActions = new(StringComparer.OrdinalIgnoreCase)
            {
                { "new game",  new Func<SaveGame>(ScriptedEvents.StartNewGame) },
                { "load game", new Func<SaveGame>(menu.OnLoadGame) },
                { "settings",  new Action(menu.OnSettings) },
                { "quit",      new Action(menu.OnQuit) },
                { "debug file",     new Func<SaveGame>(Player.debugLoadGame)}
            };
        bool firstCheck = true;
        while (firstCheck)
        {
            Console.WriteLine("Hello! and thank you for playing my game!\n\n\nNew Game\nLoad Game\nSettings\nQuit");
            string firstResponse = Console.Read().ToString();
            if (firstResponse == "new game")
            {
                // Run "new game" -> returns Player
                var newGameFunc = (Func<SaveGame>)triggerActions["new game"];
                saveGame = ScriptedEvents.StartNewGame();
                saveGame.triggers = JsonLoader.LoadFromJson<List<TriggerCoordinets>>(FileManager.TheTriggerCoordinetsPath);
                firstCheck = false;
            }
            else if (firstResponse == "load game")
            {
                // Run "load game" -> returns (Player, TriggerCoordinets)
                saveGame = menu.OnLoadGame();

                firstCheck = false;
            }
            else if (firstResponse == "settings")
            {
                // Run "settings" -> void
                var settingsAction = (Action)triggerActions["settings"];
                settingsAction();
                firstCheck = true;
            }
            else if (firstResponse == "quit")
            {
                // Run "quit" -> void
                ((Action)triggerActions["quit"])();
                firstCheck = false;
            }
            else if (firstResponse == "debug file")
            {
                saveGame = Player.debugLoadGame();

            }
            else
            {
                Console.WriteLine("Error: Null or invalid response");
                Task.Run(async () =>
                {
                    text.Write("This is my wielder?");
                    if (Console.KeyAvailable)
                    {
                        // Clear the key press so it doesn't interfere with later input
                        Console.ReadKey(true);

                        // Print the rest of the text immediately
                    }

                    await Task.Delay(1000);
                    Console.Clear();
                });
            }
        }
        return saveGame;

    }
    private SaveGame OnLoadGame()
    {

        SaveGame saveGame = new SaveGame();
        bool checker = true;
        while (checker) {
            foreach (var folder in FileManager.SavesFolder) {
                Console.WriteLine($"{folder}");
            }
            string name = text.Read("Which save would you like to load?");
            try
            {
                saveGame = JsonLoader.LoadFromJson<SaveGame>(FileManager.SavesFolder + name);
                checker = false;
            }
            catch
            {
                throw new Exception("Save file does not exist...\nDid you miss spell the name?");
            }
        }
        return saveGame;
    }
    public void OnSettings()
    {
        text.Write("Settings...\nPress ENTER to continue", 100);
    }
    private void OnQuit()
    {
        text.Write("Quitting game...\nPress ENTER to continue", 100);
        MainClass.isRunning = false;
    }
    

}
