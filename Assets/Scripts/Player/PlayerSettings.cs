using UnityEngine;

[CreateAssetMenu(fileName = "PlayerSettings", menuName = "Settings/PlayerSettings")]
public class PlayerSettings : ScriptableObject
{
    [field: SerializeField, Min(0)] public float WalkSpeed { get; private set; } = 6f;
    [field: SerializeField, Min(0)] public float CrawlSpeed { get; private set; } = 3f;
    [field: SerializeField, Min(0)] public float JumpForce { get; private set; } = 12f;
    [field: SerializeField, Min(0)] public int MaxHealth { get; private set; } = 100;
    [field: SerializeField, Min(0)] public float AttackCooldown { get; private set; } = 0.5f;
    [field: SerializeField, Min(0)] public int AttackDamage { get; private set; } = 1;
}