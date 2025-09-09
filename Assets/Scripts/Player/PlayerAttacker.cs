using System.Collections;
using UnityEngine;

[DisallowMultipleComponent]
public class PlayerAttacker : BaseAttacker
{
    [SerializeField] private PlayerSettings _playerSettings;
    [SerializeField] private AttackZone _attackZone;
    [SerializeField] private CharacterAnimator _animator;

    private Coroutine _attackCoroutine;
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
            _animator = GetComponentInChildren<CharacterAnimator>();
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
        _animator?.StopAttackAnimation();
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
                    yield return new WaitForSeconds(TARGET_CHECK_INTERVAL);
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
        CalculateDamageToTargets(_attackZone, AttackDamage);
        OnAttackPerformed();
        _animator?.PlayAttackAnimation();
    }

    private void CalculateDamageToTargets(AttackZone attackZone, int damage)
    {
        foreach (var target in attackZone.Targets)
        {
            if (target != null)
                target.TakeDamage(damage);
        }
    }

    public override bool CanAttack() =>
        _canAttack && _attackZone != null && _attackZone.TargetsInZoneCount > 0;

    public void ApplyPlayerSettings(PlayerSettings settings) =>
        _playerSettings = settings;
}