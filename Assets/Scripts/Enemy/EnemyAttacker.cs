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
        CanAttack = true;
        _isAttacking = false;
    }

    private new void OnDisable()
    {
        StopAttacking();
        base.OnDisable();
    }

    public void Initialize(Transform player) =>
        _player = player;

    public void StartAttacking()
    {
        if (_isAttacking || _player == null)
            return;

        _isAttacking = true;
        AttackCoroutine = StartCoroutine(AttackRoutine());
    }

    public void StopAttacking()
    {
        _isAttacking = false;
        StopAttackCoroutine();
    }

    public void ApplyEnemySettings(EnemySettings settings)
    {
        _enemySettings = settings;
        CalculateSqrAttackRange();
    }

    public override bool IsAbleToAttack()
    {
        if (_player == null)
            return false;

        return Vector2.SqrMagnitude(_player.position - transform.position) <= SqrAttackRange;
    }

    public override void PerformAttack()
    {
        if (CanAttack && _player != null && _enemySettings != null)
        {
            int damageDealt = AttackPlayer();
            CanAttack = false;
            StartCoroutine(ResetAttackCooldown());
            OnAttackPerformed(damageDealt);
        }
    }

    private IEnumerator AttackRoutine()
    {
        var shortDelay = new WaitForSeconds(0.1f);

        while (_isAttacking)
        {
            if (IsAbleToAttack())
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
        CanAttack = true;
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