using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EnemyPool))]
public class EnemySpawner : MonoBehaviour
{
    private const int NumberEnemyToSpawn = 1;

    private readonly HashSet<Enemy> _activeEnemies = new HashSet<Enemy>();

    [SerializeField] private float _spawnInterval = 2f;
    [SerializeField] [Range(0, 20)] private int _numberEnemiesToSpawn = 10;
    [SerializeField] private EnemySpawnData[] _spawnData;
    [SerializeField] private EnemyPool _enemyPool;
    [SerializeField] private Transform parent;
    [SerializeField] private EnemyCounterUI _enemyCounterUI;

    private int _spawnedCount = 0;
    private WaitForSeconds _spawnWait;
    private Coroutine _spawningCoroutine;

    private void Awake()
    {
        _spawnWait = new WaitForSeconds(_spawnInterval);

        if (TryGetComponent(out _enemyPool)) ;

        _numberEnemiesToSpawn = Mathf.Min(_numberEnemiesToSpawn, _enemyPool.MaxSize);
    }

    private void OnValidate()
    {
        if (_enemyPool == null)
            Debug.LogWarning("Enemy pool is not set!", this);

        if (_spawnData.Length == 0)
            Debug.LogWarning("Spawn data array is empty!", this);

        if (parent == null)
            Debug.LogWarning("Transform parent is not set!", this);

        if (_enemyCounterUI == null)
            Debug.LogWarning("EnemyCounterUI is not set!", this);
    }

    private void OnEnable()
    {
        if (_enemyPool != null)
            _spawningCoroutine = StartCoroutine(SpawnRoutine());
    }

    private void OnDisable()
    {
        if (_spawningCoroutine != null)
        {
            StopCoroutine(_spawningCoroutine);
            _spawningCoroutine = null;
        }

        UnsubscribeFromAllEnemies();
    }

    private void OnDestroy()
    {
        if (_spawningCoroutine != null)
            StopCoroutine(_spawningCoroutine);

        UnsubscribeFromAllEnemies();
    }

    private IEnumerator SpawnRoutine()
    {
        while (enabled)
        {
            if (_spawnedCount < _numberEnemiesToSpawn)
            {
                yield return _spawnWait;
                SpawnEnemy();
                _spawnedCount += NumberEnemyToSpawn;
                _enemyCounterUI.AddEnemy(NumberEnemyToSpawn);
            }
            else
            {
                yield return null;
            }
        }
    }

    private void SpawnEnemy()
    {
        if (_spawnData.Length == 0 || _enemyPool == null)
            return;

        EnemySpawnData spawnData = GetRandomSpawnData();
        SpawnEnemyAtPoint(spawnData);
    }

    private EnemySpawnData GetRandomSpawnData()
    {
        int randomIndex = Random.Range(0, _spawnData.Length);
        return _spawnData[randomIndex];
    }

    private void SpawnEnemyAtPoint(EnemySpawnData spawnData)
    {
        if (spawnData.SpawnPoint == null)
            return;

        Enemy enemy = _enemyPool.GetEnemy();

        if (enemy != null)
        {
            enemy.transform.SetPositionAndRotation(spawnData.SpawnPoint.Position, Quaternion.identity);
            enemy.transform.SetParent(parent);
            enemy.EnemyDied += HandleEnemyDeath;
            _activeEnemies.Add(enemy);

            EnemyPath randomPath = spawnData.RandomPath;

            if (spawnData.RandomPath != null && enemy.Movement != null)
                enemy.Initialize(randomPath);
        }
    }

    private void HandleEnemyDeath(Enemy enemy)
    {
        if (enemy != null)
        {
            enemy.EnemyDied -= HandleEnemyDeath;
            _activeEnemies.Remove(enemy);
            _spawnedCount -= NumberEnemyToSpawn;
            _enemyCounterUI.RemoveEnemy(NumberEnemyToSpawn);
        }
    }

    private void UnsubscribeFromAllEnemies()
    {
        foreach (Enemy enemy in _activeEnemies)
            if (enemy != null)
                enemy.EnemyDied -= HandleEnemyDeath;

        _activeEnemies.Clear();
        _spawnedCount = 0;

        if (_enemyCounterUI != null)
            _enemyCounterUI.ResetCounter();
    }
}