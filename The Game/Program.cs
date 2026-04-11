using System.Runtime.InteropServices;
using SQLitePCL;
using WindowsFormsApp1;
using System.Runtime.InteropServices;
namespace The_Game
{

    internal static class Program
    {
        // Allows for the console to be open.
        [DllImport("kernel32.dll")]
        private static extern bool AllocConsole();
        // Allowx the console to be closed
        [DllImport("kernel32.dll")]
        private static extern bool FreeConsole();
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Batteries_V2.Init();
            // Opens the console.
            MyConsole.Init();



            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            BugHunter.Log(DebugType.INIT, "Application is starting", DebugLogSeverity.telemetry);
            ApplicationConfiguration.Initialize();
            Application.Run(new Form1());
        }
        
    }
}