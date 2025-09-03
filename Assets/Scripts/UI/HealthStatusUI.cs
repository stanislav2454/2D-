using UnityEngine;

public class HealthStatusUI : BaseCounterUI
{
    [SerializeField] private BaseHealth _health;

    protected override string TextPrefix => "Health:";

    private void Awake()
    {
        if (_health == null)
            _health = GetComponentInParent<BaseHealth>();

        if (_health != null)
        {
            _currentValue = _health.CurrentHealth;
        }
    }

    private void OnEnable()
    {
        if (_health != null)
        {
            _health.OnHealthChanged += UpdateUI;
            UpdateUI(_health.CurrentHealth);
        }
    }

    private void OnDisable()
    {
        if (_health != null)
            _health.OnHealthChanged -= UpdateUI;
    }
}