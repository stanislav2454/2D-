using System.Collections;
using TMPro;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private float _spawnInterval = 2f;
    [SerializeField] private EnemySpawnData[] _spawnData;
    [SerializeField] private Enemy _enemyPrefab;
    [SerializeField] public Transform parent;
    [SerializeField] private int _maxEnemies = 10;
    [SerializeField] private TextMeshProUGUI _spawnedCountText;

    private int _spawnedCount = 0;
    private WaitForSeconds _spawnWait;
    private Coroutine _spawningCoroutine;

    private void Awake()
    {
        _spawnWait = new WaitForSeconds(_spawnInterval);
    }

    private void OnValidate()
    {
        if (_enemyPrefab == null)
            Debug.LogWarning("Enemy prefab is not set!", this);
        if (_spawnData.Length == 0)
            Debug.LogWarning("Spawn data array is empty!", this);
    }

    private void OnEnable() =>
        _spawningCoroutine = StartCoroutine(SpawnRoutine());

    private void OnDisable()
    {
        if (_spawningCoroutine != null)
            StopCoroutine(_spawningCoroutine);
        _spawningCoroutine = null;
    }

    private void OnDestroy()
    {
        if (_spawningCoroutine != null)
            StopCoroutine(_spawningCoroutine);
    }

    private void HandleEnemyDeath()
    {
        _spawnedCount--;
        _spawnedCountText.text = $"Enemies: {_spawnedCount}";
    }

    private IEnumerator SpawnRoutine()
    {
        while (enabled && _spawnedCount < _maxEnemies)
        {
            yield return _spawnWait;
            SpawnEnemy();
            _spawnedCount++;
            _spawnedCountText.text = $"Enemies: {_spawnedCount}";
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
        if (spawnData.SpawnPoint == null || _enemyPrefab == null)
            return;

        Enemy newEnemy = Instantiate(_enemyPrefab, spawnData.SpawnPoint.Position, spawnData.SpawnPoint.Rotation, parent);

        newEnemy.Dead += HandleEnemyDeath;

        if (spawnData.RandomPath != null && newEnemy.Movement != null)
            newEnemy.Movement.Initialize(spawnData.RandomPath);
    }
}