using System;
using System.Collections;
using UnityEngine;

[DisallowMultipleComponent, RequireComponent(typeof(EnemyMover))]
public class Enemy : MonoBehaviour
{
    [SerializeField] private float _lifeTime = 5f;
    public Action<Enemy> Dead;
    private Coroutine _deathCoroutine;
    private EnemyPool _pool;

    public EnemyMover Movement { get; private set; }

    private void Awake()
    {
        Movement = GetComponent<EnemyMover>();
    }

    private void OnDisable()
    {
        if (_deathCoroutine != null)
        {
            StopCoroutine(_deathCoroutine);
            _deathCoroutine = null;
        }
    }

    public void ResetEnemy()
    {
        if (_deathCoroutine != null)
        {
            StopCoroutine(_deathCoroutine);
            _deathCoroutine = null;
        }

        transform.rotation = Quaternion.identity;
        _deathCoroutine = StartCoroutine(DieAfterDelay());
    }

    public void SetPool(EnemyPool pool)
    {
        _pool = pool;
    }

    private IEnumerator DieAfterDelay()
    {
        yield return new WaitForSeconds(_lifeTime);
        Die();
    }
    public void Die()
    {
        Dead?.Invoke(this);
        _pool.ReleaseEnemy(this);
    }
}