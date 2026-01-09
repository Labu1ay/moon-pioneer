using MoonPioneer.UI.ScreenSystem.Screens;
using MoonPioneer.UI.ScreenSystem.Services.ScreenManager;
using UnityEngine;
using Zenject;

namespace MoonPioneer.Game
{
  public class GameBootstrapper : MonoBehaviour
  {
    private IScreenService _screenService;

    [Inject]
    private void Construct(IScreenService screenService)
    {
      _screenService = screenService;
    }
    
    private void Start()
    {
      // _gameStateService.SetGameState(GameStateType.GAME);
      // _playerFactory.CreatePlayer(Vector3.zero, Quaternion.identity);
      // //_enemyFactory.CreateEnemy(new Vector3(0f, 0f, 4f), Quaternion.identity);
      _screenService.ShowScreen<GameScreen>();
    }
  }
}