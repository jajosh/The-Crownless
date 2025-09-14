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
    public static readonly string ItemFilePath = Path.Combine(DataFolder, "items.json");
    public static readonly string TheGridFilePath = Path.Combine(DataFolder, "grid.json");
    public static readonly string TheTriggerCoordinetsPath = Path.Combine(DataFolder, "TriggerCoordinets.json");
    public static readonly string TheNPCFilePath = Path.Combine(DataFolder, "NPCs.json");
    public static readonly string ConfigFilePath = Path.Combine(ConfigFolder, "config.json");
    public static readonly string EventsFilePath = Path.Combine(DataFolder, "Events.json");
    public static readonly string QuestFilePath = Path.Combine(DataFolder, "Quests.json");
    public static readonly string TheRandiomEnvironmentalDialog = Path.Combine(DataFolder, "RandiomEnvironmentalDialog.json");
    #endregion
    public FileManager()
	{

        EnsureDataFilesExist();
        VerifyPaths();
    }
    /// <summary>
    /// Returns the path to the save folder under Documents/MyGame, creating it if missing.
    /// </summary>
    public static string GetSavePath()
    {
        string folder = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
            "The Vrownless"
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

        string[] files = { ItemFilePath, ConfigFilePath, EventsFilePath, TheGridFilePath };
        foreach (var file in files)
        {
            if (!File.Exists(file))
                Console.WriteLine($"[WARNING] File does not exist: {file}");
        }
    }
    
    public static void PrintSaveFiles()
    {
        foreach (var folder in SavesFolder)
        {
            Console.WriteLine($"{folder}");
        }

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
public static class JsonLoader
{
    private static readonly JsonSerializerOptions Options = new()
    {
        Converters = { new JsonStringEnumConverter(JsonNamingPolicy.CamelCase) },
        PropertyNameCaseInsensitive = true,
        WriteIndented = true
    };
    public static T LoadFromJson<T>(string filePath)
    {
        string path = FileManager.ItemFilePath;
        if (!File.Exists(path))
        {
            Console.WriteLine($"File not found");
        }
        else if (File.Exists(path))
        {
            Console.WriteLine("Json found!");
        }
        var options = new JsonSerializerOptions
        {
            Converters = { new JsonStringEnumConverter(JsonNamingPolicy.CamelCase) },
            PropertyNameCaseInsensitive = true
        };

        string json = File.ReadAllText(filePath);
        return JsonSerializer.Deserialize<T>(json, options);
    }
    /// <summary>
    /// Saves an object to json
    /// </summary>
    /// <typeparam name="T"> This is a placeholder for the actually object</typeparam>
    /// <param name="filePath"> Path to the save location </param>
    /// <param name="obj"> object to save</param>
    public static void SaveToJson<T>(string filePath, T obj)
    {
        var options = new JsonSerializerOptions
        {
            WriteIndented = true,
            ReferenceHandler = ReferenceHandler.Preserve // Handles circular references if any
        };
        options.Converters.Add(new JsonStringEnumConverter());// 👈 this lets enum keys serialize as strings
        string json = JsonSerializer.Serialize(obj, options);
        File.WriteAllText(filePath, json);
    }
}