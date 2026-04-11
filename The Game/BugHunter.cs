using MyGame.Controls;
using System;
using System.Collections.Concurrent;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
public enum DebugLogSeverity // The severity of the message being sent.

{

    telemetry = 0, // 4D0D11 → RGB(77, 13, 17)
    // Telemetry data
    FILESINK = 1, // 7A2B00 → RGB(122, 43, 0)
    // Data related to files 
    INFO = 2, // 23684C → RGB(35, 104, 76)
    // Worthy of note but not of importance
    DEBUG = 3, // 0A1D6D → RGB(10, 29, 109)
    // Relanvant to potential issues
    WARN = 4, // A39B6D → RGB(163, 155, 109)
    // Likely an issue
    ERROR = 5, // FCBD81 → RGB(252, 189, 129)
    // Issue, needs to be addressed but not game breaking
    FATAL = 6, // EBEDC9 → RGB(235, 237, 201)
    // Game breaking, needs to be addressed
}
public enum DebugType
{
    // Engine
    INIT, // 5A4335 → RGB(90, 67, 53)
    GAMELOADINGSTAGE, // 834D2F → RGB(131, 77, 47)
    ERROR, // B3886F → RGB(179, 136, 111)
    LOG, // CF8114 → RGB(207, 129, 20)

    // World
    MAPGENERATION, // 166305 → RGB(22, 99, 5)
    GENERICPROCESSING, 
    LOCATIONPROCESSING, // !!! NO COLOR !!!
    GRIDPROCESSING, // 7ED256 → RGB(126, 210, 86)
    TILEPROCESSING, // 75E06B → RGB(117, 224, 107)
    MAPRENDERING, // CAF2BE → RGB(202, 242, 190)

    // Gameplay
    MOVEMENT, // 8E1180 → RGB(142, 17, 128)

    // Data
    SQL, // 5E1DC5 → RGB(94, 29, 197)
    LOCATIONREPOSITORY,// B36CDA → RGB(179, 108, 218)
    GRIDREPOSITORY, // BA09C4 → RGB(186, 9, 196)
    TILEREPOSITORY, // AD3DD6 → RGB(173, 61, 214)
    GAMEFILE, // C381FC → RGB(195, 129, 252)



}
public class DebugMessage

{

    public DebugType Type { get; set; }
    public string Message { get; set; }
    public DebugLogSeverity Severity { get; set; }
    public int ThreadId { get; set; } // Added Thread ID property
    public DebugMessage(DebugType type, string message, DebugLogSeverity severity)
    {
        Type = type;
        Message = message;
        Severity = severity;
        // Capture the ID of the thread that called the Log method
        ThreadId = Environment.CurrentManagedThreadId;
    }
}
public static class MyConsole
{
    private const int STD_OUTPUT_HANDLE = -11;
    private const uint ENABLE_VIRTUAL_TERMINAL_PROCESSING = 0x0004;

    [DllImport("kernel32.dll")]
    private static extern IntPtr GetStdHandle(int nStdHandle);
    [DllImport("kernel32.dll")]
    private static extern bool GetConsoleMode(IntPtr hConsoleHandle, out uint lpMode);
    [DllImport("kernel32.dll")]
    private static extern bool SetConsoleMode(IntPtr hConsoleHandle, uint dwMode);
    [DllImport("kernel32.dll", SetLastError = true)]
    private static extern bool AllocConsole();

    private static bool _initialized = false;

    public static void Init()
    {
        if (_initialized) return;

        AllocConsole();

        // Redirect standard output to the new console window
        var standardOutput = new StreamWriter(Console.OpenStandardOutput()) { AutoFlush = true };
        Console.SetOut(standardOutput);
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        // Enable ANSI Colors
        var handle = GetStdHandle(STD_OUTPUT_HANDLE);
        if (GetConsoleMode(handle, out uint mode))
        {
            SetConsoleMode(handle, mode | ENABLE_VIRTUAL_TERMINAL_PROCESSING);
        }

        _initialized = true;
        BugHunter.Log(DebugType.INIT, "System Online and Terminal Connected!");
    }

