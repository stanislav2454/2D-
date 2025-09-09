using UnityEngine;

[CreateAssetMenu(fileName = "EnemySettings", menuName = "Settings/EnemySettings")]
public class EnemySettings : ScriptableObject
{
    [Header("Movement Settings")]
    [SerializeField, Min(0)] private float _walkSpeed = 4f;
    [SerializeField, Min(0)] private float _chaseSpeed = 6f;
    [SerializeField, Min(0)] private float _acceleration = 12f;

    [Header("AI Settings")]
    [SerializeField, Min(0)] private float _detectionRadius = 4f;
    [SerializeField, Min(0)] private float _attackRange = 0.12f;
    [SerializeField, Min(0)] private float _attackCooldown = 3f;

    [Header("Combat Settings")]
    [SerializeField, Min(0)] private int _attackDamage = 2;
    [SerializeField, Min(0)] private int _maxHealth = 30;

    [Header("Visual Settings")]
    [SerializeField, Range(0.1f, 2f)] private float _enemyScale = 1f;

    public float WalkSpeed => _walkSpeed;
    public float ChaseSpeed => _chaseSpeed;
    public float Acceleration => _acceleration;
    public float DetectionRadius => _detectionRadius;
    public float AttackRange => _attackRange;
    public float AttackCooldown => _attackCooldown;
    public int AttackDamage => _attackDamage;
    public int MaxHealth => _maxHealth;
    public float EnemyScale => _enemyScale;
}