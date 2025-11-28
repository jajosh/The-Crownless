using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


// The Wiring + dependency Injection
public static class GameFactory
{
    public static GameEngine Create()
    {
        try
        {
            var beeper = new BeeperManager();
            Log("BeeperManager created successfully.");

            var fileManager = new FileManager();
            Log("FileManager created successfully.");

            var npc = new NPCManager();
            Log("NPCManager created successfully.");

            var player = new PlayerManager();
            Log("PlayerManager created successfully.");

            var processor = new TileProcessorManager();
            Log("TileProcessorManager created successfully.");

            var map = new MapManager();
            Log("MapManager created successfully.");

            var npcType = new NPCTypeManager();
            Log("NPCTypeManager created successfully.");

            var weather = new WeatherManager();

            var engine = new GameEngine(
                beeper: beeper,
                fileManager: fileManager,
                npc: npc,
                player: player,
                processor: processor,
                map: map,
                npcType: npcType,
                weather: weather
            );
            Log("GameEngine created successfully.");

            return engine;
        }
        catch (Exception ex)
        {
            Log($"Exception in Create: {ex.Message}\nStackTrace: {ex.StackTrace}");
            throw; // Re-throw to let the app handle it, or return null if needed
        }
    }

    private static void Log(string message)
    {
        try
        {
            File.AppendAllText("debug_log.txt", $"{DateTime.Now}: {message}\n");
        }
        catch { } // Silent fail if logging itself breaks
    }

}
