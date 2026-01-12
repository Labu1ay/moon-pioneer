using System;
using DG.Tweening;
using UniRx;
using UnityEngine;

namespace MoonPioneer.Game.Items
{
  public class Item : MonoBehaviour
  {
    private const float MOVE_DURATION = 0.5f;
    private const float SPEED = 25f;
    
    [field: SerializeField] public ItemTypeId ItemTypeId { get; private set; }
    
    private Sequence _sequence;
    private IDisposable _disposable;

    public void MoveTo(Vector3 position, Quaternion rotation, Action callback = null)
    {
      _sequence = DOTween.Sequence();
      _sequence.Append(transform.DOMove(position, MOVE_DURATION));
      _sequence.Join(transform.DORotateQuaternion(rotation, MOVE_DURATION));
      _sequence.OnComplete(() => callback?.Invoke());
    }

    public void ContinuousMoveTo(Transform point, Action callback = null)
    {
      _sequence?.Kill();
      
      _disposable = Observable.EveryUpdate().Subscribe(_ =>
      {
        transform.position = Vector3.MoveTowards(transform.position, point.position, SPEED * Time.deltaTime);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, point.rotation, SPEED * Time.deltaTime);

        if (Vector3.Distance(transform.position, point.position) <= Mathf.Epsilon)
        {
          transform.rotation = point.rotation;
          callback?.Invoke();
          _disposable?.Dispose();
        }
      });
    }

    private void OnDestroy()
    {
      _sequence?.Kill();
      _disposable?.Dispose();
    }
  }
}