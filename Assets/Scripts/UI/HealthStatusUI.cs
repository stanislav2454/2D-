using TMPro;
using UnityEngine;

public class HealthStatusUI : MonoBehaviour
{
    [SerializeField] private BaseHealth _health;
    [SerializeField] private TextMeshProUGUI[] _healthTexts;

    private void OnEnable()
    {
        _health.OnHealthChanged += UpdateText;
        UpdateText(_health.CurrentHealth);
    }

    private void OnDisable() =>
        _health.OnHealthChanged -= UpdateText;

    private void UpdateText(int health)
    {
        foreach (var textElement in _healthTexts)
            textElement.text = Utils.UpdateUIText("Health", health);
    }
}