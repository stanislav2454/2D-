using UnityEngine;

public abstract class HealthVisualizer : MonoBehaviour
{
    [Header("Health Reference")]
    [SerializeField] protected BaseHealth _healthComponent;

    protected virtual void Start()
    {
        TryFindHealthComponent();
        SubscribeToEvents();
        UpdateVisualization();
    }

    protected virtual void OnDestroy()
    {
        UnsubscribeFromEvents();
    }

    protected virtual void TryFindHealthComponent()
    {
        if (_healthComponent == null)
            _healthComponent = GetComponentInParent<BaseHealth>();

        if (_healthComponent == null)
            Debug.LogWarning($"HealthComponent not found for {GetType().Name} on {gameObject.name}", this);
    }

    protected virtual void SubscribeToEvents()
    {
        if (_healthComponent == null) 
            return;

        _healthComponent.HealthChanged += HandleHealthChanged;
        _healthComponent.Died += HandleDeath;
        _healthComponent.Revived += HandleRevive;
    }

    protected virtual void UnsubscribeFromEvents()
    {
        if (_healthComponent == null) 
            return;

        _healthComponent.HealthChanged -= HandleHealthChanged;
        _healthComponent.Died -= HandleDeath;
        _healthComponent.Revived -= HandleRevive;
    }

    protected virtual void HandleHealthChanged(int current, int max) => 
        UpdateVisualization();

    protected virtual void HandleDeath(BaseHealth health) => 
        UpdateVisualization();

    protected virtual void HandleRevive() => 
        UpdateVisualization();

    protected abstract void UpdateVisualization();
}