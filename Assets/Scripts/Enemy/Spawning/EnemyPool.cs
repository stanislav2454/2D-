using UnityEngine;
using UnityEngine.Pool;

public class EnemyPool : MonoBehaviour
{
    [Header("Pool Settings")]
    [SerializeField] private int _defaultCapacity = 10;
    [SerializeField] [Range(0, 20)] private int _maxSize = 20;
    [SerializeField] private bool _collectionCheck = true;
    //[SerializeField] private Enemy _enemyPrefab;
    [SerializeField] private BaseHealth _enemyPrefab;

    //private IObjectPool<Enemy> _pool;
    //private HashSet<Enemy> _activeEnemies = new HashSet<Enemy>();

    //public IObjectPool<Enemy> Pool => _pool;
    private IObjectPool<BaseHealth> _pool;
    public int MaxSize => _maxSize;
    public IObjectPool<BaseHealth> Pool => _pool;

    private void Awake()
    {
        _pool = new ObjectPool<BaseHealth>(
        //_pool = new ObjectPool<Enemy>(
            CreatePooledObject,
            OnGetFromPool,
            OnReturnedToPool,
            OnDestroyPooledObject,
            _collectionCheck,
            _defaultCapacity,
            _maxSize);
    }

    public BaseHealth GetEnemy() =>
         // public Enemy GetEnemy() =>
         _pool.Get();

    public void ReleaseEnemy(BaseHealth enemy) =>
    //public void ReleaseEnemy(Enemy enemy) =>
        _pool.Release(enemy);

    private BaseHealth CreatePooledObject()
    {
        BaseHealth enemy = Instantiate(_enemyPrefab, transform);
        enemy.gameObject.SetActive(false);
        enemy.GetComponent<Enemy>().SetPool(this);
        //enemy.Died += HandleEnemyDeath;

        return enemy;
    }

    private void OnGetFromPool(BaseHealth enemy)
    {
        enemy.gameObject.SetActive(true);
        enemy.GetComponent<Enemy>().ResetEnemy();
        //enemy.ResetEnemy();
    }

    private void OnReturnedToPool(BaseHealth enemy) =>
        enemy.gameObject.SetActive(false);

    private void OnDestroyPooledObject(BaseHealth enemy) =>
        Destroy(enemy);
}