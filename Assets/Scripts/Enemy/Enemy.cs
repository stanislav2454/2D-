using UnityEngine;

[DisallowMultipleComponent, RequireComponent(typeof(EnemyMover), typeof(EnemyHealth), typeof(EnemyAI))]
public class Enemy : MonoBehaviour
{
    private EnemyMover _mover;
    private EnemyHealth _health;
    private EnemyAI _ai;

    public EnemyMover Movement => _mover;

    private void Awake()
    {
        _mover = GetComponent<EnemyMover>();
        _health = GetComponent<EnemyHealth>();
        _ai = GetComponent<EnemyAI>();
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