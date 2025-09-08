using ABI.Windows.AI.MachineLearning;
using GameNamespace;
using System;
using System.ComponentModel.Design;
using System.Threading;


/// <summary>
///Checks for which event should be running and grabs the next event
/// </summary>
public class Events
{
    public int id { get; set; }
    public string title { get; set; }
    public EventType? type { get; set; }
    public int? linkedID { get; set; }
    public Dictionary<int, string> description { get; set; }
    public List<EventChoice> choices { get; set; }
    public List<Events> theTree { get; set; }

    public combatData? Combat { get; set; }
    public DialogData? Dialog { get; set; }


    public class combatData
    {
        public List<int>? EnemyID { get; set; }
        public List<int>? AllyID { get; set; }
    }
    public class DialogData
    {
        public List<string>? InvolvedNPC { get; set; }
    }
}
public class EventChoice
{

    public string text {  set; get; }
    public bool? pass { get; set; }
    public int? requiredItem {  get; set; }
    public int? weight { get; set; }// Chance out of 100 the event will happen
    public SkillSubset? skill { get; set; }
    public int? skillDifficulty { get; set; }
    public int? nextEvent { get; set; }
    public int? fallBackEvent { get; set; }
    public List<EventChoice>? dialogTree { get; set; }
    public List<EventChoice>? choices {  get; set; }
    public List<EventChoice>? skillCheck { get; set; }
    
}

public class ScriptedEvents
{
    private static TextHandler text = new TextHandler();
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
        WeatherEngine weather = new WeatherEngine();
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
        text.Write("Awake\n", 50); System.Threading.Thread.Sleep(500);
        text.Write("Awake my wielder. You have been asleep for so long\n", 100);
        text.Write("It is time for that sleep to end\n", 100);
        bool loopChecker = true;
        while (loopChecker)
        {
            string name = text.Read("What is your name?\n");
            if (name != null)
            {
                text.Write("That is not a name...\n");
            }
            else
            {
                text.Write($"{name}... are you sure?\n");
                Console.Write("\n\nEnter yes or no: \n\n");
                string input = Console.ReadLine().Trim().ToLower();
                if (WordAlias.Responses.TryGetValue(input, out var response))
                {
                    {
                        loopChecker = false;
                    }
                }
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
        text.Write("Hmmm, not as strong as i would've thought");

        //The players first weapon
        bool checker = true;
        while (checker) {
            Console.Clear();
            text.Write("What weapon would you like?\nA shortbow\nA scimitar\nA spoon");
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