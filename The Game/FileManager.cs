using NAudio.Gui;
using WindowsFormsApp1;

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
                BugHunter.Log($"[WARNING] Folder missing: {folder}", DebugLogSeverity.Debug);
            else
                BugHunter.Log($"[SUCCESS] Folder missing: {folder}", DebugLogSeverity.Info);
        }

        string[] files = {
                FilePaths.ItemFilePath, FilePaths.ConfigFilePath, FilePaths.EventsFilePath,
                FilePaths.GridFilePath, FilePaths.TriggerCoordinatesPath, FilePaths.NPCFilePath,
                FilePaths.NPCTypeFilePath, FilePaths.QuestFilePath, FilePaths.RandomTextPath,
                FilePaths.RandomEnvironmentalDialogPath
            };
        foreach (var file in files)
        {
            if (File.Exists(file))
                BugHunter.Log($"[SUCCESS] Folder exists: {file}", DebugLogSeverity.Info);
            else
                BugHunter.Log($"[SUCCESS] Folder missing: {file}");
        }
    }

    public void PrintSaveFiles()
    {
        if (!Directory.Exists(FilePaths.SavesFolder)) return;
        var saveFiles = Directory.GetFiles(FilePaths.SavesFolder, "*.json");  // Fixed: Enumerable files
        foreach (var file in saveFiles)
        {
            BugHunter.Log($"Save file: {Path.GetFileName(file)}");
        }
    }


    public void EnsureDataFilesExist()
    {
        Directory.CreateDirectory(FilePaths.DataFolder);
        Directory.CreateDirectory(FilePaths.ConfigFolder);

        CopyIfMissing("Data/items.json", FilePaths.ItemFilePath);
        CopyIfMissing("Data/Maps.json", FilePaths.GridFilePath);
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
                BugHunter.Log($"[COPIED] {relativePathFromProject} → {targetPath}");
            }
            else
            {
                // Optional: Uncomment for always-overwrite
                // File.Copy(sourcePath, targetPath, overwrite: true);
                BugHunter.Log($"[EXISTS] {targetPath} - skipped copy.");
            }
        }
        else
        {
            BugHunter.Log($"[WARNING] Source missing: {sourcePath}");
        }
    }

}