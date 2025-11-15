using System;

public enum DebugLogSeverity // The severity of the message being sent
{
    Debug,
    Info,
    Warning,
    Error,
    Fatal

}
public static class BugHunter
{

    public static void Log(string message) => Log(message, DebugLogSeverity.Info);
    public static void Log(string message, DebugLogSeverity Severity)
    {
        try
        {
            File.AppendAllText("debug_log.txt", $"{DateTime.Now}: [[{Severity}]] {message}\n");
            Console.WriteLine($"{DateTime.Now}: [[{Severity}]] {message}\n");
        }
        catch { } // Silent fail if logging itself breaks
    }
    /// <summary>
    /// Logs the message and stack trace of the specified exception and all inner exceptions.
    /// </summary>
    /// <param name="exception">The exception to log. If the exception contains inner exceptions, each will be logged in sequence. Cannot be
    /// null.</param>
    public static void LogException(Exception exception)
    {
        while (exception != null)
        {
            Log(exception.Message + "\n" + exception.StackTrace, DebugLogSeverity.Fatal);
            exception = exception.InnerException!;
        }
    }

}
