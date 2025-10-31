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
public class Player : ICharacter
{
    
    #region Identity
    // Ties object to IActionable for use in interobject targeting in game.
    public string Name { get; set; }
    public string Race { get; set; }
    public string Background { get; set; }

    // Key = pronoun set ID, Value = (subjective, possessive, reflexive)
    private Dictionary<int, (string Subjective, string Possessive, string Reflexive)> pronouns =
        new()
        {
        { 1, ("he", "his", "himself") },
        { 2, ("she", "her", "herself") },
        { 3, ("they", "their", "themselves") }
        };
    private int pronounKey = 3;
    #endregion

    #region Initialization
    public bool _isInitialized = false; // TODO: Replace with proper exception handling
    private static Random random = new Random();
    private EngineText text = new EngineText();
    #endregion

    #region Actions (IActionable)
    public int TileSpeed { get; set; } = 6;   // Default movement speed
    public int ActionCount { get; } = 1;      // Base actions
    public int BonusActionCount { get; } = 1; // Base bonus actions
    public Root Root { get; set; }
    public int Initiative { get; set; }
    #endregion

    #region Money (IMoney)
    // Player’s wallet
    public Dictionary<CoinType, int> Money { get; set; }
    #endregion

    #region Components
    public List<int> InventoryData { get; set; }
    [JsonIgnore]public InventoryComponent? Inventory { get; set; }
    [JsonIgnore] public HealthComponent? Health { get; set; }
    #endregion

    #region Triggers & Actions
    [JsonIgnore]
    public Dictionary<ActionKeys, GameAction> Triggers { get; set; } = new();
    public Dictionary<ActionKeys, string> Tiggerdata { get; set; }
    #endregion

    #region Social & Progression
    public Dictionary<string, int> Reputation { get; set; }
    public List<string> Languages { get; set; }

    [JsonConverter(typeof(DictionaryKeyConverter<NPC, int>))]
    public Dictionary<NPC, int> KnownNPC { get; set; }

    public Dictionary<Skill, int> Skills { get; set; }
    #endregion

    #region Events
    // Event tracking
    public int CurrentEvent { get; set; }
    public Dictionary<int, int> PastEvents { get; set; }
    #endregion





    public Player()
    {
        // Identity
        Name = string.Empty;
        Race = string.Empty;
        Background = string.Empty;

        // Initialization
        _isInitialized = true;

        TileSpeed = 6; 
        ActionCount = 1; 
        BonusActionCount = 1; 
        Root = new Root(); 
        Initiative = 0;
        Money = new Dictionary<CoinType, int> 
        {
            { CoinType.Gold, 0 },
            { CoinType.Silver, 0 },
            { CoinType.Copper, 0 }
        };
        // Components
        Inventory = new InventoryComponent();
        Health = new HealthComponent();
        Root.GridX = 0;
        Root.GridY = 0;
        Root.LocalX = 11;
        Root.LocalY = 8;
        // Triggers & Actions
        Triggers = new Dictionary<ActionKeys, GameAction>(); 
        Tiggerdata = new Dictionary<ActionKeys, string>();
        // Social & Progression 
        Reputation = new Dictionary<string, int>(); 
        Languages = new List<string>(); 
        KnownNPC = new Dictionary<NPC, int>(); 
        Skills = new Dictionary<Skill, int>();
        // Events
        CurrentEvent = 0; 
        PastEvents = new Dictionary<int, int>();


    }

    #region === IActionable Methods ===
    public bool CanAttack(IActionable target)
    {
        Root caster = Root;
        Root tar = target.Root;
        int x = caster.LocalX - tar.LocalX;
        x = Math.Abs(x);
        int y = caster.LocalY - tar.LocalY;
        y = Math.Abs(y);
        foreach (var trigger in Triggers)
        {
            if (trigger.Value.Range <= x || trigger.Value.Range <= y)
            {
                return true;
            }
        }
        return false;
    }
    public void PerformAttack(IActionable target)
    {
        if (CanAttack(target) && target is IHealth healthTarget)
        {
            // Placeholder: flat 1 damage
            healthTarget.Health.DamageHP(1);
        }
    }
    public bool CanUseBonus() => BonusActionCount > 0;
    public void PerformBonus(IActionable target)
    {

    }
    public CombatRelation GetCombatRelation(SaveGame saveGame)
    {
        // TODO: Add logic for relation checks
        return CombatRelation.Neutral;
    }
    public List<GameAction> PossibleAttacks(ICharacter Target, List<ICharacter> allies)
    {
        List<GameAction> actions = new List<GameAction>();
        return actions;
    }
    public (int GridX, int GridY, int LocalX, int LocalY) GetPosition()
    {
        // TODO: Replace with actual position tracking
        return (0, 0, 0, 0);
    }
    #endregion
     
    

    /// <summary>
    /// A load file to quickly start debuging</UNFINISHED>
    /// </summary>
    public static SaveGame debugLoadGame()
    {
        SaveGame saveGame = new SaveGame();
        Player player = new Player();
        player.Root.GridX = 0;
        player.Root.GridY = 0;
        player.Root.LocalX = 11;
        player.Root.LocalY = 8;
        player._isInitialized = false; //for error checking
        player.Health.CurrentHP = 100;
        player.Health.CurrentMP = 100;
        player.Health.MaxHP = 100;
        player.Health.MaxMP = 100;
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

        player.Reputation = new Dictionary<string, int>();
        player.Languages = new List<string>();
        player.Inventory.EquipedItems = new Dictionary<EquipmentSlots, List<ItemStack>>();
        player.KnownNPC = new Dictionary<NPC, int>();

        Console.Clear();
        Item? foundItem = Item.FindItemByName("spoon");
        if (foundItem != null)
            player.Inventory.AddItem(foundItem);
        player.Money[CoinType.Gold] = 100;
        player.Money[CoinType.Silver] = 100;
        player.Money[CoinType.Copper] = 100;
        saveGame.PlayerCharacter = player;
        saveGame.Weather = new EngineWeather();
        saveGame.Flags = new StatusFlags();
        saveGame.NPCs = new DataNPC();
        saveGame.Flags.IsDebug = true;
        saveGame.NPCs.NPCs = JsonLoader.LoadFromJson<List<NPC>>(FileManager.TheNPCFilePath);
        saveGame.NPCs.NPCType = JsonLoader.LoadFromJson<List<NPCType>>(FileManager.TheNPCTypeFilePath);
        saveGame.NPCs = NPCHydrator.Hydrate(saveGame.NPCs);
        saveGame.Triggers = JsonLoader.LoadFromJson<List<TriggerCoordinets>>(FileManager.TheTriggerCoordinetsPath);
        saveGame.GameMap = Map.LoadMapFromJson();
        return saveGame;
    }



    


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
            if (MainClass.saveGame.Flags.IsDebug)
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
