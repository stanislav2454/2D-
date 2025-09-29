using System.Collections;
using UnityEngine;

public class SmoothBarHealthVisualizer : BarHealthVisualizer
{
    private const float DefaultValue = 0f;
    private const float AnimationTolerance = 0.001f;

    [Header("Smooth Settings")]
    [SerializeField] private float _smoothSpeed = 0.2f;

    private float _targetValue;
    private Coroutine _smoothCoroutine;
    private bool _isActive => isActiveAndEnabled && _slider != null;

    protected override void Start()
    {
        base.Start();
        _targetValue = Health?.Normalized ?? DefaultValue;

        if (_slider != null)
            _slider.value = _targetValue;
    }

    private void OnEnable()
    {
        if (Health != null && _slider != null)
        {
            _targetValue = Health.Normalized;
            _slider.value = _targetValue;
            UpdateBarColor();
        }
    }

    private void OnDisable()
    {
        StopSmoothAnimation();
    }

    protected override void OnDestroy()
    {
        StopSmoothAnimation();
        base.OnDestroy();
    }

    protected override void UpdateVisualization()
    {
        if (Health == null || _isActive == false)
            return;

        float newTargetValue = Health.Normalized;

        if (Mathf.Abs(newTargetValue - _targetValue) > AnimationTolerance)
        {
            _targetValue = newTargetValue;
            UpdateBarColor();
            StartSmoothAnimation();
        }
        else
        {
            _targetValue = newTargetValue;

            if (_slider != null)
                _slider.value = _targetValue;

            UpdateBarColor();
        }
    }

    private void StartSmoothAnimation()
    {
        if (_isActive == false || _smoothCoroutine != null)
            return;

        _smoothCoroutine = StartCoroutine(SmoothAnimationRoutine());
    }

    private IEnumerator SmoothAnimationRoutine()
    {
        if (_slider == null)
            yield break;

        while (Mathf.Approximately(_slider.value, _targetValue) == false)
        {
            _slider.value = Mathf.MoveTowards(
                _slider.value,
                _targetValue,
                _smoothSpeed * Time.deltaTime);

            yield return null;
        }

        _slider.value = _targetValue;
        _smoothCoroutine = null;
    }

    private void StopSmoothAnimation()
    {
        if (_smoothCoroutine != null)
        {
            StopCoroutine(_smoothCoroutine);
            _smoothCoroutine = null;
        }
    }
}