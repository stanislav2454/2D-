using System.Collections;
using UnityEngine;

public abstract class BaseAttacker : MonoBehaviour
{
    protected const float TargetCheckInterval = 0.1f;
    protected const float MinAttackRange = 0.01f;

    protected bool _canAttack = true;
    protected float _sqrAttackRange;
    protected Coroutine _attackCoroutine;

    protected abstract int AttackDamage { get; }
    protected abstract float AttackCooldown { get; }
    protected abstract float AttackRange { get; }

    public event System.Action<int> AttackPerformed;
    public event System.Action<int> OnDamageDealt;

    protected virtual void Awake()
    {
        CalculateSqrAttackRange();
    }

    private void OnDisable()
    {
        if (_attackCoroutine != null)
        {
            StopCoroutine(_attackCoroutine);
            _attackCoroutine = null;
        }
    }

    protected void CalculateSqrAttackRange()
    {
        float range = Mathf.Max(AttackRange, MinAttackRange);
        _sqrAttackRange = range * range;
    }

    protected IEnumerator AttackCooldownRoutine()
    {
        _canAttack = false;
        yield return new WaitForSeconds(AttackCooldown);
        _canAttack = true;
    }

    protected virtual void OnAttackPerformed(int damageDealt)
    {
        AttackPerformed?.Invoke(damageDealt);
        OnDamageDealt?.Invoke(damageDealt);
    }

    public abstract bool CanAttack();
    public abstract void PerformAttack();
}