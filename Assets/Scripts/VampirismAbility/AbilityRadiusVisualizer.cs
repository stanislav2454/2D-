using UnityEngine;

public class AbilityRadiusVisualizer : MonoBehaviour
{
    [System.Serializable]
    public class VisualSettings
    {
        public float radiusScale = 5f;
        public float pulseSpeed = 1.8f;
        public float pulseAmplitude = 0.2f;
        public Color activeColor = new Color(1f, 0f, 0f, 0.3f);
        public Color cooldownColor = new Color(0.5f, 0.5f, 0.5f, 0.2f);
        public Color readyColor = new Color(0f, 1f, 0f, 0.1f);
    }

    [Header("Components")]
    [SerializeField] private SpriteRenderer _radiusSprite;

    [Header("Settings")]
    [SerializeField] private VisualSettings _settings = new VisualSettings();

    private VampirismAbility _ability;
    private bool _isActive;
    private Vector3 _baseScale;

    public void Initialize(VampirismAbility ability)
    {
        _ability = ability;
        SetupRadiusVisualization();
    }

    public void SetActive(bool active)
    {
        _isActive = active;

        if (_radiusSprite != null)
        {
            _radiusSprite.enabled = active;
            UpdateColor();
        }
    }

    public void UpdateVisualization()
    {
        if (_isActive == false || _radiusSprite == null)
            return;

        if (_ability.IsAbilityActive)
        {
            float pulse = Mathf.PingPong(Time.time * _settings.pulseSpeed, _settings.pulseAmplitude) + 1f;
            _radiusSprite.transform.localScale = _baseScale * pulse;
        }
        else
        {
            _radiusSprite.transform.localScale = _baseScale;
        }
    }

    private void UpdateColor()
    {
        if (_radiusSprite == null)
            return;

        _radiusSprite.color = _ability.IsAbilityActive ? _settings.activeColor :
                             _ability.IsAbilityReady ? _settings.readyColor :
                             _settings.cooldownColor;
    }

    private void SetupRadiusVisualization()
    {
        if (_radiusSprite != null)
        {
            _baseScale = Vector3.one * _settings.radiusScale;
            _radiusSprite.transform.localScale = _baseScale;
            UpdateColor();
            _radiusSprite.enabled = false;
        }
    }
}