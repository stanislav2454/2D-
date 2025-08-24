using UnityEngine;

public class PlayerHealth : BaseHealth
{
    [Header("Debug Settings")]
    [SerializeField] private int _testDamageAmount = 10;
    [SerializeField] private int _testHealAmount = 10;

    public void Heal(int amount)
    {
        if (amount <= 0) 
            return; 

        CurrentHealth += amount;
        LimitHealth();
        InvokeHealthChanged(CurrentHealth);
    }
}