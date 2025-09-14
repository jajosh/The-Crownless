using System;
/// <summary>
/// The Frame that ties all of the engines together. 
/// </summary>
public class EngineGame
{
    Console.SetBufferSize(Console.WindowWidth, Console.WindowHeight);
    int lastWidth { get; set; }
    int lastHeight { get; set; }
    //Gets the initial screen height
    int bottomRow { get; set; }
    int bottomLine { get; set; }
    public EngineGame()
	{
        
    }
    public void Ignition()
    {
        #region === Initialization Data ===
        Console.SetBufferSize(Console.WindowWidth, Console.WindowHeight);
        // Allows the console to print unicode
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        Map gameMap = Map.LoadMapFromJson();
        #endregion
    }
    public void update()
    {
        Console.SetBufferSize(Console.WindowWidth, Console.WindowHeight);
        int lastWidth = Console.WindowWidth;
        int lastHeight = Console.WindowHeight;
        //Gets the initial screen height
        int bottomRow = lastHeight - 1; // last valid row index
        int bottomLine = lastHeight - 4; // bottom line of the UI
    }
}
