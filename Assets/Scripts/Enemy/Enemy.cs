using UnityEngine;

[DisallowMultipleComponent, RequireComponent(typeof(EnemyMover), typeof(EnemyHealth), typeof(EnemyAI))]
public class Enemy : MonoBehaviour
{
    private EnemyMover _mover;
    private EnemyHealth _health;
    private EnemyAI _ai;

    public EnemyMover Movement => _mover;

    //public event Action<Enemy> Died;
    //private EnemyPool _pool;

    private void Awake()
    {
        _mover = GetComponent<EnemyMover>();
        _health = GetComponent<EnemyHealth>();
        _ai = GetComponent<EnemyAI>();
    }

    private void OnEnable() =>
            _health.Died += OnDead;

    private void OnDisable() =>
            _health.Died -= OnDead;

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

    private EnemyPool _pool;// Сделать: перенести поле

    public void SetPool(EnemyPool pool) =>
        _pool = pool;

    private void OnDead(BaseHealth health) 
    {
        _pool?.ReleaseEnemy(health); 
        gameObject.SetActive(false);
    }
}