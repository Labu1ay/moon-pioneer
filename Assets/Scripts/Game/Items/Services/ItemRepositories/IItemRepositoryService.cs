using UnityEngine;

namespace MoonPioneer.Game.Items.Services.ItemRepositories
{
  public interface IItemRepositoryService
  {
    Transform Content { get; }
    Item Instantiate(ItemTypeId itemTypeId, Vector3 position, Quaternion rotation);
    void Destroy(Item item);
    void Release(ItemTypeId itemTypeId);
  }
}