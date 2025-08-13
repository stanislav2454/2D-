using UnityEngine;

public class EnemySpawnPoint : MonoBehaviour
{
    [SerializeField] private EnemyPath _enemyPath;

    public Vector3 Position => transform.position;
    public Quaternion Rotation => transform.rotation;
    public EnemyPath Path => _enemyPath;

    public void SetPath(EnemyPath path)
    {
        _enemyPath = path;
    }
}
