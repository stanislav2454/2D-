using UnityEngine;

public class EnemyFactory
{
    private readonly Enemy _enemyPrefab;

    public EnemyFactory(Enemy enemyPrefab)
    {
        _enemyPrefab = enemyPrefab;
    }

    public Enemy CreateEnemy(Vector3 position, Quaternion rotation)
    {
        return Object.Instantiate(_enemyPrefab, position, rotation);
    }
}
