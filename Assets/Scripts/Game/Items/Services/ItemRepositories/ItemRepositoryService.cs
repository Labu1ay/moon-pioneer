using System;
using MoonPioneer.Core.Services.AssetProvider;
using MoonPioneer.Utils;
using UnityEngine;
using Zenject;

namespace MoonPioneer.Game.Items.Services.ItemRepositories
{
  public class ItemRepositoryService : IItemRepositoryService, IInitializable
  {
    private const string BRONZE_ITEM_PATH = "Items/BronzeItem";

    private readonly IAssetService _assetService;
    
    private ObjectPool<Item> _bronzePool;
    
    private Item _bronzeItemPrefab;
    
    
    private Transform _content;

    public ItemRepositoryService(IAssetService assetService)
    {
      _assetService = assetService;
    }

    public void Initialize()
    {
      _content = new GameObject("BronzeItems").transform;

      _bronzeItemPrefab = _assetService.Load<Item>(BRONZE_ITEM_PATH);
      _bronzePool = new ObjectPool<Item>(_bronzeItemPrefab, _content, 10);
    }

    public Item Instantiate(ItemTypeId itemTypeId, Vector3 position, Quaternion rotation)
    {
      switch (itemTypeId)
      {
        case ItemTypeId.Bronze: return _bronzePool.Instantiate(_bronzeItemPrefab, _content, position, rotation);
      }
      
      throw new Exception($"[ItemRepositoryService: Instantiate] Item with typeId: {itemTypeId} is not found");
    }

    public void Destroy(Item item)
    {
      switch (item.ItemTypeId)
      {
        case ItemTypeId.Bronze: _bronzePool.Destroy(item); return;
      }
      
      throw new Exception($"[ItemRepositoryService: Destroy] Item with typeId: {item.ItemTypeId} is not found");
    }

    public void Release(ItemTypeId itemTypeId)
    {
      switch (itemTypeId)
      {
        case ItemTypeId.Bronze: _bronzePool.Release(); return;
      }
      
      throw new Exception($"[ItemRepositoryService: Release] Item with typeId: {itemTypeId} is not found");
    }
  }
}