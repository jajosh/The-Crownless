using GameNamespace;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Data.SqlTypes;
using System.Diagnostics;
using System.IO.Pipes;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.JavaScript;
using System.Security.Cryptography.X509Certificates;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Xml.Linq;
using Windows.Devices.Bluetooth;
using Windows.Foundation.Collections;
using Windows.Foundation.Metadata;
using Windows.Globalization;
using Windows.Networking.NetworkOperators;
using Windows.Services.Store;
using Windows.Storage.Provider;
/// <summary>
/// Handles the player object and related methods. 
/// </summary>
public class Player
{
    #region Initialization data
    public bool _isInitialized = false; //Error Check, to do need erro exception here
    static StatusFlags flags = new StatusFlags();
    //Inventory inventory = new Inventory();
    //PlayerInputHandler inputHandler = new PlayerInputHandler();
    static Random random = new Random();
    static EngineGUI UI = new EngineGUI();
    EngineText text = new EngineText();
    #endregion
    
    #region Core data
    public string Name { get; set; }
    public string Race { get; set; }
    // Key = pronoun set ID, Value = (subjective, possessive, reflexive)
    [JsonIgnore]
    Dictionary<int,(string, string, string)> pronouns = new Dictionary<int, (string Subjective, string Possessive, string reflexive)>
        {
            { 1, ("he", "his", "himself") },
            { 2, ("she", "her", "herself") },
            { 3, ("they", "their", "themselves") }
        };
    int pronounKey = 0;
    public string Background { get; set; }
    public int Gridx { get; set; }
    public int GridY { get; set; }
    public int LocalX { get; set; }
    public int LocalY { get; set; }
    #endregion

    
    #region Social data
    public Dictionary<string, int> Reputation { get; set; }
    public List<string> Languages = new List<string>();
    [JsonConverter(typeof(DictionaryKeyConverter<NPC, int>))]
    public Dictionary<NPC, int> KnownNPC {  get; set; }
    #endregion

    
    #region Core Stats data
    //current Health
    public int Health { get; set; }//current Health
    public int MaxHealth { get; set; }
    public int Mana { get; set; }
    public int MaxMana { get; set; }
    public int TileSpeed { get; set; }
    #endregion


    #region Skill and Money Data
    //Gives the absolute value of each coind
    public List<(String Name, int Value)> cointTypes = new()
    {
        ("Gold", 100),
        ("Silver", 10),
        ("Copper", 1)
    };
    // str, dex, etc.
    public Dictionary<Skill, int> Skills { get; set; }
    //used by the object to track coins.
    public Dictionary<string, int> Money { get; set; }
    public int CurrentEvent { get; set; }
    public Dictionary<int, int> PastEvents {  get; set; }
    #endregion

    #region Equipment data
    // The shit you have
    [JsonConverter(typeof(Dictionary<int, Item>))]  
    public Dictionary<int, Item> Inventory { get; set; }
    // Shit you are wearing
    [JsonConverter(typeof(Dictionary<EquipmentSlots, Item>))]
    public Dictionary<EquipmentSlots, Item> EquippedItems { get; set; }
    #endregion


   


    /// <summary>
    /// initializes the player with default values, runs an error check to make
    /// sure that the player correctly initialized, by checking if string
    /// name is empty. Search "AssertInitialized" for more information.
    /// </summary>
    public Player()//^
    {
        _isInitialized = false; //for error checking
        flags.IsDead = false;
        Health = 100;
        MaxHealth = 100;
        Mana = 100;
        MaxMana = 100;
        Name = string.Empty;
        Race = string.Empty;
        pronounKey = 0;
        Background  = string.Empty;
        Skills = new Dictionary<Skill, int>();
        Money = new Dictionary<string, int>();
        Reputation = new Dictionary<string, int>();
        Languages = new List<string>();
        EquippedItems = new Dictionary<EquipmentSlots, Item>();
        KnownNPC = new Dictionary<NPC, int>();
        Inventory = new Dictionary<int, Item>();
        return;
    }
    
    
    /// <summary>
    /// A load file to quickly start debuging</UNFINISHED>
    /// </summary>
    public static SaveGame debugLoadGame()
    {
        SaveGame saveGame = new SaveGame();
        Player player = new Player();
        player.Gridx = 0;
        player.GridY = 0;
        player.LocalX = 11;
        player.LocalY = 8;
        player._isInitialized = false; //for error checking
        flags.IsDead = false;
        player.Health = 100;
        player.MaxHealth = 100;
        player.Mana = 100;
        player.MaxMana = 100;
        player.Name = "BugFinder";
        player.Race = "Bug";
        player.pronounKey = 1;
        player.Background = string.Empty;
        player.Skills.Add(Skill.Strength, 20);
        player.Skills.Add(Skill.Wisdom, 20);
        player.Skills.Add(Skill.Charisma, 20);
        player.Skills.Add(Skill.Constitution, 20);
        player.Skills.Add(Skill.Intelligence, 20);
        player.Skills.Add(Skill.Dexterity, 20);

        player.Money = new Dictionary<string, int>();
        player.Reputation = new Dictionary<string, int>();
        player.Languages = new List<string>();
        player.EquippedItems = new Dictionary<EquipmentSlots, Item>();
        player.KnownNPC = new Dictionary<NPC, int>();
        player.Inventory = new Dictionary<int, Item>();
         
        Console.Clear();
        Item item = new Item();
        item = item.FindItemByName("spoon", MainClass.saveGame.items);
        player.Inventory.Add(1, item);
        player.Money.Add("copper", 100);
        player.Money.Add("silver", 100);
        player.Money.Add("gold", 100);
        saveGame.player = player;
        return saveGame;
    }
    


