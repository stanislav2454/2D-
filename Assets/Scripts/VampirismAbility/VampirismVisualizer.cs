using UnityEngine;

[DisallowMultipleComponent]
public class VampirismVisualizer : MonoBehaviour
{
    [Header("Visualization Components")]
    [SerializeField] private VampirismAbilityTimerUI _timerUI;
    [SerializeField] private AbilityRadiusVisualizer _radiusVisualizer;

    private void Start()
    {
        var ability = GetComponent<VampirismAbility>();

        if (ability != null)
        {
            InitializeWithAbility(ability);
        }
    }

    public void InitializeWithAbility(VampirismAbility ability)
    {
        if (_timerUI != null)
            _timerUI.Initialize(ability);

        if (_radiusVisualizer != null)
            _radiusVisualizer.Initialize(ability);

        ability.AbilityStarted += OnAbilityStarted;
        ability.AbilityEnded += OnAbilityEnded;
        ability.AbilityReady += OnAbilityReady;
    }

    private void Update()
    {
        _timerUI?.UpdateUI();
        _radiusVisualizer?.UpdateVisualization();
    }

    private void OnDestroy()
    {
        var ability = GetComponent<VampirismAbility>();

        if (ability != null)
        {
            ability.AbilityStarted -= OnAbilityStarted;
            ability.AbilityEnded -= OnAbilityEnded;
            ability.AbilityReady -= OnAbilityReady;
        }
    }

    private void OnAbilityStarted()
    {
        _timerUI?.SetActive(true);
        _radiusVisualizer?.SetActive(true);
    }

    private void OnAbilityEnded()    {    }

    private void OnAbilityReady()
    {
        _timerUI?.SetActive(false);
        _radiusVisualizer?.SetActive(false);
    }
}