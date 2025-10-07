using UnityEngine;

public class PlayerHealth : BaseHealth
{
    [SerializeField] private PlayerSettings _settings;

    private void Start()
    {
        if (_settings != null)
            SetMaxHealth(_settings.MaxHealth); 

        Init();
    }
}