    #region Money Logic and methods
    //Calculates total money
    public int TotalMoney()
    {
        int totalMoney = Money["copper"] + Money["Silver"] + Money["Gold"];
        return totalMoney;
    }
    // Normalized coins into the smallest amount of coins. 
    public void NormalizeCoins()
    {
        int x = TotalMoney();
        foreach (var(name, value) in cointTypes)
        {
            while (x >= value)
            {
                Money[name] += 1;
                x -= value;
            }

        }
    }
    //Charges the player for an item.
    public bool Spend(int price)
    {
        while (price > 0)
        {
            int _copper = Money["Copper"];
            int _silver = Money["Silver"];
            int _gold = Money["Gold"]; ;
            while (price >= 100 && _gold > 0)
            {
                price -= 100;
                _gold -= 1;
                if (price % 100 > _silver * 10)
                {
                    _gold -= 1;
                    _silver += 10;
                }
            }
            while (price >= 10 && _silver > 0)
            {
                price -= 10;
                _silver -= 1;
                if (price % 10 > _copper)
                    {

                    _silver -= 1;
                    _copper += 10;
                }
            }
            while (price >= 1 && _copper > 0)
            {
                price -= 1;
                _copper -= 1;
            }
            int _TotalMoney = _copper + _silver + _gold;
            if (_TotalMoney < 0)
            { return false; }
            else if (price == 0 && _TotalMoney >= 0)
            {
                Money["Copper"] = _copper;
                Money["Silver"] = _silver;
                Money["Gold"] = _gold;
                return true;
            }
            Console.WriteLine("Error, should not reach this code");
            return false;
        }
        Console.WriteLine("Either item is free or error in code.");
        return true;
    }
    //Prints the players money, by coin type. i.e. gold: 1\nSilver: 2\nCopper: 9
    public void PrintMoney()
    {
        foreach (Moneytype money in Enum.GetValues (typeof(Moneytype)))
        {
            int value = (int)money;
            string name = money.ToString();
            Console.WriteLine($"{name}: {Money[name]}");
        }
    }
    /// <summary>
    /// Handles giving the play change from the merchant or shop keep
    /// </summary>
    /// <param name="amount">Amount of change to give back to the player. </param>
    /// <returns></returns>
    public Dictionary<string, int> GiveChange(int amount)
    {
        Dictionary<string, int> change = new();
        foreach (var (name, value) in cointTypes)
        {
            int count = amount / value;
            if (count > 0)
            {
                change[name] = count;
                amount -= count * value;
            }
        }
        return change;
    }
    #endregion


    #region Equipment Logic and methods
    public void EquipItem(Item item)
    {
        
    }
    public void UnequipItem(Item item)
    {
        text.HandleInput("What would you like to unequip", 100);
    }
    #endregion

    #region skills logic and methods
    //Calulates modifer
    public int modifier(Skill skill, Player player)
    {
        int value = Skills.ContainsKey(skill) ? Skills[skill] : 10;
        return (value - 10) / 2;
    }
    //Prints the players skills to the console.
    public void PrintSkills()
    {
        foreach (Skill skill in Enum.GetValues(typeof(Skill)))
        {
            int value = Skills.ContainsKey(skill) ? Skills[skill] : 0;
            text.Write($"{skill}: {value}");
        }
    }
    //For leveling the player up
    public void LevelUp()
    {
        
    }
    #endregion

    [System.Diagnostics.Conditional("DEBUG")]
    /// <summary>
    /// Checks for errors in the player class
    /// Must be added to as error checking is added
    /// Check VS code for to do's
    private void AssertInitialized()//^
    {
        if (!_isInitialized)
            if (flags.IsDebug)
                Console.WriteLine("WARNING: Player not initialized.");
        try
        {
            throw new InvalidOperationException("Player has not been initialized. Did you forget to call StartNewGame() or LoadGame()?");
        }
        catch (InvalidOperationException ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
}
