using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EnemyPool))]
public class EnemySpawner : MonoBehaviour
{
    private const int NumberEnemyToSpawn = 1;

    private readonly HashSet<Enemy> _activeEnemies = new HashSet<Enemy>();

    [SerializeField] private EnemySpawnSettings _settings;
    [Space(15)]
    [SerializeField] private EnemySpawnData[] _spawnData;
    [SerializeField] private EnemyPool _enemyPool;
    [SerializeField] private Transform _parent;
    [SerializeField] private EnemyCounterUI _enemyCounterUI;[Space(10)]

    private int _spawnedCount = 0;
    private WaitForSeconds _spawnWait;
    private Coroutine _spawningCoroutine;

    private void Awake()
    {
        if (_settings == null)
        {
            Debug.LogError("EnemySpawnSettings not set! Using default values.", this);
            _settings = ScriptableObject.CreateInstance<EnemySpawnSettings>();
        }

        if (_enemyPool != null)
            _enemyPool.ApplySettings(_settings);

        _spawnWait = new WaitForSeconds(_settings.spawnInterval);

        if (_enemyPool != null)
            _enemyPool.Initialize();
    }

    private void OnValidate()
    {
        if (_settings != null)
            _settings.numberEnemiesToSpawn = Mathf.Min(_settings.numberEnemiesToSpawn, _settings.maxPoolSize);

        if (_enemyPool == null)
            Debug.LogWarning("Enemy pool is not set!", this);

        if (_spawnData.Length == 0)
            Debug.LogWarning("Spawn data array is empty!", this);

        if (_parent == null)
            Debug.LogWarning("Transform parent is not set!", this);

        if (_enemyCounterUI == null)
            Debug.LogWarning("EnemyCounterUI is not set!", this);
    }

    private void OnEnable()
    {
        if (_enemyPool != null && _settings != null)
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
            if (_spawnedCount < _settings.numberEnemiesToSpawn)
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

    private EnemySpawnData GetRandomSpawnData() =>
         _spawnData[Random.Range(0, _spawnData.Length)];

    private void SpawnEnemyAtPoint(EnemySpawnData spawnData)
    {
        if (spawnData.SpawnPoint == null)
            return;

        Enemy enemy = _enemyPool.GetEnemy();

        if (enemy != null && _activeEnemies.Contains(enemy) == false)
        {
            enemy.transform.SetPositionAndRotation(spawnData.SpawnPoint.Position, Quaternion.identity);
            enemy.transform.SetParent(_parent);

            enemy.Died += HandleEnemyDeath;

            _activeEnemies.Add(enemy);

            if (spawnData.RandomPath != null && enemy.Movement != null)
                enemy.Initialize(spawnData.RandomPath);

            enemy.gameObject.SetActive(true);
            enemy.ResetEnemy();
        }
    }

    private void HandleEnemyDeath(Enemy enemy)
    {
        if (enemy != null && _activeEnemies.Contains(enemy))
        {
            enemy.Died -= HandleEnemyDeath;

            _activeEnemies.Remove(enemy);
            _spawnedCount -= NumberEnemyToSpawn;
            _enemyCounterUI?.RemoveEnemy(NumberEnemyToSpawn);
        }
    }

    private void UnsubscribeFromAllEnemies()
    {
        foreach (Enemy enemy in _activeEnemies)
            if (enemy != null)
                enemy.Died -= HandleEnemyDeath;

        _activeEnemies.Clear();
        _spawnedCount = 0;

        _enemyCounterUI?.ResetCounter();
    }
}