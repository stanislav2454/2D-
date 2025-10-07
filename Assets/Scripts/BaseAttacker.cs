using System.Collections;
using UnityEngine;

public abstract class BaseAttacker : MonoBehaviour
{
    protected const float TargetCheckInterval = 0.1f;
    protected const float MinAttackRange = 0.01f;

    protected bool CanAttack = true;
    protected float SqrAttackRange;
    protected Coroutine AttackCoroutine;

    protected abstract int AttackDamage { get; }
    protected abstract float AttackCooldown { get; }
    protected abstract float AttackRange { get; }

    public event System.Action<int> AttackStarted;

    protected virtual void Awake()
    {
        CalculateSqrAttackRange();
    }

    protected virtual void OnDisable()
    {
        StopAttackCoroutine();
    }

    protected void StopAttackCoroutine()
    {
        if (AttackCoroutine != null)
        {
            StopCoroutine(AttackCoroutine);
            AttackCoroutine = null;
        }
    }

    protected void CalculateSqrAttackRange()
    {
        float range = Mathf.Max(AttackRange, MinAttackRange);
        SqrAttackRange = range * range;
    }

    protected IEnumerator AttackCooldownRoutine()
    {
        CanAttack = false;
        yield return new WaitForSeconds(AttackCooldown);
        CanAttack = true;
    }

    protected virtual void OnAttackPerformed(int damageDealt)
    {
        AttackStarted?.Invoke(damageDealt);
    }

    public abstract bool IsAbleToAttack();
    public abstract void PerformAttack();
}