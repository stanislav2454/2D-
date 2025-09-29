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


    // Vampirism Ability Settings
    [field: Header("Vampirism Settings")]
    [field: SerializeField, Min(0)] public float VampirismDuration { get; private set; } = 6f;
    [field: SerializeField, Min(0)] public float VampirismCooldown { get; private set; } = 4f;
    [field: SerializeField, Min(0.01f)] public float VampirismTickInterval { get; private set; } = 0.2f;
    [field: SerializeField, Min(0)] public int VampirismDamagePerTick { get; private set; } = 2;
    [field: SerializeField, Range(0f, 1f)] public float VampirismHealRatio { get; private set; } = 0.5f;
}