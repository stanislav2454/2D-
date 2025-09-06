using UnityEngine;
using UnityEngine.Pool;

public class EnemyPool : MonoBehaviour
{
    [Header("Pool Settings")]
    [SerializeField] private int _defaultCapacity = 10;
    [SerializeField] [Range(0, 20)] private int _maxSize = 20;
    [SerializeField] private bool _collectionCheck = true;
    [SerializeField] private BaseHealth _enemyPrefab;

    private IObjectPool<BaseHealth> _pool;
    public int MaxSize => _maxSize;
    public IObjectPool<BaseHealth> Pool => _pool;

    private void Awake()
    {
        _pool = new ObjectPool<BaseHealth>(
            CreatePooledObject,
            OnGetFromPool,
            OnReturnedToPool,
            OnDestroyPooledObject,
            _collectionCheck,
            _defaultCapacity,
            _maxSize);
    }

    private void OnDestroy()
    {
        _pool?.Clear();
    }

    public BaseHealth GetEnemy() =>
         _pool.Get();

    public void ReleaseEnemy(BaseHealth enemy) =>
        _pool.Release(enemy);

    private BaseHealth CreatePooledObject()
    {
        BaseHealth enemy = Instantiate(_enemyPrefab, transform);
        enemy.gameObject.SetActive(false);

        enemy.Died += OnEnemyDied;

        return enemy;
    }

    private void OnEnemyDied(BaseHealth enemy)
    {
        if (enemy == null || enemy.gameObject.activeInHierarchy == false)
            return;

        enemy.Died -= OnEnemyDied;

        ReleaseEnemy(enemy);
    }

    private void OnGetFromPool(BaseHealth enemy)
    {
        enemy.gameObject.SetActive(true);

        enemy.Died += OnEnemyDied;
        enemy.GetComponent<Enemy>()?.ResetEnemy();
    }


    private void OnReturnedToPool(BaseHealth enemy)
    {
        if (enemy != null)
        {
            enemy.Died -= OnEnemyDied;
            enemy.gameObject.SetActive(false);
        }
    }

    private void OnDestroyPooledObject(BaseHealth enemy)
    {
        enemy.Died -= OnEnemyDied;
        Destroy(enemy.gameObject);
    }
}