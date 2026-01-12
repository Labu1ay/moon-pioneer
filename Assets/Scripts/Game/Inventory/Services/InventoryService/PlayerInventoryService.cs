using System.Collections.Generic;
using System.Linq;
using MoonPioneer.Game.Items;
using Unity.VisualScripting;
using UnityEngine;

namespace MoonPioneer.Game.Inventory.Services.InventoryService
{
  public class PlayerInventoryService : IPlayerInventoryService
  {
    private const int CAPACITY = 10;
    private const float Y_OFFSET = 0.51f;

    private Transform _inventoryItemPoint;

    List<Item> _items = new List<Item>(16);

    public void SetInventoryItemPoint(Transform inventoryItemPoint) =>
      _inventoryItemPoint = inventoryItemPoint;

    public bool InventoryIsFull()
    {
      return _items.Count == CAPACITY;
    }

    public void AddItem(Item item)
    {
      _items.Add(item);

      var targetPoint = CreateTargetPoint(item);

      item.ContinuousMoveTo(targetPoint, () =>
      {
        item.transform.parent = _inventoryItemPoint.parent;
        Object.Destroy(targetPoint.GameObject());
      });
    }

    public bool TryGetItem(ItemTypeId itemTypeId, out Item item)
    {
      item = _items.FirstOrDefault(i => i.ItemTypeId == itemTypeId);
      return item != null;
    }

    public void OptimizeInventory()
    {
      foreach (var item in _items) 
        item.MoveTo(GetNeedItemPosition(item), _inventoryItemPoint.rotation);
    }

    private Vector3 GetNeedItemPosition(Item item)
    {
      return new Vector3(
        _inventoryItemPoint.position.x,
        _inventoryItemPoint.position.y + _items.IndexOf(item) * Y_OFFSET,
        _inventoryItemPoint.position.z);
    }
    
    private Transform CreateTargetPoint(Item item)
    {
      var point = new GameObject().transform;
      point.parent = _inventoryItemPoint;
      point.position = GetNeedItemPosition(item);
      point.localRotation = Quaternion.identity;
      return point;
    }
  }
}