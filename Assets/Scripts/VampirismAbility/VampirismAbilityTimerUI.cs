using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class VampirismAbilityTimerUI : MonoBehaviour
{
    private const float SliderMinValue = 0f;
    private const float SliderMaxValue = 1f;
    private const string _timerFormat = "{0}\n{1:F1}s";

    [Header("UI References")]
    [SerializeField] private Slider _timerSlider;
    [SerializeField] private TMP_Text _timerText;
    [SerializeField] private Image _sliderFillImage;

    [Header("Colors")]
    [SerializeField] private Color _activeColor = Color.red;
    [SerializeField] private Color _cooldownColor = Color.blue;

    [Header("Text Settings")]
    [SerializeField] private string _activeStatusText = "ACTIVE";
    [SerializeField] private string _cooldownStatusText = "COOLDOWN";

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

    public void SetActiveState()
    {
        if (_sliderFillImage != null)
            _sliderFillImage.color = _activeColor;
    }

    public void SetCooldownState()
    {
        if (_sliderFillImage != null)
            _sliderFillImage.color = _cooldownColor;
    }

    public void UpdateUI()
    {
        if (_ability == null || _isActive == false)
            return;

        if (_ability.IsAbilityActive)
        {
            float progress = _ability.GetRemainingTimeNormalized();
            float seconds = _ability.GetRemainingTime();
            UpdateTimerDisplay(progress, _activeStatusText, seconds);
        }
        else if (_ability.IsAbilityReady == false)
        {
            float progress = _ability.GetCooldownProgressNormalized();
            float seconds = _ability.GetRemainingTime();
            UpdateTimerDisplay(progress, _cooldownStatusText, seconds);
        }
    }

    private void SetupUI()
    {
        if (_timerSlider != null)
        {
            _timerSlider.minValue = SliderMinValue;
            _timerSlider.maxValue = SliderMaxValue;
            _timerSlider.gameObject.SetActive(false);
        }

        if (_timerText != null)
            _timerText.gameObject.SetActive(false);
    }

    private void UpdateTimerDisplay(float progress, string status, float seconds)
    {
        if (_timerSlider != null)
            _timerSlider.value = progress;

        if (_timerText != null)
            _timerText.text = string.Format(_timerFormat, status, seconds);
    }
}