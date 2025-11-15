using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using The_Game;


public interface InventoryEngine
{
    bool AddItem(IInventory actor, Item item, int quantity = 1);
    bool RemoveItem(IInventory actor, Item item, int quantity = 1);
    bool EquipItem(IInventory actor, Item item);
    List<Item> GetEquippedItem(IInventory actor, EquipmentSlots slot);
    bool HasItem(IInventory actor, Item Item, int minQuantity = 1);
    ItemStackComponent? GetSlot(IInventory actor, int slot);
}
