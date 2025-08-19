using System;
using System.Collections;
using UnityEngine;

[DisallowMultipleComponent, RequireComponent(typeof(EnemyMover))]
public class Enemy : MonoBehaviour
{
    [SerializeField] private float _lifeTime = 5f;
    public Action Dead;
    public EnemyMover Movement { get; private set; }
    private Coroutine _deathCoroutine;

    private void Awake()
    {
        Movement = GetComponent<EnemyMover>();
        _deathCoroutine = StartCoroutine(DieAfterDelay());
    }

    private void OnDisable()
    {
        if (_deathCoroutine != null)
            StopCoroutine(_deathCoroutine);
    }

    private IEnumerator DieAfterDelay()
    {
        yield return new WaitForSeconds(_lifeTime);
        Die();
    }
    public void Die()
    {
        if (_deathCoroutine != null)
        {
            StopCoroutine(_deathCoroutine);
            _deathCoroutine = null;
        }

        Dead?.Invoke();
        Destroy(gameObject);
    }
}