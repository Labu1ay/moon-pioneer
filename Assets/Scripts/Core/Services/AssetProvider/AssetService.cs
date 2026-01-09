using Cysharp.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using Zenject;

namespace MoonPioneer.Core.Services.AssetProvider
{
  public class AssetService : IAssetService
  {
    public async UniTask<GameObject> Instantiate(string path, DiContainer diContainer, Vector3 position = default, Quaternion rotation = default, Transform parent = null)
    {
      var request = Resources.LoadAsync<GameObject>(path);
      await request;

      var prefab = request.asset;
      return diContainer.InstantiatePrefab((GameObject)prefab, position, rotation, parent);
    }
    
    public async UniTask<T> Instantiate<T>(string path, DiContainer container, Vector3 position = default, Quaternion rotation = default, Transform parent = null)
    {
      var request = Resources.LoadAsync<GameObject>(path);
      await request;

      var prefab = request.asset as GameObject;
      var instance = container.InstantiatePrefabForComponent<T>(prefab,position, rotation, parent);
      return instance;
    }

    public T Load<T>(string path)
    {
      var prefab = Resources.LoadAsync(path).asset;
      return prefab.GetComponent<T>();
    }

    public async UniTaskVoid UnloadUnusedAssets() =>
      await Resources.UnloadUnusedAssets();
  }
}