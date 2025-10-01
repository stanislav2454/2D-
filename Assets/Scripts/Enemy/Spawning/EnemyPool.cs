using UnityEngine;
using UnityEngine.Pool;

public class EnemyPool : MonoBehaviour
{
    [Header("Pool Settings reference")]
    [SerializeField] private EnemySpawnSettings _settings;

    [Space(3)]
    [Header("Prefab")]
    [SerializeField] private Enemy _enemyPrefab;

    private IObjectPool<Enemy> _pool;
    private int _maxSize;

    public int MaxSize => _maxSize;
    public IObjectPool<Enemy> Pool => _pool;

    private void Awake()
    {
        Initialize();
    }

    private void OnValidate()
    {
        if (_settings != null)
            if (_settings.defaultCapacity > _settings.maxPoolSize)
                _settings.defaultCapacity = _settings.maxPoolSize;

        if (_enemyPrefab == null)
            Debug.LogWarning("Enemy prefab is not set!", this);
    }

    private void OnDestroy()
    {
        _pool?.Clear();
    }

    public void ApplySettings(EnemySpawnSettings settings)
    {
        if (settings != null)
        {
            _settings = settings;

            if (_pool != null)
            {
                _pool.Clear();
                CreatePool();
            }
        }
    }

    public void Initialize()
    {
        if (_pool == null)
            CreatePool();
    }

    public Enemy GetEnemy() =>
         _pool?.Get();

    public void ReleaseEnemy(Enemy enemy) =>
        _pool?.Release(enemy);

    private void CreatePool()
    {
        int capacity = _settings?.defaultCapacity ?? 10;
        _maxSize = _settings?.maxPoolSize ?? 20;
        bool check = _settings?.collectionCheck ?? true;

        _pool = new ObjectPool<Enemy>(
            CreatePooledObject,
            OnGetFromPool,
            OnReturnedToPool,
            OnDestroyPooledObject,
            check,
            capacity,
            _maxSize);
    }

    private Enemy CreatePooledObject()
    {
        Enemy enemy = Instantiate(_enemyPrefab, transform);
        enemy.gameObject.SetActive(false);

        enemy.Died += OnEnemyDied;

        return enemy;
    }

    private void OnEnemyDied(Enemy enemy)
    {
        if (enemy == null || enemy.gameObject.activeInHierarchy == false)
            return;

        enemy.Died -= OnEnemyDied;
        ReleaseEnemy(enemy.GetComponent<Enemy>());
    }

    private void OnGetFromPool(Enemy enemy)
    {
        enemy.gameObject.SetActive(true);

        enemy.Died += OnEnemyDied;

        enemy.ResetEnemy();
    }


    private void OnReturnedToPool(Enemy enemy)
    {
        if (enemy != null)
        {
            enemy.Died -= OnEnemyDied;

            enemy.gameObject.SetActive(false);
        }
    }

    private void OnDestroyPooledObject(Enemy enemy)
    {
        if (enemy != null)
        {
            enemy.Died -= OnEnemyDied;

            Destroy(enemy.gameObject);
        }
    }
}