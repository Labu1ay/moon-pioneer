using MoonPioneer.Game.Items.Services.ItemRepositories;
using Zenject;

namespace MoonPioneer.Game.Items.Services
{
  public class ItemServicesInstaller : MonoInstaller
  {
    public override void InstallBindings()
    {
      Container.BindInterfacesAndSelfTo<ItemRepositoryService>().AsSingle();
    }
  }
}