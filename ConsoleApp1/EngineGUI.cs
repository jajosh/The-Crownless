using GameNamespace;
using System;
using static System.Net.Mime.MediaTypeNames;

/// <summary>
///  This engine handles the creating the GUI. Methods must be changed to fit any changes that are desired in the gui. 
/// </summary>
public class EngineGUI
{
    public int lastWidth { get; set; }
    public int lastHeight { get; set; }
    //Gets the initial screen height
    public int bottomRow { get; set; }
    public int bottomLine { get; set; }
    StatusFlags flags = new StatusFlags();
    public EngineGUI()
    {

    }
    public static void Render(SaveGame saveGame)
    {
        EngineGUI.DrawLayout(saveGame.PlayerCharacter);
        EngineGUI.UpdateStats(saveGame.PlayerCharacter);
        Map.PrintWorld(saveGame.GameMap, saveGame.PlayerCharacter, 0);
    }
    /// <summary>
    /// Draws out the boxes of the UI
    /// </summary>
    /// <param name="x"> starting x position of the box </param>
    /// <param name="y"> starting y position of the box </param>
    private static void DrawBox(int x, int y, int width, int height, string title = "")
    {
        // Set cursor position to the starting point
        Console.SetCursorPosition(x, y);
        Console.Write("+" + new string('-', width - 2) + "+");
        for (int i = 1; i < height - 1; i++)
        {
            Console.SetCursorPosition(x, y + i);
            Console.Write("|");
            Console.SetCursorPosition(x + width - 1, y + i);
            Console.Write("|");
        }
        Console.SetCursorPosition(x, y + height - 1);
        Console.Write("+" + new string('-', width - 2) + "+");
        if (!string.IsNullOrEmpty(title))
        {
            Console.SetCursorPosition(x + 2, y);
            Console.Write(title);
        }
    }
    /// <summary>
    /// Draws the overall game layout including map, stats, and inventory panels.
    /// </summary>
    /// <param name="player">The current player, used to label the stats panel.</param>
    public static void DrawLayout(Player player)
    {

        // Map (top-left)
        DrawBox(0, 0, 53, 27, $" MAP - Grid {player.Root.GridX}, {player.Root.GridY} - Local {player.Root.LocalX}, {player.Root.LocalY}");

        // Player Stats (top-right)
        DrawBox(52, 0, 31, 27, ($" {player.Name} "));

        // Inventory (bottom-right)
        DrawBox(82, 0, 31, 27, " INVENTORY ");

        for (int i = 3; i < Console.WindowHeight; i++)
        {
            Console.SetCursorPosition(114, i - 3);
            Console.Write('\n');
        }
    }
    // Existing WriteInPanel
    public static void WriteInPanel(int x, int y, string text)
    {
        Console.SetWindowPosition(0, 0);
        Console.SetCursorPosition(x, y);
        Console.Write(text);
        Console.SetCursorPosition(0, 27);
    }

    /// <summary>
    /// Writes text to the console, pauses briefly, then clears it with an animation.
    /// </summary>
    /// <param name="x">The x-coordinate for the text.</param>
    /// <param name="y">The y-coordinate for the text.</param>
    /// <param name="text">The text to display.</param>
    public static async Task WriteWithClearAnimation(int x, int y, string text)
    {
        WriteInPanel(x, y, text);

        // Pause before clearing
        await Task.Delay(1500);

        for (int i = 0; i <= text.Length; i++)
        {
            Console.SetWindowPosition(x, y);
            WriteInPanel(i, y, " ");
        }
    }
    public static async Task WriteMessageToPlayer(string text)
    {
        EngineGUI.WriteWithClearAnimation(0, 27, text);
    }
    /// <summary>
    /// Updates the player stats panel with health, money, skills, and position coordinets
    /// </summary>
    /// <param name="player">The player whose stats are displayed.</param>
    public static void UpdateStats(Player player)
    {
        //Writes the players location
        EngineGUI.WriteInPanel(8, 0, $" {player.Root.GridX}, {player.Root.GridY} - {player.Root.LocalX}, {player.Root.LocalY} ---");
        WriteInPanel(55, 1, $"HP: {player.Health.CurrentHP} / {player.Health.MaxHP}");
        WriteInPanel(55, 2, $"MP: {player.Health.CurrentMP} / {player.Health.MaxMP}");
        int x = 3;
        if (player.Money != null)
        {
            foreach (var value in player.Money)
            {
                WriteInPanel(55, x, $"{value.Key}: {value.Value}");
                x++;
            }
        }
        x = 6;
        if (player.Skills != null)
        {
            foreach (var value in player.Skills)
            {
                WriteInPanel(55, x, $"{value.Key}: {value.Value}");
                x++;
            }
        }

    }


    public int ConsoleHeightChange()
    {
        int consoleHeightChange = Console.WindowHeight - lastHeight;
        return consoleHeightChange;
    }

    public void HighlightEnemy()
    {

    }
    public void unhighlight()
    {

    }
}
