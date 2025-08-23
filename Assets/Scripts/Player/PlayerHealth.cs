using UnityEngine;

public class PlayerHealth : BaseHealth
{
    [Header("Debug Settings")]
    [SerializeField] private int _testDamageAmount = 10;
    [SerializeField] private int _testHealAmount = 10;

    public void Heal(int amount)
    {
        CurrentHealth += amount;
        LimitHealth();
        InvokeHealthChanged(CurrentHealth);
    }

#if UNITY_EDITOR
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