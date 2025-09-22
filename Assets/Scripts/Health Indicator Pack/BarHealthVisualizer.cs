using UnityEngine;
using UnityEngine.UI;

public class BarHealthVisualizer : HealthVisualizer
{
    private const int SliderMinValue = 0;
    private const int SliderMaxValue = 1;

    [Header("Bar Settings")]
    [SerializeField] protected Slider _healthSlider;
    [SerializeField] protected Image _fillImage;
    [SerializeField] protected Color _fullHealthColor = Color.green;
    [SerializeField] protected Color _lowHealthColor = Color.red;
    [SerializeField] protected float _lowHealthThreshold = 0.3f;

    protected override void Start()
    {
        base.Start();
        InitializeSlider();
    }

    protected void InitializeSlider()
    {
        if (_healthSlider != null)
        {
            _healthSlider.minValue = SliderMinValue;
            _healthSlider.maxValue = SliderMaxValue;
            _healthSlider.wholeNumbers = false;
        }
    }

    protected override void UpdateVisualization()
    {
        if (_healthSlider == null || _healthComponent == null)
            return;

        _healthSlider.value = _healthComponent.GetHealthNormalized();
        UpdateBarColor();
    }

    protected void UpdateBarColor()
    {
        if (_fillImage == null)
            return;

        float healthNormalized = _healthComponent.GetHealthNormalized();
        _fillImage.color = Color.Lerp(_lowHealthColor, _fullHealthColor, healthNormalized);
    }
}