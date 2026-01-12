using UnityEngine;

namespace MoonPioneer.Game.Items.Services.ItemRepositories
{
  public interface IItemRepositoryService
  {
    Item Instantiate(ItemTypeId itemTypeId, Vector3 position, Quaternion rotation);
    void Destroy(Item item);
    void Release(ItemTypeId itemTypeId);
  }
}