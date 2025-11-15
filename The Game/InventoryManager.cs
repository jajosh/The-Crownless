using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using The_Game;


// put ALL methods here (logic!).
internal class InventoryManager : InventoryEngine
{
    public bool AddItem(IInventory actor, Item item, int quantity = 1)
    {
        bool check = false;
        // Check if the item is already in the inventory
        while (quantity > 0)
        {
            foreach (var itemstack in actor.Inventory.Inventory)
            {
                if (itemstack.Value.Item == item && item.MaxStack < itemstack.Value.StackSize)
                {
                    while (item.MaxStack < itemstack.Value.StackSize)
                    {
                        itemstack.Value.StackSize++;
                        quantity--;
                        check = true;
                    }
                }
            }
        }
        if (actor.Inventory.Inventory.Count < actor.Inventory.Capacity)
        {
            int slot = GetEmptySlot(actor);
            ItemStackComponent itemToAdd = new ItemStackComponent(item);
            actor.Inventory.Inventory.Add(slot, itemToAdd);
            check = true;
        }
        return check;
       // Needs to add the item
    }
    public bool RemoveItem(IInventory actor, Item item, int quantity = 1)
    {
        return false;
    }
    public bool EquipItem(IInventory actor, Item item)
    {
        

        return false;
    }
    public List<Item> GetEquippedItem(IInventory actor, EquipmentSlots slot)
    {
        List<Item> results = new List<Item>();
        return results;
    }
    // Checks if the player has a particular item 
    public bool HasItem(IInventory actor, Item item, int minQuantity = 1)
    {
        ItemStackComponent itemToCheck = new ItemStackComponent(item, minQuantity);
        if (actor.Inventory.Inventory.ContainsValue(itemToCheck))
        {
            return true;
        }
        return false;
    }
    // Gets the itemstact of a particular slot
    public ItemStackComponent? GetSlot(IInventory actor, int slot)
    {
        ItemStackComponent itemstack = actor.Inventory.Inventory[slot];
        return itemstack;
    }
    // Gets the first empty slot. Returns 0 for no empty slots
    public int GetEmptySlot(IInventory actor)
    {
        ShrinkInventory(actor);
        foreach (var itemStack in actor.Inventory.Inventory)
        {
            if (itemStack.Value == null)
                return itemStack.Key;
        }
        return 0;
    }
    // shifts the inventory values to the first open keys/ Shifting everything down.
    // Fix for CS8625: Cannot convert null literal to non-nullable reference type.
    // Instead of assigning null, remove the key from the dictionary.
    public void ShrinkInventory(IInventory actor)
    {
        int previousKey = -1;
        foreach (var key in new List<int>(actor.Inventory.Inventory.Keys)) // Avoid modifying during iteration
        {
            if (actor.Inventory.Inventory[key] == null && previousKey != -1)
            {
                actor.Inventory.Inventory[key] = actor.Inventory.Inventory[previousKey];
                actor.Inventory.Inventory.Remove(previousKey); // Remove the previous key instead of setting to null
            }
            previousKey = key;
        }
    }
}


