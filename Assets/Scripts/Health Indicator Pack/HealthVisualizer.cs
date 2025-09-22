using UnityEngine;

public abstract class HealthVisualizer : MonoBehaviour
{
    [Header("Health Reference")]
    [SerializeField] protected BaseHealth Health;

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
        if (Health == null)
            Health = GetComponentInParent<BaseHealth>();

        if (Health == null)
            Debug.LogWarning($"HealthComponent not found for {GetType().Name} on {gameObject.name}", this);
    }

    protected virtual void SubscribeToEvents()
    {
        if (Health == null) 
            return;

        Health.Changed += HandleHealthChanged;
        Health.Died += HandleDeath;
        Health.Revived += HandleRevive;
    }

    protected virtual void UnsubscribeFromEvents()
    {
        if (Health == null) 
            return;

        Health.Changed -= HandleHealthChanged;
        Health.Died -= HandleDeath;
        Health.Revived -= HandleRevive;
    }

    protected virtual void HandleHealthChanged(int current, int max) => 
        UpdateVisualization();

    protected virtual void HandleDeath(BaseHealth health) => 
        UpdateVisualization();

    protected virtual void HandleRevive() => 
        UpdateVisualization();

    protected abstract void UpdateVisualization();
}