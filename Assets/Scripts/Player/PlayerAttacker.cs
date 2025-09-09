using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class PlayerAttacker : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerSettings _settings;
    [SerializeField] private AttackZone _attackZone;
    [SerializeField] private CharacterAnimator _animator;

    private Coroutine _attackCoroutine;
    private bool _isAttacking;
    private bool _canAttack = true;

    public event System.Action AttackPerformed;

    private void Awake()
    {
        if (_attackZone == null)
            _attackZone = GetComponentInChildren<AttackZone>();

        if (_animator == null)
            _animator = GetComponentInChildren<CharacterAnimator>();
    }

    private void OnDisable()
    {
        StopAttacking();
    }

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
                    yield return StartCoroutine(AttackCooldown());
                }
                else
                {
                    yield return new WaitForSeconds(0.1f);//
                }
                //yield return new WaitForSeconds(_damageInterval);

                //if (_attackZone.TargetsInZoneCount > 0)
                //    Attack();
            }
            else
            {
                yield return null;
            }
        }
    }

    private void PerformAttack()
    {
        if (_attackZone == null || _attackZone.TargetsInZoneCount == 0 || _settings == null)
            return;

        int totalDamageDealt = CalculateDamageToTargets(_attackZone, _settings.AttackDamage);
        AttackPerformed?.Invoke();
        _animator?.PlayAttackAnimation();
    }

    private int CalculateDamageToTargets(AttackZone attackZone, int damage)
    {
        int totalDamageDealt = 0;
        IReadOnlyCollection<IDamageable> targets = attackZone.Targets;

        foreach (var target in targets)
        {
            if (target != null)
            {
                int damageDealt = target.TakeDamage(damage);
                totalDamageDealt += damageDealt;
            }
        }

        return totalDamageDealt;
    }

    private IEnumerator AttackCooldown()
    {
        _canAttack = false;
        yield return new WaitForSeconds(_settings.AttackCooldown);
        _canAttack = true;
    }

    public void ApplySettings(PlayerSettings settings)
    {
        _settings = settings;
    }
}