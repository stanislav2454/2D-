using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class Attacker : MonoBehaviour
{
    [Header("Attack Settings")]
    [SerializeField] private float _damageInterval = 0.5f;
    [SerializeField] private int _damage = 1;
    [SerializeField] private AttackZone _attackZone;
    [SerializeField] private CharacterAnimator _animator;

    private Coroutine _attackCoroutine;
    private bool _isAttacking;

    public event System.Action Attacked;

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
        if (_isAttacking)
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
                yield return new WaitForSeconds(_damageInterval);

                if (_attackZone.TargetsInZoneCount > 0)
                    Attack();
            }
            else
            {
                yield return null;
            }
        }
    }

    private void Attack()
    {
        if (_attackZone == null || _attackZone.TargetsInZoneCount == 0)
            return;

        int totalDamageDealt = CalculateDamageToTargets(_attackZone, _damage);
        Attacked?.Invoke();
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
}