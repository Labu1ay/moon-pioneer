using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace MoonPioneer.Core.Services.AssetProvider
{
  public interface IAssetService
  {
    UniTask<GameObject> Instantiate(string path, DiContainer diContainer, Vector3 position = default, Quaternion rotation = default, Transform parent = null);
    UniTask<T> Instantiate<T>(string path, DiContainer container, Vector3 position = default, Quaternion rotation = default, Transform parent = null);
    GameObject Instantiate(GameObject prefab, DiContainer diContainer, Vector3 position = default, Quaternion rotation = default, Transform parent = null);
    T Instantiate<T>(T prefab, DiContainer diContainer, Vector3 position = default, Quaternion rotation = default, Transform parent = null) where T : Component;
    T Instantiate<T>(T prefab, DiContainer diContainer, Transform parent = null) where T : Component;
    T Load<T>(string path);
    public UniTaskVoid UnloadUnusedAssets();
  }
}