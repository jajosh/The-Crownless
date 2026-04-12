using NAudio.Gui;

namespace The_Game;


public class FileManager : IIFileEngine
{
    public FileManager()
    {
        EnsureDataFilesExist();
        VerifyPaths();
    }
    public static string GetSavePath()
    {
        string folder = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
            "The Crownless"
        );
        Directory.CreateDirectory(folder);  // Always create
        return folder;
    }

    public void VerifyPaths()
    {
        string[] folders = { FilePaths.DataFolder, FilePaths.ConfigFolder, FilePaths.SavesFolder };
        foreach (var folder in folders)
        {
            if (!Directory.Exists(folder))
                BugHunter.Log(DebugType.GAMEFILE, $"Folder missing: {folder}", DebugLogSeverity.WARN);
            else
                BugHunter.Log(DebugType.GAMEFILE, $"Folder found: {folder}", DebugLogSeverity.INFO);
        }

        string[] files = {
                FilePaths.ItemFilePath, FilePaths.ConfigFilePath, FilePaths.EventsFilePath,
                FilePaths.LocationsFilePath, FilePaths.TriggerCoordinatesPath, FilePaths.NPCFilePath,
                FilePaths.NPCTypeFilePath, FilePaths.QuestFilePath, FilePaths.RandomTextPath,
                FilePaths.RandomEnvironmentalDialogPath
            };
        foreach (var file in files)
        {
            if (File.Exists(file))
                BugHunter.Log(DebugType.GAMEFILE, $"Folder exists: {file}", DebugLogSeverity.INFO);
            else
                BugHunter.Log(DebugType.GAMEFILE, $"Folder missing: {file}" ,DebugLogSeverity.WARN);
        }
    }

    public void PrintSaveFiles()
    {
        if (!Directory.Exists(FilePaths.SavesFolder)) return;
        var saveFiles = Directory.GetFiles(FilePaths.SavesFolder, "*.json");  // Fixed: Enumerable files
        foreach (var file in saveFiles)
        {
            BugHunter.Log(DebugType.GAMEFILE, $"Save file: {Path.GetFileName(file)}", DebugLogSeverity.INFO);
        }
    }


    public void EnsureDataFilesExist()
    {
        Directory.CreateDirectory(FilePaths.DataFolder);
        Directory.CreateDirectory(FilePaths.ConfigFolder);

        CopyIfMissing("Data/items.json", FilePaths.ItemFilePath);
        CopyIfMissing("Data/Maps.json", FilePaths.LocationsFilePath);
        CopyIfMissing("Data/NPCs.json", FilePaths.NPCFilePath);
        CopyIfMissing("Data/TriggerCoordinates.json", FilePaths.TriggerCoordinatesPath);  // Fixed typo
        CopyIfMissing("Data/Events.json", FilePaths.EventsFilePath);
        CopyIfMissing("Config/config.json", FilePaths.ConfigFilePath);
        CopyIfMissing("Data/NPCTypes.json", FilePaths.NPCTypeFilePath);
        CopyIfMissing("Data/Quests.json", FilePaths.QuestFilePath);
        CopyIfMissing("Data/RandomText.json", FilePaths.RandomTextPath);  // Fixed typo
        CopyIfMissing("Data/RandomEnvironmentalDialog.json", FilePaths.RandomEnvironmentalDialogPath);  // Fixed typo
    }

    public void CopyIfMissing(string relativePathFromProject, string targetPath)
    {

        string projectRoot = Path.GetFullPath(Path.Combine(FilePaths.BaseFolder, "..", "..", ".."));
        string sourcePath = Path.Combine(projectRoot, relativePathFromProject);

        if (File.Exists(sourcePath))
        {
            if (!File.Exists(targetPath))
            {
                File.Copy(sourcePath, targetPath);
                BugHunter.Log(DebugType.GAMEFILE, $"File Copied: {relativePathFromProject} ? {targetPath}", DebugLogSeverity.INFO);
            }
            else
            {
                // Optional: Uncomment for always-overwrite
                // File.Copy(sourcePath, targetPath, overwrite: true);
                BugHunter.Log(DebugType.GAMEFILE, $"{targetPath} - skipped copy.", DebugLogSeverity.INFO);
            }
        }
        else
        {
            BugHunter.Log(DebugType.GAMEFILE, $"Source missing: {sourcePath}", DebugLogSeverity.WARN);
        }
    }

}
