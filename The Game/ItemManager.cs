using System;
using System.Collections.Generic;

public class ItemManager
{
    Item item = new Item();
    ItemStackComponent ItemStack = new ItemStackComponent();
    public static List<Item> Items { get; set; }
    public ItemManager()
    {
    }

    public static Item FindItemByName(string NameToFind)
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

        Item? foundItem = Items.Find(item =>
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
    public static Item FindItemByID(int IDToFind)
    {
        List<Item> itemList = Items;
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