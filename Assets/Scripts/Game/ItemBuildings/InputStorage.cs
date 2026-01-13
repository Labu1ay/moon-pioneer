using System;
using System.Collections.Generic;
using MoonPioneer.Game.Inventory.Services.InventoryService;
using MoonPioneer.Game.Items;
using MoonPioneer.Game.Items.Services.ItemRepositories;
using MoonPioneer.Utils.Extensions;
using Sirenix.OdinInspector;
using Unity.Mathematics;
using UnityEngine;
using Zenject;

namespace MoonPioneer.Game.ItemBuildings
{
  public class InputStorage : MonoBehaviour
  {
    private IItemRepositoryService _itemRepositoryService;
    private IPlayerInventoryService _playerInventoryService;
    
    [field: SerializeField] public int Capacity { get; private set; } = 10;
    
    [SerializeField] private Transform _buildingPoint;
    [SerializeField] private Transform _firstItemStoragePoint;
    
    [SerializeField] private ItemTypeId[] _neededItemTypes;

    private Stack<Item> _items = new (16);

    public event Action ItemAdded;

    [Inject]
    private void Construct(IItemRepositoryService itemRepositoryService, IPlayerInventoryService playerInventoryService)
    {
      _itemRepositoryService = itemRepositoryService;
      _playerInventoryService = playerInventoryService;
    }

    public bool TryAddItem()
    {
      List<Item> items = new List<Item>();
      
      foreach (ItemTypeId typeId in _neededItemTypes)
      {
        if (!_playerInventoryService.TryGetItem(typeId, out Item item))
        {
          items.Clear();
          return false;
        }
        
        items.Add(item);
      }

      foreach (Item item in items)
      {
        item.transform.SetParent(_itemRepositoryService.Content);
        item.MoveTo(GetStoragePosition(), Quaternion.identity, () => ItemAdded?.Invoke());
        
        _items.Push(item);
        _playerInventoryService.RemoveItem(item);
      }
      
      _playerInventoryService.OptimizeInventory();
      
      return true;
    }

    public void GetItem()
    {
      for (var i = 0; i < _neededItemTypes.Length; i++)
      {
        bool result = _items.TryPop(out Item item);
      
        if(result)
          item.MoveTo(_buildingPoint.position, Quaternion.identity, () => _itemRepositoryService.Destroy(item));
      }
    }

    public bool StorageIsFull() => _items.Count >= Capacity;
    public bool StorageIsEmpty() => _items.Count == 0;

    private Vector3 GetStoragePosition()
    {
      var currentColumn = _items.Count / Constants.COLUMN_SIZE;
      
      return new Vector3(
        _firstItemStoragePoint.position.x + _items.Count % Constants.COLUMN_SIZE * Constants.X_OFFSET,
        _firstItemStoragePoint.position.y,
        _firstItemStoragePoint.position.z - currentColumn * Constants.Z_OFFSET);
    }
  }
}