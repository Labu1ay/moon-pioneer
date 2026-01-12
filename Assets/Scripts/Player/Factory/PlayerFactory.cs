using Cysharp.Threading.Tasks;
using MoonPioneer.Core.Services.AssetProvider;
using UnityEngine;
using Zenject;

namespace MoonPioneer.Player.Factory
{
  public class PlayerFactory : IPlayerFactory
  {
    private const string PLAYER_PATH = "Player";
    
    [Inject] private readonly IAssetService _assetService;
    [Inject] private readonly DiContainer _diContainer;

    public GameObject Player { get; private set; }
    
    public async UniTask<GameObject> CreatePlayer(Vector3 position, Quaternion rotation)
    {
      Player = await _assetService.Instantiate(PLAYER_PATH, _diContainer, position, rotation);
      return Player;
    }

    public async UniTask<GameObject> GetPlayerAsync()
    {
      await UniTask.WaitWhile(() => Player == null);
      return Player;
    }
  }
}