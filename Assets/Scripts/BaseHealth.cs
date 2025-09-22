using UnityEngine;
using System;

public class BaseHealth : MonoBehaviour, IDamageable, IHealth
{
    private const int DefaultValue = 0;
    protected const int MinHealth = 0;
    private const int MinAllowedMaxHealth = 1;
    private const int MinDamageAmount = 0;

    [SerializeField] private int _max = 100;

    public event Action<BaseHealth> Died;
    public event Action<int, int> Changed;
    public event Action Revived;

    [field: SerializeField] public int Current { get; protected set; }
    public int Max => _max;
    public bool IsDead { get; private set; }
    public float Normalized => _max > MinAllowedMaxHealth ? (float)Current / _max : DefaultValue;

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

    //public float GetHealthNormalized() =>
    //    (float)Current / Max;

    protected void LimitHealth() =>
        Current = Mathf.Clamp(Current, MinHealth, Max);

#if UNITY_EDITOR
    [Header("Debug Settings")]
    [SerializeField] private int _testDamageAmount = 20;
    [SerializeField] private int _testHealAmount = 10;

    [ContextMenu(nameof(TakeTestDamage))]
    public void TakeTestDamage() =>
        TakeDamage(_testDamageAmount);

    [ContextMenu(nameof(TestHeal))]
    public void TestHeal() =>
        Heal(_testHealAmount);

    [ContextMenu("Reset Health")]
    public void EditorResetHealth() =>
        Init();
#endif
}