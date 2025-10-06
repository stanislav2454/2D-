using System.Collections;
using UnityEngine;

[DisallowMultipleComponent]
public class PlayerAttacker : BaseAttacker
{
    [SerializeField] private PlayerSettings _playerSettings;
    [SerializeField] private AttackZone _attackZone;
    [SerializeField] private CharacterAnimator _animator;

    private bool _isAttacking;

    protected override int AttackDamage => _playerSettings?.AttackDamage ?? 1;
    protected override float AttackCooldown => _playerSettings?.AttackCooldown ?? 0.5f;
    protected override float AttackRange => 0f;

    private new void Awake()
    {
        base.Awake();

        if (_attackZone == null)
            _attackZone = GetComponentInChildren<AttackZone>();

        if (_animator == null)
            Debug.LogError($"CharacterAnimator Component, not found for \"{GetType().Name}.cs\" on \"{gameObject.name}\" GameObject", this);
    }

    private void OnDisable() =>
        StopAttacking();

    public void StartAttacking()
    {
        if (_isAttacking || _canAttack == false)
            return;

        _isAttacking = true;
        _attackCoroutine = StartCoroutine(AttackRoutine());
    }

    public void StopAttacking()
    {
        _isAttacking = false;

        if (_attackCoroutine != null)
        {
            StopCoroutine(_attackCoroutine);
            _attackCoroutine = null;
        }
    }

    private IEnumerator AttackRoutine()
    {
        while (_isAttacking)
        {
            if (_attackZone != null)
            {
                _attackZone.CleanDestroyedTargets();

                if (_attackZone.TargetsInZoneCount > 0)
                {
                    PerformAttack();
                    yield return StartCoroutine(AttackCooldownRoutine());
                }
                else
                {
                    yield return new WaitForSeconds(TargetCheckInterval);
                }
            }
            else
            {
                yield return null;
            }
        }
    }

    public override void PerformAttack()
    {
        int totalDamageDealt = CalculateDamageToTargets(_attackZone, AttackDamage);
        OnAttackPerformed(totalDamageDealt);
        _animator?.PlayAttackAnimation();
    }

    private int CalculateDamageToTargets(AttackZone attackZone, int damage)
    {
        int totalDamageDealt = 0;

        foreach (var target in attackZone.Targets)
        {
            if (target != null)
            {
                int actualDamage = target.TakeDamage(damage);
                totalDamageDealt += actualDamage;
            }
        }

        return totalDamageDealt;
    }

    public override bool CanAttack() =>
        _canAttack && _attackZone != null && _attackZone.TargetsInZoneCount > 0;

    public void ApplyPlayerSettings(PlayerSettings settings) =>
        _playerSettings = settings;
}