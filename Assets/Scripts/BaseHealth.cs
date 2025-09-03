using UnityEngine;
using System;

public class BaseHealth : MonoBehaviour, IDamageable
{
    protected const int MinHealth = 0;
    protected const int MaxHealth = 100;

    [field: SerializeField] public int CurrentHealth { get; protected set; }

    public event Action OnDeath;
    public event Action<int> OnHealthChanged;

    private void Awake()
    {
        CurrentHealth = MaxHealth;
        InvokeHealthChanged(CurrentHealth);
    }

    public virtual void Die()
    {
        OnDeath?.Invoke();
        OnDeath = null;
        OnHealthChanged = null;
        CurrentHealth = MaxHealth;
    }

    public virtual int TakeDamage(int damage)
    {
        int actualDamage = Mathf.Min(CurrentHealth, damage);
        CurrentHealth -= actualDamage;
        LimitHealth();

        OnHealthChanged?.Invoke(CurrentHealth);

        if (CurrentHealth <= 0)
            Die();

        return actualDamage;
    }

    public virtual void Heal(int amount)
    {
        if (amount <= 0)
            return;

        CurrentHealth += amount;
        LimitHealth();
        InvokeHealthChanged(CurrentHealth);
    }

    public void ResetHealth()
    {
        CurrentHealth = MaxHealth;
        OnHealthChanged?.Invoke(CurrentHealth);
    }

    protected void InvokeHealthChanged(int health) =>
        OnHealthChanged?.Invoke(health);

    protected void LimitHealth() =>
        CurrentHealth = Mathf.Clamp(CurrentHealth, MinHealth, MaxHealth);

#if UNITY_EDITOR
    [Header("Debug Settings")]
    [SerializeField] private int _testDamageAmount = 10;
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