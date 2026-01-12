using System;
using MoonPioneer.InputSystem.Services.Input;
using MoonPioneer.Player.Configs;
using UniRx;
using UnityEngine;
using Zenject;

namespace MoonPioneer.Player.Components
{
  public class PlayerMovement : MonoBehaviour
  {
    private IInputService _inputService;
    private PlayerMovementConfig _setup;

    [SerializeField] private Rigidbody _rigidbody;

    private IDisposable _disposable;

    [Inject]
    private void Construct(IInputService inputService, PlayerMovementConfig setup)
    {
      _inputService = inputService;
      _setup = setup;
    }
    
    private void Start()
    {
      _disposable = Observable.EveryUpdate().Subscribe(_ =>
      {
        var direction = new Vector3(_inputService.Axis.x, 0, _inputService.Axis.y);

        Rotate(direction);
        Move(direction);
      });
    }

    private void Move(Vector3 direction)
    {
      Vector3 offset = direction.normalized * direction.magnitude * _setup.MovementSpeed * Time.deltaTime;
      _rigidbody.MovePosition(_rigidbody.position + offset);
    }

    private void Rotate(Vector3 direction)
    {
      if (!(Vector3.Angle(transform.forward, direction) > 0)) return;

      Vector3 newDirection = Vector3.RotateTowards(transform.forward, direction, _setup.RotateSpeed, 0);
      _rigidbody.rotation = Quaternion.LookRotation(newDirection);
    }

    public void OnDestroy()
    {
      _disposable?.Dispose();
    }
  }
}