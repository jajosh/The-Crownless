using ABI.Windows.AI.MachineLearning;
using GameNamespace;
using System;
using System.ComponentModel.Design;
using System.Security.Cryptography.X509Certificates;
using System.Threading;


/// <summary>
/// The canister for each event instance
/// </summary>
public class EventTree 
{
    public int ID { get; set; }
    public Root? Startinglocation { get; set; } // The grid x and y and local x and y trigger location for this event. Leave blank to represent a random event.
    public string Title { get; set; } // Title of the event. For readability, referencing and possibly for player journal.
    public int? LinkedID { get; set; } // If this is linked to a quest, pulls the quest
    public List<string> Description { get; set; }

    public List<int>? EnemyID { get; set; } // to pull list<NPC> for easy access
    public List<int>? AllyID { get; set; } // to pull list<NPC> for easy access

    public Dictionary<int, EventBranchPoint> Branches { get; set; } // The meat and potateos of the event
}
/// <summary>
/// Represents a choice within an event, including its properties, conditions, and potential outcomes. This is what makes up the branches of the event tree
public class EventBranchPoint
{
    public int ChoiceID { get; set; } // ID used to navigate the event tree
    public string? Text { set; get; } // Text to print to the screen. Narration
    public string? Dialog { get; set; } // Dialog to print to the screen. Character speaking. Separate as dialog will be printed character by character verses all at one like Text
    public Dictionary<int, Limbs> Limbs { get; set; } // The possible choices 
    public CombatBranchDataComposition? Combat {  get; set; } // meaning this is a combat event
    public DialogBranchDataComposition? DialogTree { get; set; } // meaning this is a dialog event
    public List<Theleaves>? Leaves { get; set; } // Leads to the end of the event

    #region --- Branch Types
    public class CombatBranchDataComposition
    {
        public bool isCombat { get; set; } // bool representing the player wins the combat
        public int? RewardXP { get; set; } // Leave Blank to revert to default value based on the NPC
        public List<int>? RewardItemID { get; set; }// Leave Blank to revert to default value based on the NPC
        public List<int>? NPCID { get; set; } // Pulls the NPC talking, who the check is against.
    }
    public class DialogBranchDataComposition
    {
        public int? NPCID { get; set; } // Pulls the NPC talking, who the check is against.
        public DialogLimb? Limb { get; set; } // The start of the dialog event sub path. The limb of for the dialog before getting to the players skill check
        public string CheckText {  get; set; } // The final Dialog before the check
    }
    // Add more branch types here
    #endregion
}
/// <summary>
/// Represents the next possible EventChoice 
/// </summary>
public class Limbs
{
    public int NextBranchID { get; set; } // Event choice to go to
    public bool? RequiredFlag {  get; set; } // The required bool value to have fo this path
    public bool? AlreadyGoneDown { get; set; }
    public string ChoiceText { set; get; }
    public int? DC { get; set; } // use 0 to represent fail. 1 to represent a crit fail.
    public Skill? skill { get; set; } // Skill to Check against
    public int? requiredItem { get; set; }
    public int? TimePassed { get; set; } // The time passed in days for this choice. Leave at null for no time passed
}
/// <summary>
/// The trigger point for the event. Stored here so that it doesnt need to be held with the other triggercoordinets. 
/// </summary>
public class Root
{
    public int GridX { get; set; }
    public int GridY { get; set; }
    public int LocalX { get; set; }
    public int LocalY { get; set; }
}
/// <summary>
/// Represents the end of the event, the end of the canister
/// </summary>
public class Theleaves
{
    public bool? RequiredFlag { get; set;}
    public int? DC { get; set; }
    public Skill? Skill { get; set; }
    public int RewardXP {  get; set; }
    public List<int> RewardItems { get; set; }
    public Root? NewRoot { get; set; }
}
/// <summary>
/// Handles straight dialog paths
/// </summary>
public class DialogLimb
{
    // int = the order it is printed
    public Dictionary<int, string> Text { set; get; } 
    public Dictionary<int, string> Dialog {  set; get; }
}

