using UnityEngine;

public class AbilityRadiusVisualizer : MonoBehaviour
{
    private const float PulseAnimationSpeed = 1.8f;
    private const float PulseAnimationAmplitude = 1.2f;
    private const float BaseScaleMultiplier = 1f;

    [Header("Radius Visualization")]
    [SerializeField] private SpriteRenderer _radiusSprite;
    [SerializeField] private float _radiusScale = 5f;

    [Header("Colors")]
    [SerializeField] private Color _activeColor = new Color(1f, 0f, 0f, 0.3f);
    [SerializeField] private Color _cooldownColor = new Color(0.5f, 0.5f, 0.5f, 0.2f);

    private bool _isActive;
    private VampirismAbility _ability;

    public void Initialize(VampirismAbility ability)
    {
        _ability = ability;
        SetupRadiusVisualization();
    }

    private void SetupRadiusVisualization()
    {
        if (_radiusSprite != null)
        {
            _radiusSprite.transform.localScale = Vector3.one * _radiusScale;
            _radiusSprite.color = _cooldownColor;
            _radiusSprite.enabled = false;
        }
    }

    public void SetActive(bool active)
    {
        _isActive = active;

        if (_radiusSprite != null)
        {
            _radiusSprite.enabled = active;
            _radiusSprite.color = active ? _activeColor : _cooldownColor;
        }
    }

    public void SetCooldownState()
    {
        if (_radiusSprite != null)
            _radiusSprite.color = _cooldownColor;
    }

    public void UpdateVisualization()
    {
        if (_isActive == false || _radiusSprite == null || !_ability.IsAbilityActive)
            return;

        float pulse = Mathf.PingPong(Time.time * PulseAnimationSpeed, PulseAnimationAmplitude) + BaseScaleMultiplier;
        _radiusSprite.transform.localScale = Vector3.one * (_radiusScale * pulse);
    }
}