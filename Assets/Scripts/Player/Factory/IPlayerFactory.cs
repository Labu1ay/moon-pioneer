using Cysharp.Threading.Tasks;
using UnityEngine;

namespace MoonPioneer.Player.Factory
{
  public interface IPlayerFactory
  {
    GameObject Player { get; }
    UniTask<GameObject> CreatePlayer(Vector3 position, Quaternion rotation);
    UniTask<GameObject> GetPlayerAsync();
  }
}