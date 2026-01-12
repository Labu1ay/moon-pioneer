using System;
using Cysharp.Threading.Tasks;
using MoonPioneer.Game.Items;
using MoonPioneer.UI.Gameplay;
using UniRx;
using UnityEngine;

namespace MoonPioneer.Game.ItemBuildings
{
  public class ItemBuilding : MonoBehaviour
  {
    [SerializeField] private float _createItemDuration = 1f;
    [SerializeField] private ItemTypeId _createdItemType;
    [SerializeField] private BuildingCreateStatusUI _buildingCreateStatusUI;

    private float _timer;
    private bool _isCreating;
    
    private OutputStorage _outputStorage;
    private IDisposable _disposable;

    private void Start()
    {
      _outputStorage = GetComponent<OutputStorage>();

      _outputStorage.ItemGived += ItemGived;
      
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
        // log in UI
        _buildingCreateStatusUI.Hide().Forget();
        _isCreating = false;
        return;
      }
      
      _buildingCreateStatusUI.Show();
      _isCreating = true;

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

    private void OnDestroy()
    {
      _outputStorage.ItemGived -= ItemGived;
    }
  }
}