using System.Collections.Generic;
using MoonPioneer.Game.Items;
using MoonPioneer.Game.Items.Services.ItemRepositories;
using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;

namespace MoonPioneer.Game.ItemBuildings
{
  public class OutputStorage : MonoBehaviour
  {
    private const float X_OFFSET = 1.1f;
    private const float Z_OFFSET = 0.55f;
    private const int COLUMN_SIZE = 4;
    
    private IItemRepositoryService _itemRepositoryService;
    
    [field: SerializeField] public int Capacity { get; private set; } = 10;
    
    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private Transform _firstItemStoragePoint;

    private Stack<Item> _items = new Stack<Item>();

    [Inject]
    private void Construct(IItemRepositoryService itemRepositoryService)
    {
      _itemRepositoryService = itemRepositoryService;
    }

    [Button]
    public void AddItem(ItemTypeId itemTypeId)
    {
      var item = _itemRepositoryService.Instantiate(itemTypeId, _spawnPoint.position, Quaternion.identity);

      item.MoveTo(GetStoragePosition(), Quaternion.identity);
      _items.Push(item);
    }

    public bool StorageIsFull()
    {
      return _items.Count == Capacity;
    }

    private Vector3 GetStoragePosition()
    {
      var currentColumn = _items.Count / COLUMN_SIZE;
      
     return new Vector3(
        _firstItemStoragePoint.position.x + _items.Count % COLUMN_SIZE * X_OFFSET,
        _firstItemStoragePoint.position.y,
        _firstItemStoragePoint.position.z - currentColumn * Z_OFFSET);
    }

    [Button]
    private void RemoveItem()
    {
      var item = _items.Pop();
      _itemRepositoryService.Destroy(item);
    }
  }
}