using System;
using Cysharp.Threading.Tasks;
using MoonPioneer.Core.Services.AssetProvider;
using MoonPioneer.Core.Services.CurtainProvider;
using MoonPioneer.UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace MoonPioneer.Core.Services.SceneLoader
{
  public class SceneLoaderService : ISceneLoaderService
  {
    private readonly ILoadingCurtainProvider _loadingCurtainProvider;

    public string ActiveSceneName => SceneManager.GetActiveScene().name;

    public SceneLoaderService(ILoadingCurtainProvider loadingCurtainProvider)
    {
      _loadingCurtainProvider = loadingCurtainProvider;
    }

    public void Load(string name, Action callback = null) => 
      _loadingCurtainProvider.Show( action: () => LoadSceneAsync(name, callback).Forget());

    private async UniTaskVoid LoadSceneAsync(string nextScene, Action callback = null)
    {
      AsyncOperation waitNextScene = SceneManager.LoadSceneAsync(nextScene);

      await UniTask.WaitUntil(() => waitNextScene.isDone);

      _loadingCurtainProvider.Hide();
      callback?.Invoke();
    }
  }
}