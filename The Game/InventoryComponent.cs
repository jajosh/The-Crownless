using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class ItemStackComponent
{
    public Item Item { get; set; }
    public int StackSize { get; set; }
    public ItemStackComponent(Item item, int quantity = 1)
    {
        Item = item;
        StackSize = quantity;
    }
    public ItemStackComponent()
    {
        Item = new Item();
    }
}
public class InventoryComponent
{
    // Json loading key
    public Dictionary<int, int>? InventoryKey { get; set; }
    public List<int> ArmorKey { get; set; }
    //Shit you are holding
    public int Capacity { get; private set; }
    public Dictionary<int, ItemStackComponent> Inventory { get; set; }
    public Dictionary<EquipmentSlots, List<ItemStackComponent>> EquipedItems { get; set; }


    public InventoryComponent()
    {
        Capacity = 1;
        Inventory = new Dictionary<int, ItemStackComponent>();
        Dictionary<EquipmentSlots, List<ItemStackComponent>> EquipedItems = new Dictionary<EquipmentSlots, List<ItemStackComponent>>
        {
            { EquipmentSlots.Head, new List<ItemStackComponent>() },
            { EquipmentSlots.Chest, new List<ItemStackComponent>() },
            { EquipmentSlots.Waist, new List<ItemStackComponent>() },
            { EquipmentSlots.Legs, new List<ItemStackComponent>() },
            { EquipmentSlots.Feet, new List<ItemStackComponent>() },
            { EquipmentSlots.Arms, new List<ItemStackComponent>() },
            { EquipmentSlots.Hands, new List<ItemStackComponent>() },
            { EquipmentSlots.Wrists, new List<ItemStackComponent>() },
            { EquipmentSlots.Shoulders, new List<ItemStackComponent>() },
            { EquipmentSlots.Neck, new List<ItemStackComponent>() },

            { EquipmentSlots.Ring, new List<ItemStackComponent>() },       // multiple allowed
            { EquipmentSlots.Bracelet, new List<ItemStackComponent>() },   // multiple allowed
            { EquipmentSlots.Trinket, new List<ItemStackComponent>() },    // multiple allowed

            { EquipmentSlots.MainHand, new List<ItemStackComponent>() },
            { EquipmentSlots.OffHand, new List<ItemStackComponent>() },
            { EquipmentSlots.Back, new List<ItemStackComponent>() },
            { EquipmentSlots.LowerBack, new List<ItemStackComponent>() },
            { EquipmentSlots.Side, new List<ItemStackComponent>() },

            { EquipmentSlots.Cap, new List<ItemStackComponent>() },
            { EquipmentSlots.Belt, new List<ItemStackComponent>() },
            { EquipmentSlots.Tabard, new List<ItemStackComponent>() },
        };

    }
}
