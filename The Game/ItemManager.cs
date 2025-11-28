using System;
using System.Collections.Generic;

public class ItemManager
{
    ItemObject item = new ItemObject();
    ItemStackComponent ItemStack = new ItemStackComponent();
    public static List<ItemObject> Items { get; set; }
    public ItemManager()
    {
    }

    public static ItemObject FindItemByName(string NameToFind)
    {
        if (string.IsNullOrWhiteSpace(NameToFind))
        {
            Console.WriteLine("Invalid name.");
            return null;
        }

        if (Items == null)
        {
            Console.WriteLine("FileManager.Items is NULL! Did you call FileManager.LoadData()?");
            return null;
        }

        ItemObject? foundItem = Items.Find(item =>
            !string.IsNullOrEmpty(item.Name) &&
            item.Name.Equals(NameToFind, StringComparison.OrdinalIgnoreCase));

        if (foundItem != null)
        {
            if (SaveGameManager.GameState?.Flags?.IsDebug == true)
                Console.WriteLine($"Item Found: {foundItem.Name}");
            return foundItem;
        }
        else
        {
            if (SaveGameManager.GameState?.Flags?.IsDebug == true)
                Console.WriteLine($"Item Not Found: {NameToFind}");
            return null;
        }
    }
    public static ItemObject FindItemByID(int IDToFind)
    {
        List<ItemObject> itemList = Items;
        ItemObject? foundItem = itemList.Find(item => item.ID == IDToFind);
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

    public void Add(int amount)
    {
        ItemStack.StackSize += amount;
    }

    public bool Remove(int amount)
    {
        if (amount > ItemStack.StackSize) return false;
        ItemStack.StackSize -= amount;
        return true;
    }
    public ItemStackComponent Clone()
    {
        return new ItemStackComponent
        {
            Item = this.ItemStack.Item,
            StackSize = this.ItemStack.StackSize
        };
    }
}