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
/// <summary>
/// The Frame that ties all of the engines together. 
/// </summary>
public class EngineGame
{
    public EngineMovement Mover = new EngineMovement();
    public EngineGame()
    {

    }
    public void Assembly()
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
