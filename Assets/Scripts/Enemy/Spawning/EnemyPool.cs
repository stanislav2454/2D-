using UnityEngine;
using UnityEngine.Pool;

public class EnemyPool : MonoBehaviour
{
    [Header("Pool Settings")]
    [SerializeField] private EnemySpawnSettings _settings;

    [Header("Prefab")]
    [SerializeField] private BaseHealth _enemyPrefab;

    private IObjectPool<BaseHealth> _pool;
    private int _maxSize;

    public int MaxSize => _maxSize;
    public IObjectPool<BaseHealth> Pool => _pool;

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

    public BaseHealth GetEnemy() =>
         _pool?.Get();

    public void ReleaseEnemy(BaseHealth enemy) =>
        _pool?.Release(enemy);

    private void CreatePool()
    {
        int capacity = _settings?.defaultCapacity ?? 10;
        int maxSize = _settings?.maxPoolSize ?? 20;
        bool check = _settings?.collectionCheck ?? true;

        _pool = new ObjectPool<BaseHealth>(
            CreatePooledObject,
            OnGetFromPool,
            OnReturnedToPool,
            OnDestroyPooledObject,
            check,
            capacity,
            maxSize);
    }

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
        if (enemy != null)
        {
            enemy.Died -= OnEnemyDied;
            Destroy(enemy.gameObject);
        }
    }
}