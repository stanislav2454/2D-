using System.Collections;
using UnityEngine;

public class CoinsSpawner : MonoBehaviour
{
    [SerializeField] private float _spawnInterval = 2f;
    [SerializeField] private Coin _coinPrefab;
    [SerializeField] public Transform parent; 
    [SerializeField] private Vector2 _spawnAreaMin;
    [SerializeField] private Vector2 _spawnAreaMax;

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
            SpawnCoin();
        }
    }

    private void SpawnCoin()
    {
        Vector2 spawnPosition = new Vector2(
            Random.Range(_spawnAreaMin.x, _spawnAreaMax.x),
            Random.Range(_spawnAreaMin.y, _spawnAreaMax.y));

        Coin newCoin = Instantiate(_coinPrefab, spawnPosition, Quaternion.identity, parent);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        const int DividerToHalf = 2;
        Vector2 center = (_spawnAreaMin + _spawnAreaMax) / DividerToHalf;
        Vector2 size = new Vector2(_spawnAreaMax.x - _spawnAreaMin.x, _spawnAreaMax.y - _spawnAreaMin.y);
        Gizmos.DrawWireCube(center, size);
    }
}