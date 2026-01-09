using MoonPioneer.Core.Services.AssetProvider;
using MoonPioneer.Core.Services.SceneLoader;
using Zenject;

namespace MoonPioneer.Core.Services
{
  public class CoreServicesInstaller : MonoInstaller
  {
    public override void InstallBindings()
    {
      Container.Bind<IAssetService>().To<AssetService>().AsSingle();
      Container.BindInterfacesAndSelfTo<SceneLoaderService>().AsSingle();
    }
  }
}