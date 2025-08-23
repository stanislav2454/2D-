using UnityEngine;

public class EnemySpawnData : MonoBehaviour
{
    [SerializeField] private EnemySpawnPoint _spawnPoint;
    [SerializeField] private EnemyPath[] _paths;

    public EnemySpawnPoint SpawnPoint => _spawnPoint;
    public EnemyPath RandomPath => GetRandomPath();
    public EnemyPath this[int index] => GetPath(index);

    private void OnValidate()
    {
        if (_spawnPoint == null)
            Debug.LogWarning("SpawnPoint is not set!", this);

        if (_paths == null || _paths.Length == 0)
            Debug.LogWarning("Paths array is empty!", this);
    }

    private bool TryGetPath(int index, out EnemyPath path)
    {
        path = null;

        if (_paths == null || _paths.Length == 0)
            return false;

        path = _paths[Mathf.Clamp(index, 0, _paths.Length - 1)];
        return true;
    }

    private EnemyPath GetPath(int index) =>
         TryGetPath(index, out var path) ? path : null;

    private EnemyPath GetRandomPath()
    {
        if (TryGetPath(0, out _) == false)
            return null;

        int randomIndex = Random.Range(0, _paths.Length);
        return _paths[randomIndex];
    }
}