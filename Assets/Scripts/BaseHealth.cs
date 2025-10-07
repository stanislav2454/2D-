using UnityEngine;
using System;

public class BaseHealth : MonoBehaviour, IDamageable, IHealth
{
    protected const int MinHealth = 0;
    private const int DefaultValue = 0;
    private const int MinAllowedMaxHealth = 1;
    private const int MinDamageAmount = 0;

    public event Action<BaseHealth> Died;
    public event Action<int, int> Changed;
    public event Action Revived;

    [field: SerializeField] public int Current { get; protected set; }
    public int Max { get; protected set; } = 100; 
    public bool IsDead { get; private set; }
    public float Normalized => Max > MinAllowedMaxHealth ? (float)Current / Max : DefaultValue;

    public virtual void Die()
    {
        if (IsDead)
            return;

        IsDead = true;
        Died?.Invoke(this);
    }

    public virtual int TakeDamage(int damage)
    {
        if (IsDead || damage <= MinDamageAmount)
            return 0;

        int actualDamage = Mathf.Min(Current, damage);
        Current -= actualDamage;
        LimitHealth();

        Changed?.Invoke(Current, Max);

        if (Current <= 0)
            Die();

        return actualDamage;
    }

    public virtual void Heal(int amount)
    {
        if (amount <= 0 || IsDead)
            return;

        Current += amount;
        LimitHealth();
        Changed?.Invoke(Current, Max);
    }

    public virtual void Init()
    {
        Current = Max;
        IsDead = false;
        Changed?.Invoke(Current, Max);
    }

    public virtual void SetMaxHealth(int maxHealth)
    {
        if (maxHealth < MinAllowedMaxHealth)
            return;

        int oldMax = Max;
        Max = maxHealth;

        if (oldMax != Max)
        {
            float healthRatio = oldMax > 0 ? (float)Current / oldMax : 1f;
            Current = Mathf.RoundToInt(Max * healthRatio);
            Changed?.Invoke(Current, Max);
        }
    }

    protected void LimitHealth() =>
        Current = Mathf.Clamp(Current, MinHealth, Max);
}