    // --- Color Helpers ---
    public static string Reset => "\x1b[0m";
    public static string Rgb(int r, int g, int b) => $"\x1b[38;2;{r};{g};{b}m";
    public static string BgRgb(int r, int g, int b) => $"\x1b[48;2;{r};{g};{b}m";

    public static string ColorFromRgb(Color color) => Rgb(color.R, color.G, color.B);
    public static string Rgb(ColorComponent color) => Rgb(color.R, color.G, color.B);
}
public static class BugHunter
{
    private static readonly BlockingCollection<DebugMessage> _logQueue = new BlockingCollection<DebugMessage>();
    private const string LogPath = "debug_log.txt";

    static BugHunter()
    {
        // Dedicated thread for processing logs
        Task.Factory.StartNew(ProcessLogs, TaskCreationOptions.LongRunning);
    }

    public static void Log(DebugType type, string message, DebugLogSeverity severity = DebugLogSeverity.INFO, bool Private = true)
    {
        // Private is for the future when i want to print these out to the player
        _logQueue.Add(new DebugMessage(type, message, severity));
    }

    public static void Log(DebugType type, Exception ex, DebugLogSeverity severity = DebugLogSeverity.ERROR)
    {
        string fullMessage = $"Exception: {ex.Message}{Environment.NewLine}Stack Trace: {ex.StackTrace}";
        if (ex.InnerException != null)
            fullMessage += $"{Environment.NewLine}Inner Exception: {ex.InnerException.Message}";

        Log(type, fullMessage, severity);
    }

    private static void ProcessLogs()
    {
        foreach (var item in _logQueue.GetConsumingEnumerable())
        {
            try
            {
                string typeCol = GetTypeColor(item.Type);
                string sevCol = GetSeverityColor(item.Severity);

                // Using padding (-18 and -8) to ensure pipes line up
                string formatted = $"{DateTime.Now:HH:mm:ss.fff} " +
                    $"[T:{item.ThreadId:00}] " +
                    $"[{typeCol}{item.Type,-18}{MyConsole.Reset}] | " +
                    $"[{sevCol}{item.Severity,-8}{MyConsole.Reset}] | " +
                    $"{item.Message}";

                Console.WriteLine(formatted);

                if (item.Severity >= DebugLogSeverity.DEBUG)
                {
                    CheckFileSizeAndRotate();
                    string cleanText = System.Text.RegularExpressions.Regex.Replace(formatted, @"\x1b\[[0-9;]*m", "");
                    File.AppendAllText(LogPath, cleanText + Environment.NewLine);
                }
            }
            catch { /* Best effort */ }
        }
    }

    private static string GetSeverityColor(DebugLogSeverity s) => s switch
    {
        DebugLogSeverity.telemetry => MyConsole.Rgb(77, 13, 17),
        DebugLogSeverity.FILESINK => MyConsole.Rgb(122, 43, 0),
        DebugLogSeverity.INFO => MyConsole.Rgb(35, 104, 76),
        DebugLogSeverity.DEBUG => MyConsole.Rgb(10, 29, 109),
        DebugLogSeverity.WARN => MyConsole.Rgb(163, 155, 109),
        DebugLogSeverity.ERROR => MyConsole.Rgb(252, 189, 129),
        DebugLogSeverity.FATAL => MyConsole.Rgb(235, 237, 201),
        _ => MyConsole.Reset
    };

    private static string GetTypeColor(DebugType t) => t switch
    {
        DebugType.INIT => MyConsole.Rgb(90, 67, 53),
        DebugType.MAPGENERATION => MyConsole.Rgb(22, 99, 5),
        DebugType.GRIDPROCESSING => MyConsole.Rgb(126, 210, 86),
        DebugType.TILEPROCESSING => MyConsole.Rgb(117, 224, 107),
        DebugType.SQL => MyConsole.Rgb(94, 29, 197),
        DebugType.GAMEFILE => MyConsole.Rgb(195, 129, 252),
        _ => MyConsole.Reset
    };

    private static void CheckFileSizeAndRotate()
    {
        FileInfo info = new FileInfo(LogPath);
        if (info.Exists && info.Length > 5_000_000)
        {
            string oldPath = "debug_log_old.txt";
            if (File.Exists(oldPath)) File.Delete(oldPath);
            File.Move(LogPath, oldPath);
        }
    }
}