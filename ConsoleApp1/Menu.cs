using GameNamespace;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public class Menu
{
    public EngineText text;

    public Menu()
    {

    }

    public static SaveGame StartMenu(SaveGame saveGame)
    {
        EngineGUI UI = new EngineGUI();
        Menu menu = new Menu();
        EngineText text = new EngineText();

        Dictionary<string, Delegate> triggerActions = new(StringComparer.OrdinalIgnoreCase)
        {
            { "new game",  new Func<SaveGame>(ScriptedEvents.StartNewGame) },
            { "load game", new Func<SaveGame>(menu.OnLoadGame) },
            { "settings",  new Action(menu.OnSettings) },
            { "quit",      new Action(menu.OnQuit) },
            { "debug file", new Func<SaveGame>(Player.debugLoadGame) }
        };

        bool firstCheck = true;

        while (firstCheck)
        {
            Console.WriteLine("Hello! and thank you for playing my game!\n\n\nNew Game\nLoad Game\nSettings\nQuit");

            // FIX: Console.Read() only grabs one character. Use ReadLine instead.
            string firstResponse = Console.ReadLine()?.Trim().ToLower();

            if (firstResponse == "new game")
            {
                saveGame = ScriptedEvents.StartNewGame();
                saveGame.Triggers = JsonLoader.LoadFromJson<List<TriggerCoordinets>>(FileManager.TheTriggerCoordinetsPath);
                firstCheck = false;
            }
            else if (firstResponse == "load game")
            {
                saveGame = menu.OnLoadGame();
                firstCheck = false;
            }
            else if (firstResponse == "settings")
            {
                var settingsAction = (Action)triggerActions["settings"];
                settingsAction();
                firstCheck = true;
            }
            else if (firstResponse == "quit")
            {
                ((Action)triggerActions["quit"])();
                firstCheck = false;
            }
            else if (firstResponse == "debug file")
            {
                saveGame = Player.debugLoadGame();
                firstCheck = false;
            }
            else
            {
                Console.WriteLine("Error: Null or invalid response");
                Task.Run(async () =>
                {
                    text.Write("This is my wielder?");
                    if (Console.KeyAvailable)
                    {
                        Console.ReadKey(true); // Clear any stray key presses
                    }
                    await Task.Delay(1000);
                    Console.Clear();
                }).GetAwaiter().GetResult();
            }
        }

        return saveGame;
    }

    private SaveGame OnLoadGame()
    {
        SaveGame saveGame = new SaveGame();
        bool checker = true;

        while (checker)
        {
            foreach (var folder in FileManager.SavesFolder)
            {
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
                Console.WriteLine("Save file does not exist...\nDid you misspell the name?");
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
