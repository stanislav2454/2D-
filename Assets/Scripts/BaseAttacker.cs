using System.Collections;
using UnityEngine;

public abstract class BaseAttacker : MonoBehaviour
{
    protected const float TARGET_CHECK_INTERVAL = 0.1f;
    protected const float MIN_ATTACK_RANGE = 0.01f;

    protected bool _canAttack = true;
    protected float _sqrAttackRange;

    protected abstract int AttackDamage { get; }
    protected abstract float AttackCooldown { get; }
    protected abstract float AttackRange { get; }

    public event System.Action AttackPerformed;

    protected virtual void Awake() => CalculateSqrAttackRange();

    protected void CalculateSqrAttackRange()
    {
        float range = Mathf.Max(AttackRange, MIN_ATTACK_RANGE);
        _sqrAttackRange = range * range;
    }

    protected IEnumerator AttackCooldownRoutine()
    {
        _canAttack = false;
        yield return new WaitForSeconds(AttackCooldown);
        _canAttack = true;
    }

    protected virtual void OnAttackPerformed() => AttackPerformed?.Invoke();

    public abstract bool CanAttack();
    public abstract void PerformAttack();
}