using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public static class FilePaths
{
    public static readonly string BaseFolder = AppContext.BaseDirectory;

    #region Core Folders
    public static readonly string DataFolder = Path.Combine(BaseFolder, "Data");
    public static readonly string ConfigFolder = Path.Combine(BaseFolder, "Config");
    public static readonly string SavesFolder = FileManager.GetSavePath();  // Computed
    #endregion

    #region JSON Paths
    public static readonly string TriggerCoordinatesPath = Path.Combine(DataFolder, "TriggerCoordinates.json");
    // === Not refactored yet ===
    public static readonly string ItemFilePath = Path.Combine(DataFolder, "Items.json");
    public static readonly string GridFilePath = Path.Combine(DataFolder, "grid.json");
    public static readonly string NPCFilePath = Path.Combine(DataFolder, "NPCs.json");
    public static readonly string NPCTypeFilePath = Path.Combine(DataFolder, "NPCTypes.json");
    public static readonly string ConfigFilePath = Path.Combine(ConfigFolder, "config.json");
    public static readonly string EventsFilePath = Path.Combine(DataFolder, "Events.json");
    public static readonly string QuestFilePath = Path.Combine(DataFolder, "Quests.json");
    public static readonly string RandomTextPath = Path.Combine(DataFolder, "RandomText.json");  // Fixed typo
    public static readonly string RandomEnvironmentalDialogPath = Path.Combine(DataFolder, "RandomEnvironmentalDialog.json");  // Fixed typo
    #endregion
}
