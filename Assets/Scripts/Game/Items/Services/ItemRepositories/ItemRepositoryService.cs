using System.Collections.Generic;
using MoonPioneer.Core.Services.AssetProvider;
using MoonPioneer.Utils;
using UnityEngine;
using Zenject;

namespace MoonPioneer.Game.Items.Services.ItemRepositories
{
  public class ItemRepositoryService : IItemRepositoryService, IInitializable
  {
    private const string ITEMS_FORMAT_PATH = "Items/{0}Item";
    
    private readonly IAssetService _assetService;
    
    private readonly ItemTypeId[] _itemTypes =
    {
      ItemTypeId.Bronze, 
      ItemTypeId.Silver, 
      ItemTypeId.Gold
    };
    
    private readonly Dictionary<ItemTypeId, ObjectPool<Item>> _pools = new();
    private readonly Dictionary<ItemTypeId, Item> _itemPrefabs = new();
    
    public Transform Content { get; private set; }

    public ItemRepositoryService(IAssetService assetService) => 
      _assetService = assetService;

    public void Initialize()
    {
      Content = new GameObject("ItemsContent").transform;

      foreach (var type in _itemTypes)
      {
        var prefab = _assetService.Load<Item>(string.Format(ITEMS_FORMAT_PATH, type));
        _pools[type] = new ObjectPool<Item>(prefab, Content, 10);
        _itemPrefabs[type] = prefab;
      }
    }

    public Item Instantiate(ItemTypeId itemTypeId, Vector3 position, Quaternion rotation) =>
      _pools.TryGetValue(itemTypeId, out var pool) 
        ? pool.Instantiate(_itemPrefabs[itemTypeId], Content, position, rotation) 
        : throw new KeyNotFoundException($"Pool for {itemTypeId} not found");

    public void Destroy(Item item)
    {
      if (_pools.TryGetValue(item.ItemTypeId, out var pool))
      {
        pool.Destroy(item);
        return;
      }
      
      throw new KeyNotFoundException($"Pool for {item.ItemTypeId} not found");
    }

    public void Release(ItemTypeId itemType)
    {
      if (_pools.TryGetValue(itemType, out var pool))
      {
        pool.Release();
        return;
      }
      
      throw new KeyNotFoundException($"Pool for {itemType} not found");
    }
  }
}