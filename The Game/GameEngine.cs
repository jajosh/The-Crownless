using System;
using System.Collections.Generic;
using System.Text;
using WindowsFormsApp1;


public class GameEngine
{
    // Just REFERENCES - no instantiation here!
    public IBeeperEngine Beeper { get; set; }
    public IIFileEngine FileManager { get; init; }
    public INPCEngine NPC { get; init; }
    public IPlayerEngine Player { get; init; }
    public ITileProcessorEngine Processor { get; init; }
    public MapEngine Map { get; init; }
    public INPCTypeEngine NPCType { get; init; }

    public List<IActionable> ActiveOnScreen { get; set; } = new();
    public List<IActionable> ActiveOnLocation { get; set; } = new();

    public GameEngine(IBeeperEngine beeper, IIFileEngine fileManager, INPCEngine npc, IPlayerEngine player, ITileProcessorEngine processor, MapEngine map, INPCTypeEngine npcType)
    {
        Beeper = beeper;
        FileManager = fileManager;
        NPC = npc;
        Player = player;
        Processor = processor;
        Map = map;
        NPCType = npcType;
    }

    public void Assembly()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        // Components initialize THEMSELVES
    }
}
