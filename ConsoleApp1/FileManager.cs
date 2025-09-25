using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using Windows.Storage.Search;

public class FileManager
{
    public static readonly string BaseFolder = AppContext.BaseDirectory;
    #region --- Path to data, config, and saves folder ---
    public static readonly string DataFolder = Path.Combine(BaseFolder, "Data");
    public static readonly string ConfigFolder = Path.Combine(BaseFolder, "Config");
    public static readonly string SavesFolder = GetSavePath();
    #endregion

    // JSON file paths (a.k.a. the spaghetti junction of file references)
    #region --- Sets the paths specific to the json ---
    public static readonly string ItemFilePath = Path.Combine(DataFolder, "Items.json");
    public static readonly string TheGridFilePath = Path.Combine(DataFolder, "grid.json");
    public static readonly string TheTriggerCoordinetsPath = Path.Combine(DataFolder, "TriggerCoordinets.json");
    public static readonly string TheNPCFilePath = Path.Combine(DataFolder, "NPCs.json");
    public static readonly string TheNPCTypeFilePath = Path.Combine(DataFolder, "NPCTypes.json");
    public static readonly string ConfigFilePath = Path.Combine(ConfigFolder, "config.json");
    public static readonly string EventsFilePath = Path.Combine(DataFolder, "Events.json");
    public static readonly string QuestFilePath = Path.Combine(DataFolder, "Quests.json");
    public static readonly string TheRandomTextPath = Path.Combine(DataFolder, "RandomText.json");
    public static readonly string TheRandiomEnvironmentalDialog = Path.Combine(DataFolder, "RandiomEnvironmentalDialog.json");
    #endregion
    public static List<Item> Items { get; set; }
    public static List<NPCType> NPCTypes { get;  set; } = new();
    public static List<Quest> Quests { get;  set; } = new();
    public static List<NPC> NPCs { get;  set; } = new();
    public static List<Grid> Grids { get; set; } = new();
    public static List<EngineRandomText> RandomTexts { get; private set; } = new();
    public FileManager()
    {
        
        EnsureDataFilesExist();
        VerifyPaths();
        LoadData();
        Console.WriteLine($"Loaded {FileManager.Items?.Count ?? 0} items.");
        Console.WriteLine($"Loaded {FileManager.Grids?.Count ?? 0} Grids.");
        Console.WriteLine($"Loaded {FileManager.NPCs?.Count ?? 0} NPCs.");
        Console.WriteLine($"Loaded {FileManager.NPCTypes?.Count ?? 0} NPCTypes.");
    }
    /// <summary>
    /// Returns the path to the save folder under Documents/MyGame, creating it if missing.
    /// </summary>
    public static string GetSavePath()
    {
        string folder = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
            "The Crownless"
        );
        if (folder == null)
        {
            Directory.CreateDirectory(folder);
        }
        return Path.Combine(folder);
    }
    /// <summary>
    /// Verifies that expected folders and files exist. 
    /// Warns in the console if they don’t (no exceptions thrown, because YOLO).
    /// </summary>
    public static void VerifyPaths()
    {
        string[] folders = { DataFolder, ConfigFolder, SavesFolder };
        foreach (var folder in folders)
        {
            if (!Directory.Exists(folder))
                Console.WriteLine($"[WARNING] Folder does not exist: {folder}");
        }

        string[] files = { ItemFilePath, ConfigFilePath, EventsFilePath, TheGridFilePath, TheTriggerCoordinetsPath, TheNPCFilePath, TheNPCTypeFilePath, ConfigFilePath, EventsFilePath, QuestFilePath, TheRandomTextPath, TheRandiomEnvironmentalDialog };
        foreach (var file in files)
        {
            if (File.Exists(file))
                Console.WriteLine($"[Success] File does exist: {file}");
        }
    }

    public static void PrintSaveFiles()
    {
        foreach (var folder in SavesFolder)
        {
            Console.WriteLine($"{folder}");
        }

    }
    public static void LoadData()
    {
        Items = JsonLoader.LoadFromJson<List<Item>>(FileManager.ItemFilePath);
        Console.WriteLine("Items");
        NPCTypes = JsonLoader.LoadFromJson<List<NPCType>>(FileManager.TheNPCTypeFilePath);
        Console.WriteLine("NPCTypes");
        Quests = JsonLoader.LoadFromJson<List<Quest>>(FileManager.QuestFilePath);
        Console.WriteLine("Quests");
        NPCs = JsonLoader.LoadFromJson<List<NPC>>(FileManager.TheNPCFilePath);
        Console.WriteLine("NPCs");
        Grids = JsonLoader.LoadFromJson<List<Grid>>(FileManager.TheGridFilePath);
        Console.WriteLine("Grids");
        RandomTexts = JsonLoader.LoadFromJson<List<EngineRandomText>>(FileManager.TheRandomTextPath);
        Console.WriteLine("RandomTexts");
    }
    /// <summary>
    /// Ensures that all required data and config files exist in the output folders.
    /// If they’re missing, copies them from the project root.
    /// </summary>
    public static void EnsureDataFilesExist()
    {
        Directory.CreateDirectory(DataFolder);
        Directory.CreateDirectory(ConfigFolder);

        CopyIfMissing("Data/items.json", ItemFilePath);
        CopyIfMissing("Data/grid.json", TheGridFilePath);
        CopyIfMissing("Data/NPCs.json", TheNPCFilePath);
        CopyIfMissing("Data/TriggerCoordinets.json", TheTriggerCoordinetsPath);
        CopyIfMissing("Data/Events.json", EventsFilePath);
        CopyIfMissing("Config/config.json", ConfigFilePath);
        CopyIfMissing("Data/NPCTypes.json", TheNPCTypeFilePath);
        CopyIfMissing("Data/Quests.json", QuestFilePath);
        CopyIfMissing("Data/RandomText.json", TheRandomTextPath);
    }
    /// <summary>
    /// Copies a file from the project directory into the output folder if it’s missing.
    /// Also overwrites existing files without asking, because why not.
    /// </summary>
    private static void CopyIfMissing(string relativePathFromProject, string targetPath)
    {
        string projectRoot = Path.GetFullPath(Path.Combine(BaseFolder, "..", "..", ".."));
        string sourcePath = Path.Combine(projectRoot, relativePathFromProject);

        if (!File.Exists(targetPath) && File.Exists(sourcePath))
        {
            File.Copy(sourcePath, targetPath);
            Console.WriteLine($"Copied {relativePathFromProject} to output folder.");
        }
        if (File.Exists(targetPath))
        {

            File.Copy(sourcePath, targetPath, overwrite: true);
        }
    }
}