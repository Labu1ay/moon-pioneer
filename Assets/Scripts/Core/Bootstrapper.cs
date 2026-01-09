using MoonPioneer.Core.Services.SceneLoader;
using UnityEngine;
using Zenject;

namespace MoonPioneer.Core
{
  public class Bootstrapper : MonoBehaviour
  {
    [Inject] private readonly ISceneLoaderService _sceneLoaderService;

    private void Start()
    {
      Application.targetFrameRate = 60;
      
      _sceneLoaderService.Load(Constants.GAME_SCENE_NAME);
    }
  }
}