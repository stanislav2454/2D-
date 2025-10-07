using System.Collections;
using UnityEngine;

public class EnemyAttacker : BaseAttacker
{
    [SerializeField] private EnemySettings _enemySettings;
    private Transform _player;
    private bool _isAttacking;

    protected override int AttackDamage => _enemySettings?.AttackDamage ?? 2;
    protected override float AttackCooldown => _enemySettings?.AttackCooldown ?? 3f;
    protected override float AttackRange => _enemySettings?.AttackRange ?? 0.12f;

    private void OnEnable()
    {
        _canAttack = true;
        _isAttacking = false;
    }

    private void OnDisable()
    {
        StopAttacking();
    }

    public void Initialize(Transform player) =>
        _player = player;

    public void StartAttacking()
    {
        if (_isAttacking || _player == null)
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

    public void ApplyEnemySettings(EnemySettings settings)
    {
        _enemySettings = settings;
        CalculateSqrAttackRange();
    }

    public override bool CanAttack()
    {
        if (_player == null)
            return false;

        return Vector2.SqrMagnitude(_player.position - transform.position) <= _sqrAttackRange;
    }

    public override void PerformAttack()
    {
        if (_canAttack && _player != null && _enemySettings != null)
        {
            int damageDealt = AttackPlayer();
            _canAttack = false; 
            StartCoroutine(ResetAttackCooldown()); 
            OnAttackPerformed(damageDealt);
        }
    }

    private IEnumerator AttackRoutine()
    {
        var shortDelay = new WaitForSeconds(0.1f);

        while (_isAttacking)
        {
            if (CanAttack())
            {
                PerformAttack();
                yield return new WaitForSeconds(AttackCooldown);
            }
            else
            {
                yield return shortDelay;
            }
        }
    }

    private IEnumerator ResetAttackCooldown()
    {
        yield return new WaitForSeconds(AttackCooldown);
        _canAttack = true;
    }

    private int AttackPlayer()
    {
        if (_player == null || _enemySettings == null)
            return 0;

        if (_player.TryGetComponent<PlayerHealth>(out PlayerHealth playerHealth))
            return playerHealth.TakeDamage(AttackDamage);

        return 0;
    }
}