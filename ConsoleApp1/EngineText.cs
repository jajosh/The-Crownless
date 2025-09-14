using System;

public class EngineText
{
    public static StatusFlags flags = new StatusFlags();
    private readonly Menu menu;
    private readonly Dictionary<string, Delegate> triggerActions;

    public EngineText()
    {
        menu = new Menu();
        triggerActions = new Dictionary<string, Delegate>(StringComparer.OrdinalIgnoreCase)
        {
            { "settings", new Action(menu.OnSettings) },
            { "save game", new Action(() => OnSaveToJson(new SaveGame())) }
        };
    }

    public void OnSaveToJson(SaveGame saveGame)
    {
        JsonLoader.SaveToJson<SaveGame>(
            FileManager.SavesFolder + saveGame.player.Name, saveGame
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
                trigger.Value.DynamicInvoke(trigger.Key);
                return HandleInput(input, time);
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
                trigger.Value.DynamicInvoke(trigger.Key);
                return HandleInput(input);
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
                    // Clear the key press so it doesn't interfere with later input
                    Console.ReadKey(true);

                    // Print the rest of the text immediately
                    Console.Write(text.Substring(i + 1));
                    break;
                }

                await Task.Delay(time);
            }
        }).GetAwaiter().GetResult(); // <-- blocks the caller until the task completes
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
        string response = Console.Read().ToString();
        return response;
    }
    public string Read(string text)
    {
        string response = Read(text, flags.ConsoleSpeed);
        return response;

    }
}
