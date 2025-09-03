using System;
using UnityEngine;

[DisallowMultipleComponent, RequireComponent(typeof(EnemyMover), typeof(EnemyHealth), typeof(EnemyAI))]
public class Enemy : MonoBehaviour
{
    private EnemyMover _mover;
    private EnemyHealth _health;
    private EnemyAI _ai;

    public EnemyMover Movement { get; private set; }

    public event Action<Enemy> OnEnemyDeath;

    private void Awake()
    {
        _mover = GetComponent<EnemyMover>();
        Movement = _mover;
        _health = GetComponent<EnemyHealth>();
        _ai = GetComponent<EnemyAI>();
    }

    private void OnEnable() =>
        _health.OnDeath += HandleDeath;

    private void OnDisable() =>
        _health.OnDeath -= HandleDeath;

    public void ResetEnemy()
    {
        transform.rotation = Quaternion.identity;
        _health.ResetHealth();
        _ai.ResetAI();
    }

    public void Initialize(EnemyPath path)
    {
        if (path != null)
        {
            _mover.Initialize(path);
            _ai.SetPatrolPath(path);
        }
    }

    public void SetPool(EnemyPool pool) =>
        _health.SetPool(pool);

    private void HandleDeath() =>
        OnEnemyDeath?.Invoke(this);
}