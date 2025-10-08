using UnityEngine;
using System.Collections;

public class VampirismAbility : MonoBehaviour
{
    [System.Serializable]
    public class AbilitySettings
    {
        private const float MinDuration = 0.1f;
        private const float MinCooldown = 0.1f;
        private const float MinTickInterval = 0.05f;
        private const int MinDamage = 1;

        [SerializeField] private float _duration = 6f;
        [SerializeField] private float _cooldown = 4f;
        [SerializeField] private float _tickInterval = 0.2f;
        [SerializeField] private int _damagePerTick = 2;
        [SerializeField, Range(0f, 1f)] private float _healRatio = 0.5f;

        public float Duration => _duration;
        public float Cooldown => _cooldown;
        public float TickInterval => _tickInterval;
        public int DamagePerTick => _damagePerTick;
        public float HealRatio => _healRatio;

        public void SetDuration(float duration) =>
            _duration = Mathf.Max(duration, MinDuration);

        public void SetCooldown(float cooldown) =>
            _cooldown = Mathf.Max(cooldown, MinCooldown);

        public void SetTickInterval(float interval) =>
            _tickInterval = Mathf.Max(interval, MinTickInterval);

        public void SetDamagePerTick(int damage) =>
            _damagePerTick = Mathf.Max(damage, MinDamage);

        public void SetHealRatio(float ratio) =>
            _healRatio = Mathf.Clamp01(ratio);
    }

    [Header("Settings")]
    [SerializeField] private AbilitySettings _settings = new AbilitySettings();

    [Header("References")]
    [SerializeField] private AttackZone _vampirismZone;
    [SerializeField] private BaseHealth _ownerHealth;

    private CooldownTimer _abilityTimer;
    private CooldownTimer _cooldownTimer;
    private Coroutine _abilityCoroutine;

    public event System.Action AbilityStarted;
    public event System.Action AbilityEnded;
    public event System.Action AbilityReady;

    public bool IsAbilityActive => _abilityTimer.IsActive;
    public bool IsAbilityReady => _cooldownTimer.IsReady && _abilityTimer.IsActive == false;
    public float AbilityProgressNormalized => _abilityTimer.ProgressNormalized;
    public float CooldownProgressNormalized => _cooldownTimer.ProgressNormalized;
    public float RemainingTime => _abilityTimer.IsActive ? _abilityTimer.RemainingTime : _cooldownTimer.RemainingTime;

    public AttackZone VampirismZone => _vampirismZone;
    public AbilitySettings Settings => _settings;

    private void Awake()
    {
        InitializeComponents();
        ValidateSettings();
    }

    private void Update()
    {
        _abilityTimer.Update(Time.deltaTime);
        _cooldownTimer.Update(Time.deltaTime);
    }

    private void OnDisable()
    {
        StopAbility();
    }

    public void StartAbility()
    {
        if (_abilityTimer.IsActive || _cooldownTimer.IsReady == false)
            return;

        _abilityTimer.Start();
        AbilityStarted?.Invoke();

        if (_abilityCoroutine != null)
            StopCoroutine(_abilityCoroutine);

        _abilityCoroutine = StartCoroutine(VampirismRoutine());
    }

    public void StopAbility()
    {
        if (_abilityTimer.IsActive)
        {
            _abilityTimer.Stop();
            AbilityEnded?.Invoke();
        }

        if (_abilityCoroutine != null)
        {
            StopCoroutine(_abilityCoroutine);
            _abilityCoroutine = null;
        }
    }

    public void ApplySettings(float duration, float cooldown, float tickInterval, int damagePerTick, float healRatio)
    {
        _settings.SetDuration(duration);
        _settings.SetCooldown(cooldown);
        _settings.SetTickInterval(tickInterval);
        _settings.SetDamagePerTick(damagePerTick);
        _settings.SetHealRatio(healRatio);

        _abilityTimer.SetDuration(_settings.Duration);
        _cooldownTimer.SetDuration(_settings.Cooldown);
    }

    private void InitializeComponents()
    {
        _abilityTimer = new CooldownTimer(_settings.Duration);
        _cooldownTimer = new CooldownTimer(_settings.Cooldown);
    }

    private void ValidateSettings()
    {
        if (_vampirismZone == null)
            Debug.LogError($"AttackZone not found for {GetType().Name} on {gameObject.name}", this);

        if (_ownerHealth == null)
            Debug.LogError($"BaseHealth not found for {GetType().Name} on {gameObject.name}", this);
    }

    private IEnumerator VampirismRoutine()
    {
        while (_abilityTimer.IsActive)
        {
            var nearestTarget = _vampirismZone?.FindNearestTarget(transform.position);

            if (nearestTarget != null)
            {
                int damageDealt = nearestTarget.TakeDamage(_settings.DamagePerTick);

                if (damageDealt > 0)
                {
                    int healAmount = Mathf.RoundToInt(damageDealt * _settings.HealRatio);
                    _ownerHealth.Heal(healAmount);
                }
            }

            yield return new WaitForSeconds(_settings.TickInterval);
        }

        AbilityEnded?.Invoke();
        _cooldownTimer.Start();

        yield return new WaitUntil(() => _cooldownTimer.IsReady);

        AbilityReady?.Invoke();
    }
}