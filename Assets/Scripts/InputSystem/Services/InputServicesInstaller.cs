using MoonPioneer.InputSystem.Services.Input;
using Zenject;

namespace MoonPioneer.InputSystem.Services
{
  public class InputServicesInstaller : MonoInstaller
  {
    public override void InstallBindings()
    {
#if UNITY_EDITOR
      Container.Bind<IInputService>().To<StandaloneInputService>().AsSingle();
#else
      Container.Bind<IInputService>().To<MobileInputService>().AsSingle();
#endif
    }
  }
}