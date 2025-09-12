
using System;
using static System.Net.Mime.MediaTypeNames;

public class Combat
{
    GUIEngine UI = new GUIEngine();
    TextHandler text = new TextHandler();
    /// <summary>
    /// Protentional unicode characters to use
    /// 
    ///  ⭍ 
    /// </summary>
    /// <param name="player"></param>
    /// <param name="enemy"></param>
    public Combat(Player player, List<int> enemy, List<int> ally)
	{
        text.Write("Starting Combat", 50);
		while (player.Health  > 0)
		{

		}
	}
}

