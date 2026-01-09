using System;
using Cysharp.Threading.Tasks;
using MoonPioneer.Core.Services.AssetProvider;
using MoonPioneer.UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace MoonPioneer.Core.Services.SceneLoader
{
  public class SceneLoaderService : ISceneLoaderService, IInitializable
  {
    private const string CURTAIN_PATH = "LoadingCurtain";

    private readonly IAssetService _assetService;
    private readonly DiContainer _diContainer;
    private LoadingCurtain _curtain;

    public string ActiveSceneName => SceneManager.GetActiveScene().name;

    public SceneLoaderService(IAssetService assetService, DiContainer diContainer)
    {
      _assetService = assetService;
      _diContainer = diContainer;
    }

    public async void Initialize()
    {
      _curtain = await _assetService.Instantiate<LoadingCurtain>(CURTAIN_PATH, _diContainer);
      _assetService.UnloadUnusedAssets();
    }

    public async UniTaskVoid Load(string name, Action callback = null)
    {
      await UniTask.WaitUntil(() => _curtain);
      _curtain.Show(() => LoadSceneAsync(name, callback).Forget());
    }

    private async UniTaskVoid LoadSceneAsync(string nextScene, Action callback = null)
    {
      AsyncOperation waitNextScene = SceneManager.LoadSceneAsync(nextScene);

      await UniTask.WaitUntil(() => waitNextScene.isDone);

      _curtain.Hide();
      callback?.Invoke();
    }
  }
}