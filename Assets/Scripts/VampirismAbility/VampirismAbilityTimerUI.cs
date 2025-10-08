using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class VampirismAbilityTimerUI : MonoBehaviour
{
    [System.Serializable]
    public class UIColors
    {
        public Color activeColor = Color.red;
        public Color cooldownColor = Color.blue;
        public Color readyColor = Color.green;
    }

    [Header("UI References")]
    [SerializeField] private Slider _timerSlider;
    [SerializeField] private TMP_Text _timerText;
    [SerializeField] private Image _sliderFillImage;

    [Header("Colors")]
    [SerializeField] private UIColors _colors = new UIColors();

    [Header("Text Settings")]
    [SerializeField] private string _activeStatusText = "ACTIVE";
    [SerializeField] private string _cooldownStatusText = "COOLDOWN";
    [SerializeField] private string _readyStatusText = "READY";

    private VampirismAbility _ability;
    private bool _isActive;

    public void Initialize(VampirismAbility ability)
    {
        _ability = ability;
        SetupUI();
    }

    public void SetActive(bool active)
    {
        _isActive = active;

        if (_timerSlider != null)
            _timerSlider.gameObject.SetActive(active);

        if (_timerText != null)
            _timerText.gameObject.SetActive(active);
    }

    public void UpdateUI()
    {
        if (_ability == null || _isActive == false)
            return;

        float progress;
        string status;
        float seconds;

        if (_ability.IsAbilityActive)
        {
            progress = _ability.AbilityProgressNormalized;
            seconds = _ability.RemainingTime;
            status = _activeStatusText;
            SetColor(_colors.activeColor);
        }
        else if (_ability.IsAbilityReady == false)
        {
            progress = _ability.CooldownProgressNormalized;
            seconds = _ability.RemainingTime;
            status = _cooldownStatusText;
            SetColor(_colors.cooldownColor);
        }
        else
        {
            progress = 1f;
            seconds = 0f;
            status = _readyStatusText;
            SetColor(_colors.readyColor);
        }

        UpdateTimerDisplay(progress, status, seconds);
    }

    private void SetColor(Color color)
    {
        if (_sliderFillImage != null)
            _sliderFillImage.color = color;
    }

    private void SetupUI()
    {
        if (_timerSlider != null)
        {
            _timerSlider.minValue = 0f;
            _timerSlider.maxValue = 1f;
            _timerSlider.value = 1f;
            _timerSlider.gameObject.SetActive(false);
        }

        if (_timerText != null)
        {
            _timerText.text = $"{_readyStatusText}\n0.0s";
            _timerText.gameObject.SetActive(false);
        }

        SetColor(_colors.readyColor);
    }

    private void UpdateTimerDisplay(float progress, string status, float seconds)
    {
        if (_timerSlider != null)
            _timerSlider.value = progress;

        if (_timerText != null)
            _timerText.text = $"{status}\n{seconds:F1}s";
    }
}