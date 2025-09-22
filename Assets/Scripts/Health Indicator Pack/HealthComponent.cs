using System;
using UnityEngine;

public class HealthComponent : MonoBehaviour//, IHealthProvider
{
    private const int MinHealth = 0;
    private const int MinHealAmount = 0;
    private const int MinAllowedMaxHealth = 1;
    private const int MinDamageAmount = 0;

    [Header("Health Settings")]
    [SerializeField] private int _maxHealth = 100;
    [SerializeField] private int _currentHealth = 100;

    public event Action<int, int> HealthChanged;
    public event Action<int, int> MaxHealthChanged;
    public event Action Died;
    public event Action Revived;

    public int CurrentHealth => _currentHealth;
    public int MaxHealth => _maxHealth;
    public bool IsAlive { get; private set; } = true;

    public virtual void TakeDamage(int damage)
    {
        if (IsAlive == false || damage <= MinDamageAmount)
            return;

        _currentHealth = Mathf.Max(MinHealth, _currentHealth - damage);
        HealthChanged?.Invoke(_currentHealth, _maxHealth);

        if (_currentHealth <= MinHealth)
            Die();
    }

    public virtual void Heal(int amount)
    {
        if (IsAlive == false || amount <= MinHealAmount)
            return;

        _currentHealth = Mathf.Min(_maxHealth, _currentHealth + amount);
        HealthChanged?.Invoke(_currentHealth, _maxHealth);
    }

    public virtual void SetMaxHealth(int newMaxHealth, bool healToFull = false)
    {
        _maxHealth = Mathf.Max(MinAllowedMaxHealth, newMaxHealth);

        if (healToFull)
            _currentHealth = _maxHealth;
        else
            _currentHealth = Mathf.Min(_currentHealth, _maxHealth);

        MaxHealthChanged?.Invoke(_currentHealth, _maxHealth);
        HealthChanged?.Invoke(_currentHealth, _maxHealth);
    }

    public virtual void Die()
    {
        if (IsAlive == false)
            return;

        IsAlive = false;
        Died?.Invoke();
    }

    public virtual void Revive(int healthAmount = 0)
    {
        if (IsAlive)
            return;

        IsAlive = true;
        _currentHealth = healthAmount > MinHealAmount ? Mathf.Min(healthAmount, _maxHealth) : _maxHealth;

        Revived?.Invoke();
        HealthChanged?.Invoke(_currentHealth, _maxHealth);
    }

    public float GetHealthNormalized() =>
        (float)_currentHealth / _maxHealth;

    protected virtual void OnValidate()
    {
        _maxHealth = Mathf.Max(MinAllowedMaxHealth, _maxHealth);
        _currentHealth = Mathf.Clamp(_currentHealth, MinHealth, _maxHealth);
    }
}