using Zenject;

namespace MoonPioneer.Player.Factory
{
  public class PlayerFactoryInstaller : MonoInstaller
  {
    public override void InstallBindings()
    {
      Container.Bind<IPlayerFactory>().To<PlayerFactory>().AsSingle();
    }
  }
}