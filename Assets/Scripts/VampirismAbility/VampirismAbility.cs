using UnityEngine;
using System.Collections;

[DisallowMultipleComponent, RequireComponent(typeof(BaseHealth))]
public class VampirismAbility : MonoBehaviour
{
    #region ConstantsRegion
    private const float MinAbilityDuration = 0.1f;
    private const float MinCooldown = 0.1f;
    private const float MinTickInterval = 0.05f;
    private const int MinDamagePerTick = 1;
    private const float MinHealRatio = 0f;
    private const float MaxHealRatio = 1f;
    private const float ZeroDurationThreshold = 0f;
    private const float NormalizedMin = 0f;
    private const float NormalizedMax = 1f;
    private const float CooldownCompleteThreshold = 1f;
    private const float TimeRemainingThreshold = 0f;
    #endregion

    #region FieldsRegion
    [Header("Ability Settings")]
    [SerializeField] private float _abilityDuration = 6f;
    [SerializeField] private float _abilityCooldown = 4f;
    [SerializeField] private float _damageTickInterval = 0.2f;
    [SerializeField] private int _damagePerTick = 2;
    [SerializeField] [Range(0f, 1f)] private float _healRatio = 0.5f;

    [Header("References")]
    [SerializeField] private AttackZone _vampirismZone;
    [SerializeField] private BaseHealth _ownerHealth;

    private bool _isAbilityActive = false;
    private bool _isAbilityReady = true;
    private float _abilityTimer;
    private float _cooldownTimer;
    private Coroutine _abilityCoroutine;
    private Transform _ownerTransform;
    #endregion

    public event System.Action AbilityStarted;
    public event System.Action AbilityEnded;
    public event System.Action AbilityReady;

    public bool IsAbilityActive => _isAbilityActive;
    public bool IsAbilityReady => _isAbilityReady;

    private void Awake()
    {
        InitializeReferences();
        ValidateSettings();
    }

    private void OnDisable()
    {
        StopAbility();
    }

    private void Update()
    {
        UpdateTimers();
    }

    public void StartAbility()
    {
        if (_isAbilityActive || _isAbilityReady == false)
            return;

        _isAbilityActive = true;
        _isAbilityReady = false;
        _abilityTimer = ZeroDurationThreshold;
        _cooldownTimer = ZeroDurationThreshold;

        AbilityStarted?.Invoke();

        if (_abilityCoroutine != null)
            StopCoroutine(_abilityCoroutine);

        _abilityCoroutine = StartCoroutine(VampirismRoutine());
    }

    public void StopAbility()
    {
        if (_isAbilityActive)
        {
            _isAbilityActive = false;
            AbilityEnded?.Invoke();
        }

        if (_abilityCoroutine != null)
        {
            StopCoroutine(_abilityCoroutine);
            _abilityCoroutine = null;
        }
    }

    public float GetRemainingTimeNormalized()
    {
        if (_isAbilityActive == false)
            return NormalizedMin;

        return NormalizedMax - (_abilityTimer / _abilityDuration);
    }

    public float GetCooldownProgressNormalized()
    {
        if (_isAbilityReady)
            return CooldownCompleteThreshold;

        return _cooldownTimer / _abilityCooldown;
    }

    public float GetRemainingTime()
    {
        if (_isAbilityActive)
            return Mathf.Max(TimeRemainingThreshold, _abilityDuration - _abilityTimer);
        else if (_isAbilityReady == false)
            return Mathf.Max(TimeRemainingThreshold, _abilityCooldown - _cooldownTimer);
        else
            return TimeRemainingThreshold;
    }

    public void ApplyVampirismSettings(float duration, float cooldown, float tickInterval, int damagePerTick, float healRatio)
    {
        _abilityDuration = duration;
        _abilityCooldown = cooldown;
        _damageTickInterval = tickInterval;
        _damagePerTick = damagePerTick;
        _healRatio = healRatio;
    }

    private void InitializeReferences()
    {
        if (_vampirismZone == null)
            _vampirismZone = GetComponentInChildren<AttackZone>();

        if (_ownerHealth == null)
            _ownerHealth = GetComponent<BaseHealth>();

        _ownerTransform = transform;
    }

    private void ValidateSettings()
    {
        _abilityDuration = Mathf.Max(_abilityDuration, MinAbilityDuration);
        _abilityCooldown = Mathf.Max(_abilityCooldown, MinCooldown);
        _damageTickInterval = Mathf.Max(_damageTickInterval, MinTickInterval);
        _damagePerTick = Mathf.Max(_damagePerTick, MinDamagePerTick);
        _healRatio = Mathf.Clamp(_healRatio, MinHealRatio, MaxHealRatio);

        if (_vampirismZone == null)
            Debug.LogError($"AttackZone Component, not found for \"{GetType().Name}.cs\" on \"{gameObject.name}\" GameObject", this);
    }

    private void UpdateTimers()
    {
        if (_isAbilityActive)
            _abilityTimer += Time.deltaTime;
        else if (_isAbilityReady == false)
            _cooldownTimer += Time.deltaTime;
    }

    private IEnumerator VampirismRoutine()
    {
        float abilityTimer = ZeroDurationThreshold;

        while (abilityTimer < _abilityDuration && _isAbilityActive)
        {
            IDamageable nearestTarget = _vampirismZone?.FindNearestTarget(transform.position);

            if (nearestTarget != null)
            {
                int damageDealt = nearestTarget.TakeDamage(_damagePerTick);

                if (damageDealt > 0)
                {
                    int healAmount = Mathf.RoundToInt(damageDealt * _healRatio);
                    _ownerHealth.Heal(healAmount);
                }
            }

            abilityTimer += _damageTickInterval;
            yield return new WaitForSeconds(_damageTickInterval);
        }

        _isAbilityActive = false;
        AbilityEnded?.Invoke();

        yield return new WaitForSeconds(_abilityCooldown);
        _isAbilityReady = true;
        AbilityReady?.Invoke();
    }
}