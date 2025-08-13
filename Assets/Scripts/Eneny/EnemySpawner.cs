using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private float _spawnInterval = 2f;
    [SerializeField] private EnemySpawnPoint[] _spawnPoints;
    [SerializeField] private Enemy _enemyPrefab;

    private WaitForSeconds _spawnWait;
    private Coroutine _spawningCoroutine;

    private void Awake()
    {
        _spawnWait = new WaitForSeconds(_spawnInterval);
    }

    private void OnEnable() =>
        _spawningCoroutine = StartCoroutine(SpawnRoutine());

    private void OnDisable()
    {
        if (_spawningCoroutine != null)
            StopCoroutine(_spawningCoroutine);
    }

    private IEnumerator SpawnRoutine()
    {
        while (enabled)
        {
            yield return _spawnWait;
            TrySpawnEnemy();
        }
    }

    private void TrySpawnEnemy()
    {
        if (_spawnPoints.Length == 0)
            return;

        EnemySpawnPoint spawnPoint = GetRandomSpawnPoint();
        SpawnEnemyAtPoint(spawnPoint);
    }

    private EnemySpawnPoint GetRandomSpawnPoint()
    {
        int randomIndex = Random.Range(0, _spawnPoints.Length);
        return _spawnPoints[randomIndex];
    }

    private void SpawnEnemyAtPoint(EnemySpawnPoint spawnPoint)
    {
        Enemy newEnemy = Instantiate(_enemyPrefab, spawnPoint.Position, spawnPoint.Rotation);

        if (spawnPoint.Path != null && newEnemy.Movement != null)
            newEnemy.Movement.Initialize(spawnPoint.Path);
    }
}