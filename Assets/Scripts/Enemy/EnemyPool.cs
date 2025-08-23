using UnityEngine;
using UnityEngine.Pool;

public class EnemyPool : MonoBehaviour
{
    [Header("Pool Settings")]
    [SerializeField] private int _defaultCapacity = 10;
    [SerializeField] private int _maxSize = 20;
    [SerializeField] private bool _collectionCheck = true;
    [SerializeField] private Enemy _enemyPrefab;

    private IObjectPool<GameObject> _pool;

    public IObjectPool<GameObject> Pool => _pool;

    private void Awake()
    {
        _pool = new ObjectPool<GameObject>(
            CreatePooledObject,
            OnGetFromPool,
            OnReturnedToPool,
            OnDestroyPooledObject,
            _collectionCheck,
            _defaultCapacity,
            _maxSize);
    }

    public GameObject GetEnemy() =>
         _pool.Get();

    public void ReleaseEnemy(GameObject enemy) =>
        _pool.Release(enemy);

    private GameObject CreatePooledObject()
    {
        Enemy enemy = Instantiate(_enemyPrefab, transform);
        enemy.gameObject.SetActive(false);
        enemy.SetPool(this);
        return enemy.gameObject;
    }

    private void OnGetFromPool(GameObject enemy)
    {
        enemy.SetActive(true);
        enemy.GetComponent<Enemy>().ResetEnemy();
    }

    private void OnReturnedToPool(GameObject enemy) =>
        enemy.SetActive(false);

    private void OnDestroyPooledObject(GameObject enemy) =>
        Destroy(enemy);
}