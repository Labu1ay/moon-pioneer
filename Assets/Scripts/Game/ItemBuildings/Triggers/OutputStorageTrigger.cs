using System;
using MoonPioneer.Game.Inventory.Services.InventoryService;
using MoonPioneer.Game.Items;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using Zenject;

namespace MoonPioneer.Game.ItemBuildings.Triggers
{
  public class OutputStorageTrigger : MonoBehaviour
  {
    private IPlayerInventoryService _playerInventoryService;
    
    [SerializeField] private OutputStorage _outputStorage;
    [SerializeField] private Collider _collider;

    private float _timer;

    private CompositeDisposable _disposables = new CompositeDisposable();
    private IDisposable _disposable;

    [Inject]
    private void Construct(IPlayerInventoryService playerInventoryService)
    {
      _playerInventoryService = playerInventoryService;
    }

    private void Start()
    {
      TriggerEnterSubscribe();
      TriggerExitSubscribe();
    }

    private void TriggerEnterSubscribe()
    {
      _collider.OnTriggerEnterAsObservable().Subscribe(other =>
      {
        if (other.gameObject.layer == LayerMask.NameToLayer(Constants.PLAYER_LAYER))
        {
          if(_playerInventoryService.InventoryIsFull()) return;
          CollectItemsSubscribe();
        }
      }).AddTo(_disposables);
    }

    private void TriggerExitSubscribe()
    {
      _collider.OnTriggerExitAsObservable().Subscribe(other =>
      {
        if (other.gameObject.layer == LayerMask.NameToLayer(Constants.PLAYER_LAYER)) 
          _disposable?.Dispose();
        
      }).AddTo(_disposables);
    }

    private void CollectItemsSubscribe()
    {
      _disposable = Observable.EveryUpdate().Subscribe(_ =>
      {
        _timer += Time.deltaTime;

        if (_timer >= Constants.COLLECT_DELAY)
        {
          _timer = 0f;

          if (_outputStorage.TryGetItem(out Item item)) 
            _playerInventoryService.AddItem(item);
          
          if(_playerInventoryService.InventoryIsFull())
            _disposable?.Dispose();
        }
      });
    }

    private void OnDestroy()
    {
      _disposables?.Clear();
      _disposable?.Dispose();
    }
  }
}