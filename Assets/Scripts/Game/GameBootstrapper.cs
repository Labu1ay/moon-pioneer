using MoonPioneer.Core.Services.CurtainProvider;
using MoonPioneer.Player.Factory;
using MoonPioneer.UI.ScreenSystem.Screens;
using MoonPioneer.UI.ScreenSystem.Services.ScreenManager;
using UnityEngine;
using Zenject;

namespace MoonPioneer.Game
{
  public class GameBootstrapper : MonoBehaviour
  {
    private IScreenService _screenService;
    private IPlayerFactory _playerFactory;

    [Inject]
    private void Construct(IScreenService screenService, IPlayerFactory playerFactory)
    {
      _screenService = screenService;
      _playerFactory = playerFactory;
    }
    
    private void Start()
    {
      _playerFactory.CreatePlayer(Vector3.zero, Quaternion.identity);
      _screenService.ShowScreen<GameScreen>();
      
    }
  }
}