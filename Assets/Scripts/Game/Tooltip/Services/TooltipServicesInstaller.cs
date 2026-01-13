using MoonPioneer.Game.Tooltip.Services.TooltipSystem;
using Zenject;

namespace MoonPioneer.Game.Tooltip.Services
{
  public class TooltipSystemInstaller : MonoInstaller
  {
    public override void InstallBindings()
    {
      Container.BindInterfacesAndSelfTo<TooltipService>().AsSingle();
    }
  }
}