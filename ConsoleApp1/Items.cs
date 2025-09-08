using GameNamespace;
using System.Linq.Expressions;
using System.Security.Cryptography.X509Certificates;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Xml.Linq;
using Windows.ApplicationModel.DataTransfer;


public class Item
{

    public string? AsciiKey { get; set; }
    public string Name { get; set; } = string.Empty;
    public int ID { get; set; }
    public int SpawnChance { get; set; }
    public int MaxStack { get; set; }
    public int? Price { get; set; }
    public bool CanStore { get; set; } // Determines if the item can be stored in inventory
    public Dictionary<int, string>? EncumbranceErrorMessages { get; set; } = new Dictionary<int, string>();


    // Composable Data Blocks

    public WeaponData? Weapon { get; set; }
    public MeleeWeaponData? Melee { get; set; }
    public RangeWeaponData? Range { get; set; }
    public ConsumableData? Consumable { get; set; }
    public AlchemyData? Alchemy { get; set; }
    public CraftingData? Crafting { get; set; }


    public Dictionary<string, Action<string>> triggerActions { get; set; } = new Dictionary<string, Action<string>>();
    [JsonInclude]
    public List<string>? TriggerKeys { get; set; }

    public Item()
    {
        AsciiKey = string.Empty;
        Name = string.Empty;
        ID = 0;
        SpawnChance = 0;
        MaxStack = 0;
        Price = 0;
        EncumbranceErrorMessages = new Dictionary<int, string>();
        triggerActions = new();
        TriggerKeys = new List<string>();
        CanStore = false;
        Weapon = null;
        Melee = null;
        Range = null;
        Consumable = null;
        Alchemy = null;

    }

    public void SetProperty(string key, string value)//Checks that each property is set correctly
    {
        if (triggerActions.TryGetValue(key.Trim().ToLower(), out var action))
        {
            try
            {
                action(value.Trim());
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error setting property '{key}': {ex.Message}");
            }
        }
        else
        {
            Console.WriteLine($"Unknown property: {key}");
        }
    }

    public Item? FindItemByID(int IDToFind, List<Item> itemList)
    {


        Item? foundItem = itemList.Find(item => item.ID == IDToFind);

        if (foundItem != null)
        {
            Console.WriteLine($"Item Found: {foundItem.Name}");
            return foundItem;
        }
        else
        {
            Console.WriteLine("Item not found.");
            return null;
        }
    }
    public Item? FindItemByName(string NameToFind, List<Item> itemList)
    {
        if (string.IsNullOrWhiteSpace(NameToFind))
        {
            Console.WriteLine("Invalid name.");
            return null;
        }
        Item? foundItem = itemList.Find(item => item.Name.Equals(NameToFind, StringComparison.OrdinalIgnoreCase));
        if (foundItem != null)
        {
            if (MainClass.saveGame.flags.IsDebug)
                Console.WriteLine($"Item Found: {foundItem.Name}");
            return foundItem;
        }
        else
        {
            if (MainClass.saveGame.flags.IsDebug)
                Console.WriteLine($"Item Not Found: {NameToFind}");
            return null;
        }
    }
    public static Item? FindItemByNameStatic(string NameToFind, List<Item> itemList)
    {
        if (string.IsNullOrWhiteSpace(NameToFind))
        {
            Console.WriteLine("Invalid name.");
            return null;
        }
        Item? foundItem = itemList.Find(item => item.Name.Equals(NameToFind, StringComparison.OrdinalIgnoreCase));
        if (foundItem != null)
        {
            if (MainClass.saveGame.flags.IsDebug)
                Console.WriteLine($"Item Found: {foundItem.Name}");
            return foundItem;
        }
        else
        {
            if (MainClass.saveGame.flags.IsDebug)
                Console.WriteLine($"Item Not Found: {NameToFind}");
            return null;
        }
    }


    #region Subclass data
    public class WeaponData
    {
        public int? DamageDie { get; set; }
        public Skill DamageBonusSkill { get; set; }
        damanageType DamageType { get; set; }

    }
    public class MeleeWeaponData
    {
        public bool Versatile { get; set; }
    }
    public class RangeWeaponData
    {
        public int Range { get; set; }
    }
    public class ConsumableData
    {
        public int HealthToHeal { get; set; }
        public bool Cookable { get; set; }
        public int CookingDifficulty { get; set; }//1 to 10, 0 if not cookable

    }
    public class AlchemyData
    {
        public int AlchemyID { get; set; }
    }
    public class CraftingData
    {
        public int CraftID { get; set; }
        public bool IsCookable { get; set; }
    }
    #endregion
    public static class ItemTriggers
    {
        public static readonly Dictionary<string, Action<Item, string>> ActionMap = new()
        {
            ["name"] = (item, val) => item.Name = val,
            ["price"] = (item, val) => item.Price = int.TryParse(val, out var result) ? result : null,
            ["maxstack"] = (item, val) => item.MaxStack = int.TryParse(val, out var result) ? result : item.MaxStack,
            // Add more as needed
        };

    }
}