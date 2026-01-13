using System;
using Cysharp.Threading.Tasks;
using MoonPioneer.Game.Items;
using MoonPioneer.Game.Tooltip.Services.TooltipSystem;
using MoonPioneer.UI.Gameplay;
using UniRx;
using UnityEngine;
using Zenject;

namespace MoonPioneer.Game.ItemBuildings
{
  public class ItemBuilding : MonoBehaviour
  {
    [SerializeField] private float _createItemDuration = 1f;
    [SerializeField] private ItemTypeId _createdItemType;
    [SerializeField] private BuildingCreateStatusUI _buildingCreateStatusUI;

    private ITooltipService _tooltipService;
    
    private float _timer;
    private bool _isCreating;
    private string _tooltipFormat;

    private InputStorage _inputStorage;
    private OutputStorage _outputStorage;
    
    private IDisposable _disposable;

    [Inject]
    private void Construct(ITooltipService tooltipService)
    {
      _tooltipService = tooltipService;
    }
    
    private void Start()
    {
      _tooltipFormat = $"Здание, которое производит: {_createdItemType} остановило работу по причине: " + "{0}";
      
      _inputStorage = GetComponent<InputStorage>();
      _outputStorage = GetComponent<OutputStorage>();

      _outputStorage.ItemGived += ItemGived;
      if(_inputStorage) _inputStorage.ItemAdded += ItemGived;
      
      CreateItem();
    }

    private void ItemGived()
    {
      if(_isCreating == false)
        CreateItem();
    }

    private void CreateItem()
    {
      if (_outputStorage.StorageIsFull())
      {
        StopCreating( "заполненность исходящего склада");
        return;
      }
      
      if (_inputStorage && _inputStorage.StorageIsEmpty())
      {
        StopCreating("нет необходимых ресурсов на входящем складе");
        return;
      }
      
      _buildingCreateStatusUI.Show();
      _isCreating = true;
      
      if(_inputStorage) _inputStorage.GetItem();

      _disposable = Observable.EveryUpdate().Subscribe(_ =>
      {
        _timer += Time.deltaTime;
        _buildingCreateStatusUI.SetProgress(_timer, _createItemDuration);

        if (_timer >= _createItemDuration)
        {
          _outputStorage.AddItem(_createdItemType);
          _disposable?.Dispose();
          _timer = 0f;
          
          CreateItem();
        }
      });
    }

    private void StopCreating(string reason)
    {
      if(_isCreating)
        _tooltipService.TryShowTemporaryTooltip(string.Format(_tooltipFormat, reason), durationSeconds: 5f);
        
      _buildingCreateStatusUI.Hide().Forget();
      _isCreating = false;
    }

    private void OnDestroy()
    {
      _outputStorage.ItemGived -= ItemGived;
      if(_inputStorage) _inputStorage.ItemAdded -= ItemGived;
    }
  }
}