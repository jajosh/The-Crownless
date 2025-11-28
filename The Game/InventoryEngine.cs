using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using The_Game;


public interface InventoryEngine
{
    bool AddItem(IInventory actor, ItemObject item, int quantity = 1);
    bool RemoveItem(IInventory actor, ItemObject item, int quantity = 1);
    bool EquipItem(IInventory actor, ItemObject item);
    List<ItemObject> GetEquippedItem(IInventory actor, EquipmentSlots slot);
    bool HasItem(IInventory actor, ItemObject Item, int minQuantity = 1);
    ItemStackComponent? GetSlot(IInventory actor, int slot);
}
