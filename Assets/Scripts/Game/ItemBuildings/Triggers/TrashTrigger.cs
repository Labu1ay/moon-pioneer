using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using MoonPioneer.Game.Inventory.Services.InventoryService;
using MoonPioneer.Game.Items;
using MoonPioneer.Game.Items.Services.ItemRepositories;
using MoonPioneer.Game.Outline;
using MoonPioneer.InputSystem.Services.Input;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using Zenject;

namespace MoonPioneer.Game.ItemBuildings.Triggers
{
  public class TrashTrigger : MonoBehaviour
  {
    private const float COLLECT_DELAY = 0.1f;

    private IInputService _inputService;
    private IPlayerInventoryService _playerInventoryService;
    private IItemRepositoryService _itemRepositoryService;

    [SerializeField] private Transform _trashContainerPoint;
    [SerializeField] private Collider _collider;
    [SerializeField] private InteractableOutline _outline;

    private float _timer;

    private CancellationTokenSource _cts;

    private CompositeDisposable _disposables = new CompositeDisposable();
    private IDisposable _disposable;

    [Inject]
    private void Construct(
      IInputService inputService, 
      IPlayerInventoryService playerInventoryService,
      IItemRepositoryService itemRepositoryService)
    {
      _inputService = inputService;
      _playerInventoryService = playerInventoryService;
      _itemRepositoryService = itemRepositoryService;
    }

    private void Start()
    {
      _collider.OnTriggerEnterAsObservable().Subscribe(async other =>
      {
        _outline.ShowOutline();

        _cts = new CancellationTokenSource();
        bool cancelHandler = await UniTask
          .WaitUntil(() => _inputService.Axis is { x: 0, y: 0 }, cancellationToken: _cts.Token)
          .SuppressCancellationThrow();

        if (cancelHandler) return;

        if (other.gameObject.layer == LayerMask.NameToLayer(Constants.PLAYER_LAYER))
        {
          RemoveItemsSubscribe();
        }
      }).AddTo(_disposables);

      _collider.OnTriggerExitAsObservable().Subscribe(other =>
      {
        Cancel();

        _outline.HideOutline();

        if (other.gameObject.layer == LayerMask.NameToLayer(Constants.PLAYER_LAYER))
        {
          _disposable?.Dispose();
        }
      }).AddTo(_disposables);
    }

    private void RemoveItemsSubscribe()
    {
      _disposable = Observable.EveryUpdate().Subscribe(_ =>
      {
        _timer += Time.deltaTime;

        if (_timer >= COLLECT_DELAY)
        {
          _timer = 0f;

          if (_playerInventoryService.TryGetLastItem(out Item item))
          {
            item.transform.SetParent(_itemRepositoryService.Content);
            item.MoveTo(_trashContainerPoint.position, Quaternion.identity, () =>
            {
              _itemRepositoryService.Destroy(item);
            });
        
            _playerInventoryService.RemoveItem(item);
          }
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