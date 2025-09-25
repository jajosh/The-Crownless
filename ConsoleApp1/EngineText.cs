using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public class EngineText
{
    public static StatusFlags flags = new StatusFlags();
    private readonly Dictionary<string, Delegate> triggerActions;

    public EngineText()
    {
        triggerActions = new Dictionary<string, Delegate>(StringComparer.OrdinalIgnoreCase)
        {
            { "save game", new Action(() => OnSaveToJson(new SaveGame())) }
        };
    }

    public void OnSaveToJson(SaveGame saveGame)
    {
        JsonLoader.SaveToJson(
            FileManager.SavesFolder + saveGame.PlayerCharacter.Name, saveGame
        );
    }

    public string HandleInput(string input, int time)
    {
        string response = Read(input, time);
        string cleanedInput = response.Trim().ToLower();

        foreach (var trigger in triggerActions)
        {
            if (cleanedInput.Contains(trigger.Key))
            {
                trigger.Value.DynamicInvoke();  // FIX: Don’t pass trigger.Key
                return cleanedInput;            // FIX: return instead of recursing forever
            }
        }

        return response;
    }

    public string HandleInput(string input)
    {
        string response = Read(input);
        string cleanedInput = response.Trim().ToLower();

        foreach (var trigger in triggerActions)
        {
            if (cleanedInput.Contains(trigger.Key))
            {
                trigger.Value.DynamicInvoke();
                return cleanedInput;
            }
        }

        return response;
    }

    public void Write(string text, int time)
    {
        Task.Run(async () =>
        {
            for (int i = 0; i < text.Length; i++)
            {
                Console.Write(text[i]);

                if (Console.KeyAvailable)
                {
                    Console.ReadKey(true); // clear key press
                    Console.Write(text.Substring(i + 1));
                    break;
                }

                await Task.Delay(time);
            }
        }).GetAwaiter().GetResult();
    }

    public void Write(string text)
    {
        Write(text, flags.ConsoleSpeed);
    }

    public void Write(int row, string text)
    {
        Console.SetCursorPosition(row, 0);
        Write(text);
    }

    public string Read(string text, int time)
    {
        Write(text, time);
        // FIX: Console.ReadLine instead of Console.Read
        string response = Console.ReadLine();
        return response ?? string.Empty;
    }

    public string Read(string text)
    {
        return Read(text, flags.ConsoleSpeed);
    }
}
