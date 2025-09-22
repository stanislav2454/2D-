using TMPro;
using UnityEngine;

public class TextHealthVisualizer : HealthVisualizer
{
    [Header("Text Settings")]
    [SerializeField] private TMP_Text _healthText;
    [SerializeField] private string _format = "{0}/{1}";
    [SerializeField] private bool _showMaxHealth = true;
    [SerializeField] private Color _normalColor = Color.white;
    [SerializeField] private Color _lowHealthColor = Color.red;
    [SerializeField] private float _lowHealthThreshold = 0.3f;

    protected override void UpdateVisualization()
    {
        if (_healthText == null || _healthComponent == null)
            return;

        string text = _showMaxHealth
            ? string.Format(_format, _healthComponent.CurrentHealth, _healthComponent.MaxHealth)
            : _healthComponent.CurrentHealth.ToString();

        _healthText.text = text;

        if (_healthComponent.GetHealthNormalized() <= _lowHealthThreshold)
            _healthText.color = _lowHealthColor;
        else
            _healthText.color = _normalColor;
    }
}