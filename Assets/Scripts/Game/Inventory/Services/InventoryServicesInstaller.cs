using MoonPioneer.Game.Inventory.Services.InventoryService;
using Zenject;

namespace MoonPioneer.Game.Inventory.Services
{
  public class InventoryServicesInstaller : MonoInstaller
  {
    public override void InstallBindings()
    {
      Container.Bind<IPlayerInventoryService>().To<PlayerInventoryService>().AsSingle();
    }
  }
}