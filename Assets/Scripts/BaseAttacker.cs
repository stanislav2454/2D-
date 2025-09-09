using System.Collections;
using UnityEngine;

public class BaseAttacker : MonoBehaviour // ← ИЗМЕНЕНО: Базовый класс для атаки
{
    [SerializeField] protected float _damageInterval = 0.5f;
    [SerializeField] protected int _damage = 1;
    [SerializeField] protected AttackZone _attackZone;

    protected Coroutine _attackCoroutine;
    protected bool _isAttacking;

    public event System.Action Attacked;

    protected virtual void Awake()
    {
        if (_attackZone == null)
            _attackZone = GetComponentInChildren<AttackZone>();
    }

    protected virtual void OnDisable()
    {
        StopAttacking();
    }

    public virtual void StartAttacking()
    {
        if (_isAttacking) return;
        _isAttacking = true;
        _attackCoroutine = StartCoroutine(AttackRoutine());
    }

    public virtual void StopAttacking()
    {
        _isAttacking = false;
        if (_attackCoroutine != null)
        {
            StopCoroutine(_attackCoroutine);
            _attackCoroutine = null;
        }
    }

    protected virtual IEnumerator AttackRoutine()
    {
        while (_isAttacking)
        {
            if (_attackZone != null)
            {
                _attackZone.CleanDestroyedTargets();
                yield return new WaitForSeconds(_damageInterval);

                if (_attackZone.TargetsInZoneCount > 0)
                    Attack();
            }
            yield return null;
        }
    }

    protected virtual void Attack()
    {
        if (_attackZone == null) return;
        CalculateDamageToTargets(_attackZone, _damage);
        Attacked?.Invoke();
    }

    protected virtual int CalculateDamageToTargets(AttackZone attackZone, int damage)
    {
        int totalDamageDealt = 0;
        foreach (var target in attackZone.Targets)
        {
            if (target != null)
                totalDamageDealt += target.TakeDamage(damage);
        }
        return totalDamageDealt;
    }

    public virtual void ResetAttacker()
    {
        StopAttacking();
    }
}