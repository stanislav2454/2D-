using UnityEngine;
using UnityEngine.UI;

public class BarHealthVisualizer : HealthVisualizer
{
    private const int SliderMinValue = 0;
    private const int SliderMaxValue = 1;

    [Header("Bar Settings")]
    [SerializeField] protected Slider _slider;
    [SerializeField] protected Image _fill;
    [SerializeField] protected Color _fullHealthColor = Color.green;
    [SerializeField] protected Color _lowHealthColor = Color.red;

    protected override void Start()
    {
        base.Start();
        InitializeSlider();
    }

    protected void InitializeSlider()
    {
        if (_slider != null)
        {
            _slider.minValue = SliderMinValue;
            _slider.maxValue = SliderMaxValue;
            _slider.wholeNumbers = false;
        }
    }

    protected override void UpdateVisualization()
    {
        if (_slider == null || Health == null)
            return;

        _slider.value = Health.Normalized;
        UpdateBarColor();
    }

    protected void UpdateBarColor()
    {
        if (_fill == null)
            return;

        _fill.color = Color.Lerp(_lowHealthColor, _fullHealthColor, Health.Normalized);
    }
}