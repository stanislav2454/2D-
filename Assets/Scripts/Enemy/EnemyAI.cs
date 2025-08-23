using System.Collections;
using UnityEngine;

[RequireComponent(typeof(EnemyMover))]
public class EnemyAI : MonoBehaviour
{
    private const float WaypointReachThreshhold = 0.5f;
    private const int IndexCorrector = 1;

    [Header("AI Settings")]
    [SerializeField] private float _detectionRange = 4f;
    [SerializeField] private float _attackRange = 0.12f;
    [SerializeField] private float _attackCooldown = 3f;
    [SerializeField] private int _attackDamage = 2;

    [Header("References")]
    [SerializeField] private EnemyPath _patrolPath;
    [SerializeField] private LayerMask _playerLayer;
    [SerializeField] private EnemyMover _mover;
    [SerializeField] private Transform _player;

    private int _currentWaypointIndex = 0;
    private bool _canAttack = true;
    private bool _playerFound = false;

    private float _sqrDetectionRange;
    private float _sqrAttackRange;
    private float _sqrWaypointReachThreshold;
    private EnemyState _currentState = EnemyState.Patrolling;

    public enum EnemyState { Patrolling, Chasing, Attacking }

    private void Awake()
    {
        _mover = GetComponent<EnemyMover>();
        CalculateSquaredRanges();
        FindPlayer();
    }

    private void OnEnable()
    {
        ResetAI();
    }

    private void Update()
    {
        if (_player == null || _playerFound == false)
        {
            FindPlayer();

            if (_player == null || _playerFound == false)
                return;
        }

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

    private void CalculateSquaredRanges()
    {
        _sqrDetectionRange = _detectionRange * _detectionRange;
        _sqrAttackRange = _attackRange * _attackRange;
        _sqrWaypointReachThreshold = WaypointReachThreshhold * WaypointReachThreshhold;
    }

    private void FindPlayer()
    {
        if (_player != null)
        {
            _playerFound = true;
            return;
        }

        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");

        if (playerObject != null)
        {
            _player = playerObject.transform;
            _playerFound = true;
        }
        else
        {
            enabled = false;
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

    private void PatrolBehavior()
    {
        if (_patrolPath != null && _patrolPath.Count > 0)
        {
            Transform waypoint = _patrolPath.GetWaypoint(_currentWaypointIndex);
            _mover.SetChasing(false);
            _mover.MoveToTarget(waypoint.position);

            float sqrDistanceToWaypoint = (transform.position - waypoint.position).sqrMagnitude;

            if (sqrDistanceToWaypoint < _sqrWaypointReachThreshold)
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

        float sqrDistanceToPlayer = (_player.position - transform.position).sqrMagnitude;

        if (_player != null && sqrDistanceToPlayer <= _sqrAttackRange)
            _currentState = EnemyState.Attacking;
        else if (_player == null || sqrDistanceToPlayer > _sqrDetectionRange || CanSeePlayer() == false)
            _currentState = EnemyState.Patrolling;
    }

    private void AttackBehavior()
    {
        _mover.SetChasing(false); 
        _mover.StopMovement();

        if (_canAttack && _player != null)
        {
            AttackPlayer();
            StartCoroutine(AttackCooldown());
        }

        float sqrDistanceToPlayer = (_player.position - transform.position).sqrMagnitude;
       
        if (_player == null || sqrDistanceToPlayer > _sqrAttackRange)
        {
            _currentState = EnemyState.Chasing;
        }
    }

    private void AttackPlayer()
    {
        if (_player == null)
            return;

        PlayerHealth playerHealth = _player.GetComponent<PlayerHealth>();

        if (playerHealth != null)        
            playerHealth.TakeDamage(_attackDamage);        
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

        float sqrDistanceToPlayer = (_player.position - transform.position).sqrMagnitude;

        if (sqrDistanceToPlayer > _sqrDetectionRange) return false;

        RaycastHit2D hit = Physics2D.Raycast(
            transform.position,
            _player.position - transform.position,
            _detectionRange,
            _playerLayer);

        return hit.collider != null && hit.collider.CompareTag("Player");
    }

    private void OnDrawGizmosSelected()
    {
        // Зона обнаружения
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, _detectionRange);

        // Зона атаки
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _attackRange);

        // Линия взгляда к игроку
        if (_player != null)
        {
            Gizmos.color = Color.white;
            Gizmos.DrawLine(transform.position, _player.position);
        }
    }
}