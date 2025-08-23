using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(EnemyPool))]
public class EnemySpawner : MonoBehaviour
{
    private const string UIText = "Enemies:";
    private readonly HashSet<Enemy> _activeEnemies = new HashSet<Enemy>();

    [SerializeField] private float _spawnInterval = 2f;
    [SerializeField] private int _maxEnemies = 10;
    [SerializeField] private EnemySpawnData[] _spawnData;
    [SerializeField] private EnemyPool _enemyPool;
    [SerializeField] private Transform parent;
    [SerializeField] private TextMeshProUGUI _spawnedCountText;

    private int _spawnedCount = 0;
    private WaitForSeconds _spawnWait;
    private Coroutine _spawningCoroutine;

    private void Awake()
    {
        _spawnWait = new WaitForSeconds(_spawnInterval);

        if (TryGetComponent(out _enemyPool)) ;
    }

    private void OnValidate()
    {
        if (_enemyPool == null)
            Debug.LogWarning("Enemy pool is not set!", this);

        if (_spawnData.Length == 0)
            Debug.LogWarning("Spawn data array is empty!", this);

        if (parent == null)
            Debug.LogWarning("Transform parent is not set!", this);

        if (_spawnedCountText == null)
            Debug.LogWarning("TextMeshProUGUI is not set!", this);
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
            if (_spawnedCount < _maxEnemies)
            {
                yield return _spawnWait;
                SpawnEnemy();
                _spawnedCount++;
                UpdateCounterText(UIText, _spawnedCount);
            }
            else
            {
                yield return null;
            }
        }
    }

    private void UpdateCounterText(string textUI, int spawnedCount) =>
        _spawnedCountText.text = $"{textUI} {spawnedCount}";

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

        GameObject newEnemy = _enemyPool.GetEnemy();

        newEnemy.transform.SetPositionAndRotation(spawnData.SpawnPoint.Position, Quaternion.identity);
        newEnemy.transform.SetParent(parent);

        Enemy enemyComponent = newEnemy.GetComponent<Enemy>();

        if (enemyComponent != null)
        {
            enemyComponent.OnEnemyDeath += HandleEnemyDeath;
            _activeEnemies.Add(enemyComponent);

            EnemyPath randomPath = spawnData.RandomPath;

            if (spawnData.RandomPath != null && enemyComponent.Movement != null)
                enemyComponent.Initialize(randomPath);
        }
    }

    private void HandleEnemyDeath(Enemy enemy)
    {
        enemy.OnEnemyDeath -= HandleEnemyDeath;
        _activeEnemies.Remove(enemy);
        _spawnedCount--;
        UpdateCounterText(UIText, _spawnedCount);
    }

    private void UnsubscribeFromAllEnemies()
    {
        foreach (Enemy enemy in _activeEnemies)
            if (enemy != null)
                enemy.OnEnemyDeath -= HandleEnemyDeath;

        _activeEnemies.Clear();
    }
}