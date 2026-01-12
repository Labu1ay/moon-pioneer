using System;
using MoonPioneer.Player.Factory;
using UniRx;
using UnityEngine;
using Zenject;

namespace MoonPioneer.Camera
{
  public class FollowCamera : MonoBehaviour
  {
    private IPlayerFactory _playerFactory;

    private IDisposable _disposable;

    [Inject]
    private void Construct(IPlayerFactory playerFactory)
    {
      _playerFactory = playerFactory;
    }

    private async void Start()
    {
      var targetTransform = (await _playerFactory.GetPlayerAsync()).transform;
      
      _disposable = Observable.EveryUpdate()
        .Subscribe(_ => transform.position = targetTransform.position);
    }

    private void OnDestroy()
    {
      _disposable?.Dispose();
    }
  }
}