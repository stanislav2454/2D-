using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EnemyPool))]
public class EnemySpawner : MonoBehaviour
{
    private const string TagPlayer = "Player";
    private readonly HashSet<Enemy> _activeEnemies = new HashSet<Enemy>();

    [SerializeField] private float _spawnInterval = 2f;
    [SerializeField] private int _maxEnemies = 10;
    [SerializeField] private EnemySpawnData[] _spawnData;
    [SerializeField] private EnemyPool _enemyPool;
    [SerializeField] private Transform parent;
    [SerializeField] private EnemyCounterUI _enemyCounterUI;
    [SerializeField] private Transform _player;

    private WaitForSeconds _spawnWait;
    private Coroutine _spawningCoroutine;

    private void Awake()
    {
        _spawnWait = new WaitForSeconds(_spawnInterval);

        if (TryGetComponent(out _enemyPool)) ;

        if (_player == null)
            _player = GameObject.FindGameObjectWithTag(TagPlayer)?.transform;
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

        if (_player == null)
            Debug.LogWarning("Player Transform is not set!", this);
    }

    private void OnEnable() =>
        _spawningCoroutine = StartCoroutine(SpawnRoutine());

    private void OnDisable()
    {
        if (_spawningCoroutine != null)
            StopCoroutine(_spawningCoroutine);

        _spawningCoroutine = null;
        UnsubscribeFromAllEnemies();
    }

    private void OnDestroy()
    {
        if (_spawningCoroutine != null)
            StopCoroutine(_spawningCoroutine);
    }

    private IEnumerator SpawnRoutine()
    {
        while (enabled)
        {
            if (_activeEnemies.Count < _maxEnemies)
            {
                yield return _spawnWait;
                SpawnEnemy();
                _enemyCounterUI.AddEnemy();
            }
            else
            {
                yield return null;
            }
        }
    }

    private void SpawnEnemy()
    {
        if (_spawnData.Length == 0)
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

        enemy.transform.SetPositionAndRotation(spawnData.SpawnPoint.Position, Quaternion.identity);
        enemy.transform.SetParent(parent);
        enemy.SetTarget(_player);
        enemy.OnEnemyDeath += HandleEnemyDeath;
        _activeEnemies.Add(enemy);

        EnemyPath randomPath = spawnData.RandomPath;

        if (spawnData.RandomPath != null && enemy.Movement != null)
            enemy.Initialize(randomPath);
    }

    private void HandleEnemyDeath(Enemy enemy)
    {
        enemy.OnEnemyDeath -= HandleEnemyDeath;
        _activeEnemies.Remove(enemy);
        _enemyCounterUI.RemoveEnemy();
        _enemyPool.ReleaseEnemy(enemy);
    }

    private void UnsubscribeFromAllEnemies()
    {
        foreach (Enemy enemy in _activeEnemies)
            if (enemy != null)
            {
                enemy.OnEnemyDeath -= HandleEnemyDeath;
                _enemyPool.ReleaseEnemy(enemy);
            }

        _activeEnemies.Clear();
        _enemyCounterUI.ResetCounter();
    }
}