using UnityEngine;

public class EnemyAttacker : BaseAttacker
{
    [SerializeField] private EnemySettings _enemySettings;
    private Transform _player;

    protected override int AttackDamage => _enemySettings?.AttackDamage ?? 2;
    protected override float AttackCooldown => _enemySettings?.AttackCooldown ?? 3f;
    protected override float AttackRange => _enemySettings?.AttackRange ?? 0.12f;

    private void OnEnable()
    {
        _canAttack = true;
    }

    public void Initialize(Transform player) =>
        _player = player;

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
            _attackCooldownCoroutine = StartCoroutine(AttackCooldownRoutine());
            OnAttackPerformed(damageDealt);
        }
    }

    private int AttackPlayer()
    {
        if (_player == null || _enemySettings == null)
            return 0;

        PlayerHealth playerHealth = _player.GetComponent<PlayerHealth>();

        if (playerHealth != null)
            return playerHealth.TakeDamage(AttackDamage);

        return 0;
    }

    public void ApplyEnemySettings(EnemySettings settings)
    {
        _enemySettings = settings;
        CalculateSqrAttackRange();
    }
}