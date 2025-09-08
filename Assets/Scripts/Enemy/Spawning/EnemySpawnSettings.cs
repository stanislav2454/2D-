using UnityEngine;

[CreateAssetMenu(fileName = "EnemySpawnSettings", menuName = "Game/Enemy Spawn Settings")]
public class EnemySpawnSettings : ScriptableObject
{
    [Header("Spawn Settings")]
    public float spawnInterval = 2f;
    [Range(0, 20)] public int numberEnemiesToSpawn = 10;

    [Header("Pool Settings")]
    public int defaultCapacity = 10;
    [Range(1, 20)] public int maxPoolSize = 20;
    public bool collectionCheck = true;

    [Header("Validation")]
    public bool autoValidate = true;

    private void OnValidate()
    {
        if (autoValidate)
        {
            defaultCapacity = Mathf.Min(defaultCapacity, maxPoolSize);
            numberEnemiesToSpawn = Mathf.Min(numberEnemiesToSpawn, maxPoolSize);
        }
    }
}