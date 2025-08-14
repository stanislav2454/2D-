using System.Collections;
using UnityEngine;

[DisallowMultipleComponent, RequireComponent(typeof(EnemyMovement))]
public class Enemy : MonoBehaviour
{
    [SerializeField] private float _lifeTime = 5f;

    public EnemyMovement Movement { get; private set; }
    private Coroutine _deathCoroutine;

    private void Awake()
    {
        Movement = GetComponent<EnemyMovement>();
        _deathCoroutine = StartCoroutine(DieAfterDelay());
    }

    private IEnumerator DieAfterDelay()
    {
        yield return new WaitForSeconds(_lifeTime);
        Destroy(gameObject);
    }

    private void OnDisable()
    {
        if (_deathCoroutine != null)
            StopCoroutine(_deathCoroutine);
    }
}