using UnityEngine;

[System.Serializable]
public class CooldownTimer
{
    private const float ZeroThreshold = 0f;
    private const float CooldownComplete = 1f;

    [SerializeField] private float _duration;
    private float _remainingTime;
    private bool _isReady;
    private bool _isActive;

    public CooldownTimer(float duration = 0f)
    {
        _duration = Mathf.Max(duration, ZeroThreshold);
        _isReady = true;
        _isActive = false;
        _remainingTime = ZeroThreshold;
    }

    public bool IsReady => _isReady;
    public bool IsActive => _isActive;
    public float RemainingTime => _remainingTime;
    public float Duration => _duration;
    public float ProgressNormalized => _isReady ? CooldownComplete : Mathf.Clamp01(1f - (_remainingTime / _duration));

    public void Start()
    {
        if (_isActive || _isReady == false)
            return;

        _isActive = true;
        _isReady = false;
        _remainingTime = _duration;
    }

    public void Stop() =>
        _isActive = false;

    public void Reset()
    {
        _isActive = false;
        _isReady = true;
        _remainingTime = ZeroThreshold;
    }

    public void SetDuration(float newDuration) =>
        _duration = Mathf.Max(newDuration, ZeroThreshold);

    public void Update(float deltaTime)
    {
        if (_isActive == false)
            return;

        _remainingTime -= deltaTime;

        if (_remainingTime <= ZeroThreshold)
            Complete();
    }

    private void Complete()
    {
        _remainingTime = ZeroThreshold;
        _isActive = false;
        _isReady = true;
    }
}