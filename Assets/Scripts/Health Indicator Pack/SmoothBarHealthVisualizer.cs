using UnityEngine;

public class SmoothBarHealthVisualizer : BarHealthVisualizer
{
    private const float DefaultValue = 0f;

    [Header("Smooth Settings")]
    [SerializeField] private float _smoothSpeed = 2f;

    private float _targetValue;
    private bool _isAnimating;

    protected override void Start()
    {
        base.Start();
        _targetValue = _healthComponent != null ? _healthComponent.GetHealthNormalized() : DefaultValue;
    }

    private void Update()
    {
        if (_isAnimating == false || _healthSlider == null)
            return;

        _healthSlider.value = Mathf.MoveTowards(
            _healthSlider.value,
            _targetValue,
            _smoothSpeed * Time.deltaTime);

        if (Mathf.Approximately(_healthSlider.value, _targetValue))
            _isAnimating = false;
    }

    protected override void UpdateVisualization()
    {
        if (_healthComponent == null)
            return;

        _targetValue = _healthComponent.GetHealthNormalized();
        _isAnimating = true;
        UpdateBarColor();
    }
}