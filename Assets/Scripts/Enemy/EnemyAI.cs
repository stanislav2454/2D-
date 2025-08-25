using System.Collections;
using UnityEngine;

[RequireComponent(typeof(EnemyMover), typeof(Attacker))]
public class EnemyAI : MonoBehaviour
{
    private const float WaypointReachThreshhold = 0.5f;
    private const int IndexCorrector = 1;

    [SerializeField] private float _detectionRange = 4f;
    [SerializeField] private float _attackRange = 0.12f;
    [SerializeField] private float _attackCooldown = 3f;

    [Header("References")]
    [SerializeField] private EnemyPath _patrolPath;
    [SerializeField] private LayerMask _playerLayer;
    [SerializeField] private EnemyMover _mover;
    [SerializeField] private Transform _player;
    [SerializeField] private Attacker _attacker;

    private int _currentWaypointIndex = 0;
    private bool _canAttack = true;
    private float _sqrDetectionRange;
    private float _sqrAttackRange;
    private float _sqrWaypointReachThreshold;
    private EnemyState _currentState = EnemyState.Patrolling;

    public enum EnemyState { Patrolling, Chasing, Attacking }

    private void Awake()
    {
        _mover = GetComponent<EnemyMover>();
        _attacker = GetComponent<Attacker>();
        CalculateSquaredRanges();
    }

    private void OnEnable()
    {
        ResetAI();
    }

    private void OnValidate()
    {
        if (_player == null)
            Debug.LogWarning("Transform player is not set!", this);

        if (_patrolPath == null)
            Debug.LogWarning("PatrolPath is not set!", this);

        if (_playerLayer == null)
            Debug.LogWarning("PlayerLayer is not set!", this);
    }

    private void Update()
    {
        if (_player == null)
            return;

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

    public void SetPlayerTransform(Transform playerTransform)
    {
        if (playerTransform != null && playerTransform.GetComponent<Character>())
            _player = playerTransform;
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
        _sqrDetectionRange = _detectionRange * _detectionRange;
        _sqrAttackRange = _attackRange * _attackRange;
        _sqrWaypointReachThreshold = WaypointReachThreshhold * WaypointReachThreshhold;
    }

    private void PatrolBehavior()
    {
        if (_patrolPath != null && _patrolPath.Count > 0)
        {
            Transform waypoint = _patrolPath.GetWaypoint(_currentWaypointIndex);
            _mover.SetChasing(false);
            _mover.MoveToTarget(waypoint.position);

            if (GetSqrDistanceBetween(transform.position, waypoint.position) < _sqrWaypointReachThreshold)
                _currentWaypointIndex = (_currentWaypointIndex + IndexCorrector) % _patrolPath.Count;
        }

        if (CanSeePlayer())
        {
            _currentState = EnemyState.Chasing;
        }
    }

    private void ChaseBehavior()
    {
        _mover.SetChasing(true);
        _mover.MoveToTarget(_player.position);

        if (_player != null && GetSqrDistanceBetween(_player.position, transform.position) <= _sqrAttackRange)
            _currentState = EnemyState.Attacking;
        else if (_player == null || GetSqrDistanceBetween(_player.position, transform.position) > _sqrDetectionRange || CanSeePlayer() == false)
            _currentState = EnemyState.Patrolling;
    }

    private void AttackBehavior()
    {
        _mover.SetChasing(false);
        _mover.StopMovement();

        if (_canAttack && _player != null)
        {
            _attacker.AttackOnce();
            StartCoroutine(AttackCooldown());
        }

        if (_player == null || GetSqrDistanceBetween(_player.position, transform.position) > _sqrAttackRange)
        {
            _currentState = EnemyState.Chasing;
        }
    }

    private IEnumerator AttackCooldown()
    {
        _canAttack = false;
        yield return new WaitForSeconds(_attackCooldown);
        _canAttack = true;
    }

    private bool CanSeePlayer()
    {
        if (_player == null)
            return false;

        if (GetSqrDistanceBetween(_player.position, transform.position) > _sqrDetectionRange)
            return false;

        RaycastHit2D hit = Physics2D.Raycast(
            transform.position,
            _player.position - transform.position,
            _detectionRange,
            _playerLayer);

        return hit.collider != null && hit.collider.GetComponent<Character>();
    }

    private float GetSqrDistanceBetween(Vector2 position, Vector2 target) =>
         (position - target).sqrMagnitude;

    private void OnDrawGizmosSelected()
    {
        // Зона обнаружения
        Utils.DrawZone(Color.yellow, transform.position, _detectionRange);

        // Зона атаки
        Utils.DrawZone(Color.red, transform.position, _attackRange);

        // Линия взгляда к игроку
        if (_player != null)
        {
            Gizmos.color = Color.white;
            Gizmos.DrawLine(transform.position, _player.position);
        }
    }
}