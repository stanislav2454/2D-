using System.Collections;
using UnityEngine;

public class EnemyAttacker : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private EnemySettings _settings;

    private Transform _player;
    private bool _canAttack = true;
    private float _sqrAttackRange;

    public event System.Action AttackPerformed;

    private void Awake()
    {
        CalculateSquaredRanges();
    }

    public void Initialize(Transform player)
    {
        _player = player;
    }

    public bool CanAttackPlayer()
    {
        if (_player == null || _settings == null)
            return false;

        return Vector2.SqrMagnitude(_player.position - transform.position) <= _sqrAttackRange;
    }

    public void PerformAttack()
    {
        if (_canAttack && _player != null)
        {
            AttackPlayer();
            StartCoroutine(AttackCooldown());
        }
    }

    private void AttackPlayer()
    {
        if (_player == null || _settings == null)
            return;

        PlayerHealth playerHealth = _player.GetComponent<PlayerHealth>();
        playerHealth?.TakeDamage(_settings.AttackDamage);

        AttackPerformed?.Invoke();
    }

    private IEnumerator AttackCooldown()
    {
        _canAttack = false;
        yield return new WaitForSeconds(_settings.AttackCooldown);
        _canAttack = true;
    }

    public void ApplySettings(EnemySettings settings)
    {
        _settings = settings;
        CalculateSquaredRanges();
    }

    private void CalculateSquaredRanges()
    {
        if (_settings != null)
            _sqrAttackRange = _settings.AttackRange * _settings.AttackRange;
    }
}