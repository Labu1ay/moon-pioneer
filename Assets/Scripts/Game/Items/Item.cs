using System;
using DG.Tweening;
using UnityEngine;

namespace MoonPioneer.Game.Items
{
  public class Item : MonoBehaviour
  {
    private const float MOVE_DURATION = 0.5f;
    
    [field: SerializeField] public ItemTypeId ItemTypeId { get; private set; }
    
    private Sequence _sequence;

    public void MoveTo(Vector3 position, Quaternion rotation, Action callback = null)
    {
      _sequence = DOTween.Sequence();
      _sequence.Append(transform.DOMove(position, MOVE_DURATION));
      _sequence.Join(transform.DORotateQuaternion(rotation, MOVE_DURATION));
      _sequence.OnComplete(() => callback?.Invoke());
    }

    private void OnDestroy()
    {
      _sequence?.Kill();
    }
  }
}