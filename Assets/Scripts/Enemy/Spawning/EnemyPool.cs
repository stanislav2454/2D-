using UnityEngine;
using UnityEngine.Pool;

public class EnemyPool : MonoBehaviour
{
    [Header("Pool Settings")]
    [SerializeField] private int _defaultCapacity = 10;
    [Range(0, 20)]
    [SerializeField] private int _maxSize = 20;
    [SerializeField] private bool _collectionCheck = true;
    [SerializeField] private Enemy _enemyPrefab;

    private IObjectPool<Enemy> _pool;

    public int MaxSize => _maxSize;
    public IObjectPool<Enemy> Pool => _pool;

    private void Awake()
    {
        _pool = new ObjectPool<Enemy>(
            CreatePooledObject,
            OnGetFromPool,
            OnReturnedToPool,
            OnDestroyPooledObject,
            _collectionCheck,
            _defaultCapacity,
            _maxSize);
    }

    public Enemy GetEnemy() =>
         _pool.Get();

    public void ReleaseEnemy(Enemy enemy) =>
        _pool.Release(enemy);

    private Enemy CreatePooledObject()
    {
        Enemy enemy = Instantiate(_enemyPrefab, transform);
        enemy.gameObject.SetActive(false);
        enemy.SetPool(this);
        return enemy;
    }

    private void OnGetFromPool(Enemy enemy)
    {
        enemy.gameObject.SetActive(true);
        enemy.GetComponent<Enemy>().ResetEnemy();
    }

    private void OnReturnedToPool(Enemy enemy) =>
        enemy.gameObject.SetActive(false);

    private void OnDestroyPooledObject(Enemy enemy) =>
        Destroy(enemy);
}