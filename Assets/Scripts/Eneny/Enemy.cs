using UnityEngine;

[RequireComponent(typeof(EnemyMovement))]
public class Enemy : MonoBehaviour
{
    private const float LifeTime = 5f;

    public EnemyMovement Movement { get; private set; }

    private void Awake()
    {
        Movement = GetComponent<EnemyMovement>();
        Die();
    }

    private void Die()
    {
        Destroy(gameObject, LifeTime);
    }
}