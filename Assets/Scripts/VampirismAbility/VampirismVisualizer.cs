using UnityEngine;

[DisallowMultipleComponent, RequireComponent(typeof(VampirismAbility))]
public class VampirismVisualizer : MonoBehaviour
{
    [Header("Visualization Components")]
    [SerializeField] private VampirismAbilityTimerUI _timerUI;
    [SerializeField] private AbilityRadiusVisualizer _radiusVisualizer;

    private VampirismAbility _vampirismAbility;

    private void Awake()
    {
        _vampirismAbility = GetComponent<VampirismAbility>();
        InitializeComponents();
    }

    private void OnEnable()
    {
        if (_vampirismAbility != null)
        {
            _vampirismAbility.AbilityStarted += OnAbilityStarted;
            _vampirismAbility.AbilityEnded += OnAbilityEnded;
            _vampirismAbility.AbilityReady += OnAbilityReady;
        }
    }

    private void OnDisable()
    {
        if (_vampirismAbility != null)
        {
            _vampirismAbility.AbilityStarted -= OnAbilityStarted;
            _vampirismAbility.AbilityEnded -= OnAbilityEnded;
            _vampirismAbility.AbilityReady -= OnAbilityReady;
        }
    }

    private void InitializeComponents()
    {
        if (_timerUI != null)
            _timerUI.Initialize(_vampirismAbility);

        if (_radiusVisualizer != null)
            _radiusVisualizer.Initialize(_vampirismAbility);
    }

    private void OnAbilityStarted()
    {
        _timerUI?.SetActive(true);
        _timerUI?.SetActiveState();
        _radiusVisualizer?.SetActive(true);
    }

    private void OnAbilityEnded()
    {
        _timerUI?.SetCooldownState();
        _radiusVisualizer?.SetCooldownState();
    }

    private void OnAbilityReady()
    {
        _timerUI?.SetActive(false);
        _radiusVisualizer?.SetActive(false);
    }

    private void Update()
    {
        _timerUI?.UpdateUI();
        _radiusVisualizer?.UpdateVisualization();
    }
}