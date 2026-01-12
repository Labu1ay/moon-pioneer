using MoonPioneer.Game.Inventory.Services.InventoryService;
using UnityEngine;
using Zenject;

namespace MoonPioneer.Game.Inventory
{
  public class PlayerInventory : MonoBehaviour
  {
    private IPlayerInventoryService _playerInventoryService;
    
    [SerializeField] private Transform _inventoryItemPoint;

    [Inject]
    private void Construct(IPlayerInventoryService playerInventoryService)
    {
      _playerInventoryService = playerInventoryService;
    }
    
    private void Start()
    {
      _playerInventoryService.SetInventoryItemPoint(_inventoryItemPoint);
    }
  }
}