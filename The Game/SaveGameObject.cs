using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class SaveGame
{
    public PlayerObject PlayerCharacter { get; set; }
    public StatusFlags Flags { get; set; }
    public List<QuestObject> CompletedQuests { get; set; }
    public List<QuestObject> ActiveQuests { get; set; }
    public NPCData NPCs { get; set; }
    public List<ItemObject> Items { get; set; }
    public List<TriggerCoordinets> Triggers { get; set; }
    //public LoreBoard LoreBoard { get; set; }
    public MapObject GameMap { get; set; }
    public SaveGame()
    {

    }
}
