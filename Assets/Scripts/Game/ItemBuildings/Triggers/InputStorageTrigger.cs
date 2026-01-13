using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using MoonPioneer.Game.Outline;
using MoonPioneer.InputSystem.Services.Input;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using Zenject;

namespace MoonPioneer.Game.ItemBuildings.Triggers
{
  public class InputStorageTrigger : MonoBehaviour
  {
    private IInputService _inputService;
    
    [SerializeField] private InputStorage _inputStorage;
    [SerializeField] private Collider _collider;
    [SerializeField] private InteractableOutline _outline;

    private float _timer;

    private CancellationTokenSource _cts;

    private CompositeDisposable _disposables = new CompositeDisposable();
    private IDisposable _disposable;

    [Inject]
    private void Construct(IInputService inputService)
    {
      _inputService = inputService;
    }

    private void Start()
    {
      TriggerEnterSubscribe();
      TriggerExitSubscribe();
    }

    private void TriggerEnterSubscribe()
    {
      _collider.OnTriggerEnterAsObservable().Subscribe(async other =>
      {
        _outline.ShowOutline();
        
        _cts = new CancellationTokenSource();
        bool cancelHandler = await UniTask.WaitUntil(() => _inputService.Axis is { x: 0, y: 0 }, cancellationToken: _cts.Token)
          .SuppressCancellationThrow();
        
        if (cancelHandler) return;
        
        if (other.gameObject.layer == LayerMask.NameToLayer(Constants.PLAYER_LAYER))
        {
          if(_inputStorage.StorageIsFull()) return;
          AddItemsSubscribe();
        }
      }).AddTo(_disposables);
    }

    private void TriggerExitSubscribe()
    {
      _collider.OnTriggerExitAsObservable().Subscribe(other =>
      {
        Cancel();
        
        _outline.HideOutline();
        
        if (other.gameObject.layer == LayerMask.NameToLayer(Constants.PLAYER_LAYER)) 
          _disposable?.Dispose();
        
      }).AddTo(_disposables);
    }

    private void AddItemsSubscribe()
    {
      _disposable = Observable.EveryUpdate().Subscribe(_ =>
      {
        _timer += Time.deltaTime;

        if (_timer >= Constants.COLLECT_DELAY)
        {
          _timer = 0f;

          _inputStorage.TryAddItem();
          
          if(_inputStorage.StorageIsFull())
            _disposable?.Dispose();
        }
      });
    }
    
    private void Cancel()
    {
      _cts?.Cancel();
      _cts?.Dispose();
      _cts = null;
    }

    private void OnDestroy()
    {
      _disposables?.Clear();
      _disposable?.Dispose();
      
      Cancel();
    }
  }
}