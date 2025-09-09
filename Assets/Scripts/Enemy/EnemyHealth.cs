using UnityEngine;

public class EnemyHealth : BaseHealth
{
    [SerializeField] private EnemySettings _settings;

    public override void Init()
    {
        base.Init();

        if (_settings != null)
            CurrentHealth = _settings.MaxHealth;
    }

    public void ApplySettings(EnemySettings settings)
    {
        _settings = settings;
    }
}