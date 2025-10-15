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
    public EquipmentSlots SlotType { get; set; }
    public int SpawnChance { get; set; }
    public int MaxStack { get; set; }
    public int? Price { get; set; }
    public bool CanStore { get; set; } // Determines if the item can be stored in inventory
    public List<string> EncumbranceErrorMessages { get; set; } = new();
    
    public WeaponDataFrame? WeaponData { get; set; }
    public MeleeWeaponDataFrame? MeleeWeaponData { get; set; }
    public RangeWeaponDataFrame? RangeWeaponData { get; set; }
    public ConsumableDataFrame? ConsumableData { get; set; }
    public AlchemyDataFrame? AlchemyData { get; set; }
    public CraftingDataFrame? CraftingData { get; set; }
    public ArmorDataFrame ArmorData { get; set; }

    // Actions that the item can use
    [JsonIgnore]
    public Dictionary<TriggerEnum, GameAction> TriggerData = new();
    // key to hydrate into the triggerActions
    public List<string>? TriggerRawData { get; set; }

    public Item()
    {

    }
    /// <summary>
    /// Hydrates the TriggerActions dictionary based on TriggerKeys
    /// </summary>
    public List<Item> Hydrate()
    {
        // Load all raw items from JSON
        List<Item> RawData = JsonLoader.LoadFromJson<List<Item>>(FileManager.ItemFilePath);
        List<Item> Hydrated = new List<Item>();
        foreach (var item in RawData)
        {

            // Handles loading in the trigger data for the items. IE: Heal, Curropting Touch
            foreach (var trigger in TriggerRawData)
            {
                GameActionFactory.Create(trigger);
            }
        }
        return RawData;
    }
    public static Item? FindItemByID(int IDToFind)
    {
        List<Item> itemList = FileManager.Items;
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
    public static Item? FindItemByName(string NameToFind)
    {
        if (string.IsNullOrWhiteSpace(NameToFind))
        {
            Console.WriteLine("Invalid name.");
            return null;
        }

        if (FileManager.Items == null)
        {
            Console.WriteLine("FileManager.Items is NULL! Did you call FileManager.LoadData()?");
            return null;
        }

        Item? foundItem = FileManager.Items.Find(item =>
            !string.IsNullOrEmpty(item.Name) &&
            item.Name.Equals(NameToFind, StringComparison.OrdinalIgnoreCase));

        if (foundItem != null)
        {
            if (MainClass.saveGame?.Flags?.IsDebug == true)
                Console.WriteLine($"Item Found: {foundItem.Name}");
            return foundItem;
        }
        else
        {
            if (MainClass.saveGame?.Flags?.IsDebug == true)
                Console.WriteLine($"Item Not Found: {NameToFind}");
            return null;
        }
    }


    #region Subclass data
    public class WeaponDataFrame
    {
        public int? DamageDie { get; set; }
        public Skill DamageBonusSkill { get; set; }
        public DamageTypes DamageType { get; set; }

    }
    public class MeleeWeaponDataFrame
    {
        public bool Versatile { get; set; }
    }
    public class RangeWeaponDataFrame
    {
        public int Range { get; set; }
    }
    public class ArmorDataFrame
    {
        public EquipmentSlots SlotType { get; set; }
        public List<EquipmentSlots>? Invalidated { get; set; }
    }
    public class ConsumableDataFrame
    {
        public int HealthToHeal { get; set; }
        public bool Cookable { get; set; }
        public int CookingDifficulty { get; set; }//1 to 10, 0 if not cookable

    }
    public class AlchemyDataFrame
    {
        public int AlchemyID { get; set; }
    }
    public class CraftingDataFrame
    {
        public int CraftID { get; set; }
        public bool IsCookable { get; set; }
    }
    #endregion
    
}
