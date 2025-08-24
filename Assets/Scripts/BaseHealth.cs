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

    public void ResetHealth()
    {
        CurrentHealth = MaxHealth;
        OnHealthChanged?.Invoke(CurrentHealth);
    }

    public virtual void Die()
    {
        OnDeath?.Invoke();
        OnDeath = null;
        OnHealthChanged = null;
        gameObject.SetActive(false);
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

    protected void InvokeHealthChanged(int health) =>
        OnHealthChanged?.Invoke(health);

    protected void LimitHealth() =>
        CurrentHealth = Mathf.Clamp(CurrentHealth, MinHealth, MaxHealth);
}