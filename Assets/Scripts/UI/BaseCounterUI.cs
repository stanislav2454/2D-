using TMPro;
using UnityEngine;

public abstract class BaseCounterUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI[] _additionalTextElements;

    protected int _currentValue;

    public int GetValue => _currentValue;

    protected abstract string TextPrefix { get; }

    public virtual void AddValue(int amount = 1)
    {
        _currentValue += amount;
        UpdateUI(_currentValue);
    }

    public virtual void SubtractValue(int amount = 1)
    {
        _currentValue = Mathf.Max(0, _currentValue - amount);
        UpdateUI(_currentValue);
    }

    public virtual void ResetValue()
    {
        _currentValue = 0;
        UpdateUI(_currentValue);
    }

    public virtual void SetValue(int value)
    {
        _currentValue = Mathf.Max(0, value);
        UpdateUI(_currentValue);
    }

    protected virtual void UpdateUI(int value)
    {
        if (_additionalTextElements != null)
            foreach (var textElement in _additionalTextElements)
                textElement.text = $"{TextPrefix} {value}";
    }
}