
using System;
using static System.Net.Mime.MediaTypeNames;

public class Combat
{
    EngineGUI UI = new EngineGUI();
    EngineText text = new EngineText();
    /// <summary>
    /// The combat loop. 
    /// 
    /// Protentional unicode characters to use
    /// 
    ///  ⭍ 
    /// </summary>
    /// <param name="player"></param>
    /// <param name="enemy"></param>
    public Combat(Player player, List<int> enemy, List<int> ally)
    {
        EngineText.Write("Starting Combat", 50);
        bool isRunning = true;
        
        while (isRunning)
        {

        }
    }
    public void StartCombat(Player player, List<NPC> enemy, List<NPC> ally)
    {
        EngineText text = new();
        Random rng = new Random();
        int enemyListLength = enemy.Count;
        
    }
    /// <summary>
    /// Grabs a random chat from a random enemy and one of their chats
    /// </summary>
    /// <param name="npcs">List of NPCs involed</param>
    /// <returns></returns>
    //public (NPC, string) GetRandomBattleDialog(List<NPC> npcs)
    //{
    //    string result = string.Empty;
    //    Random rng = new Random();
    //    int x = rng.Next(npcs.Count);
    //    int b = npcs[x].RandomDialog.Count;
    //    NPC speaker = npcs[x];
    //    EngineRandomText chat = speaker.RandomDialog[rng.Next(speaker.RandomDialog.Count)];
    //    x = chat.Text.Count;
    //    result = chat.Text[rng.Next(x)];
    //    var sendback = (speaker, result);
    //    return sendback;
        
    //}
}

