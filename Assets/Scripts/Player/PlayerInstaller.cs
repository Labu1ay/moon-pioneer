using MoonPioneer.Player.Configs;
using UnityEngine;
using Zenject;

namespace MoonPioneer.Player
{
  public class PlayerInstaller : MonoInstaller
  {
    [SerializeField] private PlayerMovementConfig _playerMovementConfig;
    
    public override void InstallBindings()
    {
      Container.Bind<PlayerMovementConfig>().FromInstance(_playerMovementConfig).AsSingle();
    }
  }
}