public class ScriptedEvents
{
    private static EngineText Text = new EngineText();
    /// <summary>
    /// STARTS NEW GAME
    /// 
    /// Starts a new game by initializing the player and runs through setting up 
    /// the player character.
    /// Still needs pretty much everything added to it.
    /// </summary>
    /// <return>Returns the saveGame objext that holds all things that are to be saved</return>
    public static SaveGame StartNewGame()
    {
        SaveGame saveGame = new SaveGame();
        Player player = new Player();
        List<TriggerCoordinets> triggers = JsonLoader.LoadFromJson<List<TriggerCoordinets>>(FileManager.TheTriggerCoordinetsPath);
        EngineWeather weather = new EngineWeather();
        StatusFlags flags = new StatusFlags();
        List<Quest> completedQuests = new List<Quest>();
        List<Quest> activeQuests = new List<Quest>();
        List<Item> items = new List<Item>();
    Random rng = new Random();

        // --- Starting location
        player.Gridx = 0;
        player.GridY = 0;
        player.LocalX = 12;
        player.LocalY = 9;
        // --- Player stats
        player.Health = 100;
        player.MaxHealth = 100;
        player.Mana = 100;
        player.MaxMana = 100;
        // --- Adds 0 values so that it prints to the UI
        player.Money.Add("copper", 0);
        player.Money.Add("silver", 0);
        player.Money.Add("gold", 0);
        // --- Fades screen to white then turns text black
        ConsoleColor[] fadeSteps = new ConsoleColor[]
        {
            ConsoleColor.DarkGray,
            ConsoleColor.Gray,
            ConsoleColor.White
        };
        foreach (var color in fadeSteps)
        {
            Console.BackgroundColor = color;
            Console.Clear();
            Thread.Sleep(1000); // pause for 200ms
        }
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.Black;
        Thread.Sleep(500);
        Text.Write("Awake\n", 50); System.Threading.Thread.Sleep(500);
        Text.Write("Awake my wielder. You have been asleep for so long\n", 100);
        Text.Write("It is time for that sleep to end\n", 100);
        bool loopChecker = true;
        while (loopChecker)
        {
            string name = Text.Read("What is your name?\n");
            if (name != null)
            {
                Text.Write("That is not a name...\n");
            }
            else
            {
                Text.Write($"{name}... are you sure?\n");
                Console.Write("\n\nEnter yes or no: \n\n");
                string input = Console.ReadLine().Trim().ToLower();
            }
        }
        // Initalizes the players skills
        Console.Clear();
        Console.Write("Strength:\nDexterity\nCharisma\nIntelligence\nWisom\nConstitution");
        for (int i = 0; i < 10; i++)
        {
            Console.SetCursorPosition(15, i);
            Console.Write("  ");
            Console.Write(rng.Next(8, 18));
        }
        player.Skills.Add(Skill.Strength, rng.Next(8, 18));
        player.Skills.Add(Skill.Dexterity, rng.Next(8, 18));
        player.Skills.Add(Skill.Charisma, rng.Next(8, 18));
        player.Skills.Add(Skill.Intelligence, rng.Next(8, 18));
        player.Skills.Add(Skill.Wisdom, rng.Next(8, 18));
        player.Skills.Add(Skill.Constitution, rng.Next(8, 18));
        Text.Write("Hmmm, not as strong as i would've thought");

        //The players first weapon
        bool checker = true;
        while (checker) {
            Console.Clear();
            Text.Write("What weapon would you like?\nA shortbow\nA scimitar\nA spoon");
            string response = Console.ReadLine().ToLower();
            if (response.StartsWith("a ", StringComparison.OrdinalIgnoreCase))
            {
                response = response.Substring("a ".Length).TrimStart();
            }
            if (response != null)
                checker = true;
            else
            {
                if (response == "shortbow" || response == "scimitar" || response == "spoon")
                {
                    Item item = new Item();
                    item = item.FindItemByName(response, MainClass.saveGame.items);
                    MainClass.saveGame.player.Inventory.Add(1, item);
                    checker = false;
                }
                else
                { Console.WriteLine("Please select a valid weapon."); }
            }
        }
        saveGame.player = player;
        saveGame.triggers = triggers;
        saveGame.weather = weather;
        saveGame.flags = flags;
        saveGame.completedQuests = completedQuests;
        saveGame.activeQuests = activeQuests;
        saveGame.items = items;
        return saveGame;

    }
}