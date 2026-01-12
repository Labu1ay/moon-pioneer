using MoonPioneer.Game.Items;
using UnityEngine;

namespace MoonPioneer.Game.Inventory.Services.InventoryService
{
  public interface IPlayerInventoryService
  {
    void SetInventoryItemPoint(Transform inventoryItemPoint);
    bool InventoryIsFull();
    void AddItem(Item item);
    bool TryGetItem(ItemTypeId itemTypeId, out Item item);
    void RemoveItem(Item item);
    void OptimizeInventory();
  }
}