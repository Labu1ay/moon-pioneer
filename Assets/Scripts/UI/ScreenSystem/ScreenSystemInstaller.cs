using System.Collections.Generic;
using MoonPioneer.UI.ScreenSystem.Factory;
using MoonPioneer.UI.ScreenSystem.Services.ScreenManager;
using UnityEngine;
using Zenject;

namespace MoonPioneer.UI.ScreenSystem
{
  public class ScreenSystemInstaller : MonoInstaller
  {
    [SerializeField] private List<Screen> _screens;
    
    public override void InstallBindings()
    {
      Container
        .Bind<IScreenFactory>()
        .To<ScreenFactory>()
        .AsSingle()
        .WithArguments(_screens, transform);

      Container.Bind<IScreenService>().To<ScreenService>().AsSingle();
    }
  }
}