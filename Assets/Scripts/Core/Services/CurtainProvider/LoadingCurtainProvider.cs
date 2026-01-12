using System;
using Cysharp.Threading.Tasks;
using MoonPioneer.Core.Services.AssetProvider;
using MoonPioneer.UI;
using Zenject;

namespace MoonPioneer.Core.Services.CurtainProvider
{
  public class LoadingCurtainProvider : ILoadingCurtainProvider
  {
    private const string CURTAIN_PATH = "LoadingCurtain";
    
    private readonly IAssetService _assetService;
    private readonly DiContainer _diContainer;
    
    private LoadingCurtain _curtain;

    public LoadingCurtainProvider(IAssetService assetService, DiContainer diContainer)
    {
      _assetService = assetService;
      _diContainer = diContainer;
    }

    public async UniTaskVoid Show(float fadeDuration = 1f, Action action = null)
    {
      _curtain ??= await _assetService.Instantiate<LoadingCurtain>(CURTAIN_PATH, _diContainer);
      _curtain.Show(fadeDuration, action);
    }

    public void Hide(float duration = 1f, Action action = null)
    {
      _curtain?.Hide(duration, action);
    }
  }
}