using UnityEngine;

namespace MoonPioneer.Player.Configs
{
  [CreateAssetMenu(fileName = "PlayerMovementConfig", menuName = "Configs/PlayerMovementConfig")]
  public class PlayerMovementConfig : ScriptableObject
  {
    [field: SerializeField] public float MovementSpeed { get; private set; } = 5f;
    [field: SerializeField] public float RotateSpeed { get; private set; } = 1f;
  }
}