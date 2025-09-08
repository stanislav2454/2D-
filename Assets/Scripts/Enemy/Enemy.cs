using UnityEngine;

[DisallowMultipleComponent, RequireComponent(typeof(EnemyMover), typeof(EnemyHealth), typeof(EnemyAI))]
public class Enemy : MonoBehaviour
{
    private EnemyMover _mover;
    private EnemyHealth _health;
    private EnemyAI _ai;

    public event System.Action<Enemy> EnemyDied; //  Событие смерти врага
                                                 // и спецом название с EnemyDied, для наглядности - потом уберу
    public EnemyMover Movement => _mover;

    private void Awake()
    {
        _mover = GetComponent<EnemyMover>();
        _health = GetComponent<EnemyHealth>();
        _ai = GetComponent<EnemyAI>();

        _health.Died += OnHealthDied;
    }

    private void OnDestroy()
    {
        if (_health != null)
            _health.Died -= OnHealthDied;
    }

    private void OnHealthDied(BaseHealth health)
    {
        // Преобразуем событие смерти здоровья в событие смерти врага
        EnemyDied?.Invoke(this);
    }

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
}