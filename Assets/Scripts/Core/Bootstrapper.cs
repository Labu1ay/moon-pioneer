using MoonPioneer.Core.Services.SceneLoader;
using UnityEngine;
using Zenject;

namespace MoonPioneer.Core
{
  public class Bootstrapper : MonoBehaviour
  {
    private ISceneLoaderService _sceneLoaderService;

    [Inject]
    private void Construct(ISceneLoaderService sceneLoaderService)
    {
      _sceneLoaderService = sceneLoaderService;
    }

    private void Start()
    {
      Application.targetFrameRate = 60;
      
      _sceneLoaderService.Load(Constants.GAME_SCENE_NAME);
    }
  }
}