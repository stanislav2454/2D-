using System.Collections;
using UnityEngine;

[RequireComponent(typeof(EnemyMover))]
public class EnemyAI : MonoBehaviour
{
    private const float WaypointReachThreshold = 0.5f;
    private const int WaypointIncrement = 1;

    public enum EnemyState { Patrolling, Chasing, Attacking }

    [Header("AI Settings")]
    [SerializeField] private float _attackRange = 0.12f;
    [SerializeField] private float _attackCooldown = 3f;
    [SerializeField] private int _attackDamage = 2;

    [Header("References")]
    [SerializeField] private EnemyMover _mover;
    [SerializeField] private Detector _detector;

    private EnemyPath _patrolPath;
    private Transform _player;
    private EnemyState _currentState = EnemyState.Patrolling;
    private int _currentWaypointIndex = 0;
    private bool _canAttack = true;
    private float _sqrAttackRange;
    private float _sqrWaypointReachThreshold;

    private void Awake()
    {
        _mover = GetComponent<EnemyMover>();

        CalculateSquaredRanges();
    }

    private void OnEnable()
    {
        _detector.TargetDetected += OnTargetDetected;
        _detector.TargetLost += OnTargetLost; ResetAI();
    }

    private void OnDisable()
    {
        _detector.TargetDetected -= OnTargetDetected;
        _detector.TargetLost -= OnTargetLost;
    }

    private void Update()
    {
        switch (_currentState)
        {
            case EnemyState.Patrolling:
                PatrolBehavior();
                break;
            case EnemyState.Chasing:
                ChaseBehavior();
                break;
            case EnemyState.Attacking:
                AttackBehavior();
                break;
        }
    }

    public void ResetAI()
    {
        _currentState = EnemyState.Patrolling;
        _canAttack = true;
        _currentWaypointIndex = 0;
        StopAllCoroutines();
    }

    public void SetPatrolPath(EnemyPath path)
    {
        _patrolPath = path;
        _currentWaypointIndex = 0;
    }

    private void CalculateSquaredRanges()
    {
        _sqrAttackRange = _attackRange * _attackRange;
        _sqrWaypointReachThreshold = WaypointReachThreshold * WaypointReachThreshold;
    }

    private void OnTargetDetected(Transform playerTransform)
    {
        _player = playerTransform;
        _currentState = EnemyState.Chasing;
    }

    private void OnTargetLost()
    {
        _player = null;
        _currentState = EnemyState.Patrolling;
    }

    private void PatrolBehavior()
    {
        if (_patrolPath != null && _patrolPath.Count > 0)
        {
            Transform waypoint = _patrolPath.GetWaypoint(_currentWaypointIndex);
            _mover.SetChasingSpeed(false);
            _mover.MoveToTarget(waypoint.position);

            if (Vector2.SqrMagnitude(transform.position - waypoint.position) < _sqrWaypointReachThreshold)
            {
                _currentWaypointIndex = (_currentWaypointIndex + WaypointIncrement) % _patrolPath.Count;
            }
        }
    }

    private void ChaseBehavior()
    {
        if (_player == null)
            return;

        _mover.SetChasingSpeed(true);
        _mover.MoveToTarget(_player.position);

        if (Vector2.SqrMagnitude(_player.position - transform.position) <= _sqrAttackRange)
        {
            _currentState = EnemyState.Attacking;
        }
    }

    private void AttackBehavior()
    {
        _mover.SetChasingSpeed(false);
        _mover.StopMovement();

        if (_canAttack && _player != null)
        {
            AttackPlayer();
            StartCoroutine(AttackCooldown());
        }

        if (_player == null || Vector2.SqrMagnitude(_player.position - transform.position) > _sqrAttackRange)
        {
            _currentState = EnemyState.Chasing;
        }
    }

    private void AttackPlayer()
    {
        if (_player == null)
            return;

        PlayerHealth playerHealth = _player.GetComponent<PlayerHealth>();
        playerHealth?.TakeDamage(_attackDamage);
    }

    private IEnumerator AttackCooldown()
    {
        _canAttack = false;
        yield return new WaitForSeconds(_attackCooldown);
        _canAttack = true;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _attackRange);
    }
}