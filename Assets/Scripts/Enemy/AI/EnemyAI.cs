using UnityEngine;

[RequireComponent(typeof(EnemyMover), typeof(EnemyAttacker))]
public class EnemyAI : MonoBehaviour
{
    private const float WaypointReachThreshold = 0.5f;
    private const int WaypointIncrement = 1;

    public enum EnemyState { Patrolling, Chasing, Attacking }

    [Header("References")]
    [SerializeField] private EnemySettings _settings;
    [SerializeField] private EnemyMover _mover;
    [SerializeField] private Detector _detector;
    [SerializeField] private EnemyAttacker _attacker;

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
        _attacker = GetComponent<EnemyAttacker>();

        CalculateSquaredRanges();
    }

    private void OnEnable()
    {
        _detector.TargetDetected += OnTargetDetected;
        _detector.TargetLost += OnTargetLost;
        ResetAI();
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

    public void ApplySettings(EnemySettings settings)
    {
        _settings = settings;
        _attacker.ApplyEnemySettings(settings);

        if (_detector != null)
            _detector.SetDetectionRadius(_settings.DetectionRadius);
    }

    private void CalculateSquaredRanges()
    {
        if (_settings != null)
            _sqrAttackRange = _settings.AttackRange * _settings.AttackRange;

        _sqrWaypointReachThreshold = WaypointReachThreshold * WaypointReachThreshold;
    }

    private void OnTargetDetected(Transform playerTransform)
    {
        _player = playerTransform;
        _attacker.Initialize(_player);
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
                _currentWaypointIndex = (_currentWaypointIndex + WaypointIncrement) % _patrolPath.Count;
        }
    }

    private void ChaseBehavior()
    {
        if (_player == null)
            return;

        _mover.SetChasingSpeed(true);
        _mover.MoveToTarget(_player.position);

        if (_attacker.CanAttack())
            _currentState = EnemyState.Attacking;
    }

    private void AttackBehavior()
    {
        _mover.SetChasingSpeed(false);
        _mover.StopMovement();

        _attacker.PerformAttack();

        if (_player == null || _attacker.CanAttack() == false)
            _currentState = EnemyState.Chasing;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _settings.AttackRange);
    }
}