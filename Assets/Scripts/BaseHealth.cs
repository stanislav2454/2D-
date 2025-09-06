using UnityEngine;
using System;

public class BaseHealth : MonoBehaviour, IDamageable
{
    protected const int MinHealth = 0;
    protected const int MaxHealth = 100;

    [field: SerializeField] public int CurrentHealth { get; protected set; }
    public bool IsDead { get; private set; }

    public event Action<BaseHealth> Died;
    public event Action<int> HealthChanged;

    private void Awake()
    {
        ResetHealth();
    }

    public virtual void Die()
    {
        if (IsDead)
            return;

        ResetHealth();
        IsDead = true;
        Died?.Invoke(this);
    }

    public virtual int TakeDamage(int damage)
    {
        if (IsDead)
            return 0;

        int actualDamage = Mathf.Min(CurrentHealth, damage);
        CurrentHealth -= actualDamage;
        LimitHealth();

        HealthChanged?.Invoke(CurrentHealth);

        if (CurrentHealth <= 0)
            Die();

        return actualDamage;
    }

    public virtual void Heal(int amount)
    {
        if (amount <= 0 || IsDead)
            return;

        CurrentHealth += amount;
        LimitHealth();
        HealthChanged?.Invoke(CurrentHealth);
    }

    public void ResetHealth()
    {
        CurrentHealth = MaxHealth;
        IsDead = false;
        HealthChanged?.Invoke(CurrentHealth);
    }

    protected void LimitHealth() =>
        CurrentHealth = Mathf.Clamp(CurrentHealth, MinHealth, MaxHealth);

#if UNITY_EDITOR
    [Header("Debug Settings")]
    [SerializeField] private int _testDamageAmount = 20;
    [SerializeField] private int _testHealAmount = 10;

    [ContextMenu("Take Test Damage")]
    public void TakeTestDamage() =>
        TakeDamage(_testDamageAmount);

    [ContextMenu("Test Heal")]
    public void TestHeal() =>
        Heal(_testHealAmount);

    [ContextMenu("Reset Health")]
    public void EditorResetHealth() =>
        ResetHealth();
#endif
}