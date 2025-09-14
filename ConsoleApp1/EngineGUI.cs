using System;
using static System.Net.Mime.MediaTypeNames;

/// <summary>
///  This engine handles the creating the GUI. Methods must be changed to fit any changes that are desired in the gui. 
/// </summary>
public class EngineGUI
{
    StatusFlags flags = new StatusFlags();
	public EngineGUI()
	{

	}
    /// <summary>
    /// Draws a rectangular ASCII box at the specified location.
    /// </summary>
    /// <param name="x">The starting x-coordinate.</param>
    /// <param name="y">The starting y-coordinate.</param>
    /// <param name="width">The width of the box.</param>
    /// <param name="height">The height of the box.</param>
    /// <param name="title">Optional title text displayed at the top.</param>
    private static void DrawBox(int x, int y, int width, int height, string title = "")
    {
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
        DrawBox(0, 0, 53, 27, " MAP ");

        // Player Stats (top-right)
        DrawBox(54, 0, 31, 27, ($" {player.Name} "));

        // Inventory (bottom-right)
        DrawBox(81, 0, 31, 27, " INVENTORY ");

        for (int i = 3; i < Console.WindowHeight; i++)
        {
            Console.SetCursorPosition(114, i - 3);
            Console.Write('\n');
        }
    }
    /// <summary>
    /// Writes text to a specific panel position.
    /// </summary>
    /// <param name="x">The x-coordinate of the cursor.</param>
    /// <param name="y">The y-coordinate of the cursor.</param>
    /// <param name="text">The text to display.</param>
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
            Console.SetWindowPosition(0, 0);
            WriteInPanel(i, y, " ");
        }
    }
    /// <summary>
    /// Updates the player stats panel with health, money, skills, and position coordinets
    /// </summary>
    /// <param name="player">The player whose stats are displayed.</param>
    public static void UpdataStats(Player player)
    {
        //Writes the players location
        EngineGUI.WriteInPanel(8, 0, $"{player.Gridx}, {player.GridY}, {player.LocalX}, {player.LocalY}");
        WriteInPanel(56, 1, $"HP: {player.Health}");
        int x = 3;
        foreach (var value in player.Money)
        {
            string name = player.Money.ToString();
            WriteInPanel(56, x, $"{value}");
            x++;
        }
        x = 6;
        foreach (var value in player.Skills)
        {
            string name = player.Skills.ToString();
            WriteInPanel(56, x, $"{value}");
            x++;
        }

    }

    public int GetConsoleHeight()
    {

        int consoleHeightChange = Console.WindowHeight - lastHeight;
    }
}
