using System.Runtime.InteropServices;
using WindowsFormsApp1;
using SQLitePCL;
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
            AllocConsole();

            

            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            Application.Run(new Form1());
        }
        public static void ShutDown()
        {
            GameDataBase.CloseConnection();
        }
    }
}