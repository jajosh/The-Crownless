using System;
using System.Collections.Generic;

namespace The_Game
{
    // For Item data
    public interface IItem
    {
        string? AsciiKey { get; set; }
        string Name { get; set; }
        int ID { get; set; }
        EquipmentSlots SlotType { get; set; }
        int SpawnChance { get; set; }
        int MaxStack { get; set; }
        int? Price { get; set; }
        bool CanStore { get; set; }
        List<string> EncumbranceErrorMessages { get; set; }
        // ... all other props (WeaponData, etc.)
        object Clone(); // For deep copy if needed
    }

    // For finding items (decouples from FileManager)
    public interface IItemRepository
    {
        IItem? FindByName(string name, bool ignoreCase = true);
        IItem? FindById(int id);
        List<IItem> AllItems { get; } // Expose the list
    }

    // For managing a single stack
    public interface IItemStackManager
    {
        void Add(int amount);
        bool Remove(int amount);
        ItemStackComponent Clone();
        int CurrentStackSize { get; }
        IItem? CurrentItem { get; }
    }

    // For inventory/equipment
    public interface IInventory
    {
        int Capacity { get; set; }
        bool AddItem(IItem item, int quantity = 1);
        bool RemoveItem(int slotId, int quantity = 1);
        bool EquipItem(IItem item, EquipmentSlots slot);
        bool UnequipItem(EquipmentSlots slot, int index = 0);
        // ... more methods as needed
    }
}