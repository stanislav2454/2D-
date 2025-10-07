using UnityEngine;

[DisallowMultipleComponent, RequireComponent(typeof(Enemy))]
public class EnemyMover : BaseMover
{
    [SerializeField] private EnemySettings _settings;
    [SerializeField] private float _reachThreshold = 0.2f;

    private EnemyPath _patrolPath;
    private Transform _currentWaypoint;
    private int _currentWaypointIndex;
    private bool _isChasing;
    private Vector2 _chaseTarget;
    private Vector2 _movementDirection;

    protected override void Awake()
    {
        Flipper = GetComponent<Flipper>();

        if (_settings != null)
        {
            CurrentSpeed = _settings.WalkSpeed;
            Acceleration = _settings.Acceleration;
        }
    }

    private void Update()
    {
        UpdateMovementDirection();
        ApplyMovementDirect();
    }

    protected override float GetCurrentSpeed() =>
        CurrentSpeed;

    public void SetChasingSpeed(bool isChasing)
    {
        if (_settings != null)
            CurrentSpeed = isChasing ? _settings.ChaseSpeed : _settings.WalkSpeed;

        _isChasing = isChasing;
    }

    public void StartPatrol(EnemyPath path)
    {
        _patrolPath = path;
        _isChasing = false;
        _chaseTarget = Vector2.zero;

        if (path != null && path.Count > 0)
        {
            _currentWaypointIndex = 0;
            _currentWaypoint = _patrolPath.GetWaypoint(_currentWaypointIndex);
        }
    }

    public void StartChasing(Vector2 target)
    {
        _chaseTarget = target;
        _isChasing = true;
    }

    public void UpdateChaseTarget(Vector2 target)
    {
        if (_isChasing)
            _chaseTarget = target;
    }

    public void ApplySettings(EnemySettings settings)
    {
        _settings = settings;
        CurrentSpeed = _settings.WalkSpeed;
        Acceleration = _settings.Acceleration;
    }

    public new void StopMovement() =>
        _movementDirection = Vector2.zero;

    public void MoveToTarget(Vector2 target) =>
        StartChasing(target);

    public void Initialize(EnemyPath path) =>
        StartPatrol(path);

    private void UpdateMovementDirection()
    {
        _movementDirection = Vector2.zero;

        if (_isChasing && _chaseTarget != Vector2.zero)
        {
            Vector2 direction = (_chaseTarget - (Vector2)transform.position).normalized;
            _movementDirection = direction;
        }
        else if (_patrolPath != null && _patrolPath.Count > 0)
        {
            if (_currentWaypoint == null)
            {
                _currentWaypointIndex = 0;
                _currentWaypoint = _patrolPath.GetWaypoint(_currentWaypointIndex);
            }

            Vector2 direction = (_currentWaypoint.position - transform.position).normalized;
            _movementDirection = direction;

            float sqrDistance = ((Vector2)transform.position - (Vector2)_currentWaypoint.position).sqrMagnitude;
            float sqrReachThreshold = _reachThreshold * _reachThreshold;

            if (sqrDistance < sqrReachThreshold)
            {
                _currentWaypointIndex = (_currentWaypointIndex + 1) % _patrolPath.Count;
                _currentWaypoint = _patrolPath.GetWaypoint(_currentWaypointIndex);
            }
        }
    }

    private void ApplyMovementDirect()
    {
        if (_movementDirection != Vector2.zero)
        {
            Vector2 newPosition = (Vector2)transform.position + _movementDirection * (CurrentSpeed * Time.deltaTime);
            transform.position = newPosition;

            if (Flipper != null && _movementDirection.x != 0)
                Flipper.Flip(_movementDirection.x);
        }
    }
}