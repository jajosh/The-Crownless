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
    // References to engine components
    public EngineMovement Mover = new EngineMovement();
    public EngineGUI UI = new EngineGUI();
    public EngineText Text = new EngineText();
    public FileManager FileManager = new FileManager();
    public EngineAI AI = new EngineAI();
    public DataNPC NPC = new DataNPC();

    public List<IActionable> ActiveOnScreen { get; set; }
    public List<IActionable> ActiveOnLocation { get; set; }
    public EngineGame()
    {

    }
    // Initializes everything
    public void Assembly()
    {
        #region === Initialization Data ===
        Console.SetBufferSize(Console.WindowWidth, Console.WindowHeight);
        
        // Allows the console to print unicode
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        #endregion
    }
    public void update()
    {
        // Updates the different engines
    }
}
