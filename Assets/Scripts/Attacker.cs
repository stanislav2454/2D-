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

    [Header("Lifesteal Settings")]
    [SerializeField] [Range(0f, 1f)] private float _healRatio = 0f;
    [SerializeField] private BaseHealth _ownerHealth;

    private Coroutine _attackCoroutine;
    private bool _isAttacking;

    public event System.Action<int> OnDamageDealt;
    public event System.Action<int> OnHealed;

    private void Awake()
    {
        if (_attackZone == null)
            _attackZone = GetComponentInChildren<AttackZone>();

        if (_ownerHealth == null)
            _ownerHealth = GetComponent<BaseHealth>();
    }

    public void AttackOnce()
    {
        if (_isAttacking) return;

        _isAttacking = true;

        if (_attackZone != null)
        {
            _attackZone.CleanDestroyedTargets();
            if (_attackZone.TargetsInZone.Count > 0)
                Attack();
        }

        _isAttacking = false;
    }

    public void StartAttacking()
    {
        if (_isAttacking) 
            return;

        _isAttacking = true;

        if (_attackCoroutine != null)
            StopCoroutine(_attackCoroutine);

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
                yield return new WaitForSeconds(_damageInterval);

                if (_attackZone.TargetsInZone.Count > 0)
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
        if (_attackZone == null || _attackZone.TargetsInZone.Count == 0)
            return;

        int totalDamageDealt = CalculateDamageToTargets(_attackZone, _damage);
        OnDamageDealt?.Invoke(totalDamageDealt);

        if (_healRatio > 0 && _ownerHealth != null)
        {
            int healAmount = Mathf.RoundToInt(totalDamageDealt * _healRatio);
            _ownerHealth.Heal(healAmount);
            OnHealed?.Invoke(healAmount);
        }
    }

    private int CalculateDamageToTargets(AttackZone attackZone, int damage)
    {
        int totalDamageDealt = 0;
        var targets = new List<IDamageable>(attackZone.TargetsInZone);

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

    public void SetDamage(int newDamage) =>
        _damage = newDamage;

    public void SetDamageInterval(float newInterval) =>
        _damageInterval = newInterval;

    public void SetHealRatio(float newRatio) =>
        _healRatio = Mathf.Clamp01(newRatio);
}