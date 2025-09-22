using UnityEngine;

[DisallowMultipleComponent, RequireComponent(typeof(Enemy))]
public class EnemyMover : MonoBehaviour
{
    [SerializeField] private EnemySettings _settings;
    [SerializeField] private Flipper _flipper;

    private float _reachThreshold = 0.2f;

    private float _currentSpeed;
    private int _currentWaypointIndex;
    private bool _isMoving = true;
    private EnemyPath _path;
    private Transform _currentWaypoint;
    private Vector2 _targetPosition;

    private void Start()
    {
        if (_settings != null)
            _currentSpeed = _settings.WalkSpeed;
        else
            Debug.LogWarning("EnemySettings not assigned!", this);
    }

    private void Update()
    {
        if (_isMoving && _targetPosition != Vector2.zero)
            MoveToTarget(_targetPosition);
        else if (_path != null && _isMoving && _currentWaypoint != null)
            PatrolUpdate();
    }

    public void SetChasingSpeed(bool isChasing)
    {
        if (_settings != null)
            _currentSpeed = isChasing ? _settings.ChaseSpeed : _settings.WalkSpeed;
    }

    public void MoveToTarget(Vector2 target)
    {
        _targetPosition = target;
        transform.position = Vector2.MoveTowards(transform.position, target, _currentSpeed * Time.deltaTime);

        if (_flipper != null)
        {
            float direction = Mathf.Sign(target.x - transform.position.x);
            _flipper.Flip(direction);
        }
    }

    public void StartMovement()
    {
        _isMoving = true;
    }

    public void StopMovement()
    {
        _isMoving = false;
        _targetPosition = Vector2.zero;
    }

    public void Initialize(EnemyPath path)
    {
        if (path == null || path.Count == 0)
            return;

        _path = path;
        _currentWaypointIndex = 0;
        _currentWaypoint = _path.GetWaypoint(_currentWaypointIndex);
        _isMoving = true;
    }

    public void ApplySettings(EnemySettings settings)//
    {
        _settings = settings;
        _currentSpeed = _settings.WalkSpeed;
    }

    private void PatrolUpdate()
    {
        transform.position = MoveTowardsWaypoint(transform, _currentWaypoint, _currentSpeed);

        if (_currentWaypoint != null)
        {
            Vector3 direction = (_currentWaypoint.position - transform.position).normalized;

            if (_flipper != null && direction != Vector3.zero)
                _flipper.Flip(direction.x);
        }

        _currentWaypoint = IfWaypointReached(transform, _currentWaypoint, _reachThreshold, _path);
    }

    private Vector3 MoveTowardsWaypoint(Transform current, Transform currentWaypoint, float speed) =>
          Vector3.MoveTowards(current.position, currentWaypoint.position, speed * Time.deltaTime);

    private Transform IfWaypointReached(
         Transform current, Transform currentWaypoint, float reachThreshold, EnemyPath path)
    {
        float sqrDistance = (current.position - currentWaypoint.position).sqrMagnitude;
        float sqrReachThreshold = reachThreshold * reachThreshold;

        if (sqrDistance < sqrReachThreshold)
        {
            _currentWaypointIndex++;

            if (_currentWaypointIndex >= path.Count)
                _currentWaypointIndex = 0;

            return path.GetWaypoint(_currentWaypointIndex);
        }

        return currentWaypoint;
    }
}