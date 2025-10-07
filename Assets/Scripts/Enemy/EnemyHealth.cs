using UnityEngine;

public class EnemyHealth : BaseHealth
{
    [SerializeField] private EnemySettings _settings;

    public override void Init()
    {
        if (_settings != null)
            SetMaxHealth(_settings.MaxHealth); 

        base.Init();
    }

    public void ApplySettings(EnemySettings settings)
    {
        _settings = settings;
        SetMaxHealth(_settings.MaxHealth);
    }
}