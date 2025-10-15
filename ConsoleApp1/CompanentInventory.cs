using System;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
public enum EquipmentSlots
{
    // Armor
    Head,
    Chest,
    Waist,
    Legs,
    Feet,
    Arms,
    Hands,
    Wrists,
    Shoulders,

    // Accessories
    Neck,
    Ring,
    Bracelet,
    Trinket,

    // Weapons
    MainHand,
    OffHand,
    Back,
    LowerBack,
    Side,


    // Miscellaneous / Custom
    Cap,
    Belt,
    Tabard
}

/// <summary>
/// Represents a player's inventory, including item storage and equipment management.
/// Provides methods to add, remove, equip, and query items.
/// </summary>
public class InventoryComponent : IInventory
{
    //Shit you are holding

    public int Capacity { get; private set; }
    public Dictionary<int, ItemStack> Inventory { get; set; }
    public Dictionary<EquipmentSlots, List<ItemStack>> EquipedItems { get; set; }
    

    public InventoryComponent()
    {
        Capacity = 1;
        Inventory = new Dictionary<int, ItemStack>();
        Dictionary<EquipmentSlots, List<ItemStack>> EquipedItems = new Dictionary<EquipmentSlots, List<ItemStack>>
        {
            { EquipmentSlots.Head, new List<ItemStack>() },
            { EquipmentSlots.Chest, new List<ItemStack>() },
            { EquipmentSlots.Waist, new List<ItemStack>() },
            { EquipmentSlots.Legs, new List<ItemStack>() },
            { EquipmentSlots.Feet, new List<ItemStack>() },
            { EquipmentSlots.Arms, new List<ItemStack>() },
            { EquipmentSlots.Hands, new List<ItemStack>() },
            { EquipmentSlots.Wrists, new List<ItemStack>() },
            { EquipmentSlots.Shoulders, new List<ItemStack>() },
            { EquipmentSlots.Neck, new List<ItemStack>() },

            { EquipmentSlots.Ring, new List<ItemStack>() },       // multiple allowed
            { EquipmentSlots.Bracelet, new List<ItemStack>() },   // multiple allowed
            { EquipmentSlots.Trinket, new List<ItemStack>() },    // multiple allowed

            { EquipmentSlots.MainHand, new List<ItemStack>() },
            { EquipmentSlots.OffHand, new List<ItemStack>() },
            { EquipmentSlots.Back, new List<ItemStack>() },
            { EquipmentSlots.LowerBack, new List<ItemStack>() },
            { EquipmentSlots.Side, new List<ItemStack>() },

            { EquipmentSlots.Cap, new List<ItemStack>() },
            { EquipmentSlots.Belt, new List<ItemStack>() },
            { EquipmentSlots.Tabard, new List<ItemStack>() },
        };
    }
    public bool AddItem(Item item, int quantity = 1)
    {
        // 1️⃣ Check if item already exists in any slot (stacking)
        var existingSlot = Inventory.FirstOrDefault(s => s.Value != null && s.Value.Item.ID == item.ID);
        if (existingSlot.Value != null)
        {
            existingSlot.Value.Add(quantity);
            return true;
        }

        // 2️⃣ Find the first empty slot
        for (int slot = 0; slot < Capacity; slot++)
        {
            if (!Inventory.ContainsKey(slot))
            {
                Inventory[slot] = new ItemStack(item, quantity);
                return true;
            }
        }
        // Inventory full
        return false;
    }
    public bool RemoveItem(Item item, int quantity = 1)
    {
        var slot = Inventory.FirstOrDefault(s => s.Value != null && s.Value.Item.ID == item.ID);
        if (slot.Value == null) return false;

        bool removed = slot.Value.Remove(quantity);
        if (slot.Value.stackSize <= 0)
            Inventory.Remove(slot.Key);

        return removed;
    }
    public bool EquipItem(Item item)
    {
        if (item.SlotType == null)
            return false;

        if (EquipedItems.TryGetValue(item.SlotType, out var slotList))
        {
            // Example: allow 2 rings max, 2 bracelets max, 2 trinkets max
            int maxAllowed = item.SlotType switch
            {
                EquipmentSlots.Ring => 2,
                EquipmentSlots.Bracelet => 2,
                EquipmentSlots.Trinket => 2,
                _ => 1
            };

            if (slotList.Count < maxAllowed)
            {
                slotList.Add(new ItemStack(item, 1));
                return true;
            }
            else
            {
                Console.WriteLine($"Error: no free {item.SlotType} slot available.");
                return false;
            }
        }

        return false;
    }
    public List<Item> GetEquippedItem(EquipmentSlots slot)
    {
        List<Item> results = new List<Item>();
        if (EquipedItems.TryGetValue(slot, out var itemStack))
        {
            if (itemStack == null)
                return null;
            else
            {
                foreach (ItemStack stack in itemStack)
                {
                    results.Add(stack.Item);
                }
            }
        }
        return results;
    }
    public bool HasItem(Item item, int minQuantity = 1)
    {
        var slot = Inventory.FirstOrDefault(s => s.Value.Item.ID == item.ID);
        return slot.Value != null && slot.Value.stackSize >= minQuantity;
    }
    public ItemStack? GetSlot(int slot)
    {
        return Inventory.TryGetValue(slot, out var stack) ? stack : null;
    }
}
