using UnityEngine;

[DisallowMultipleComponent, RequireComponent(typeof(Enemy))]
public class EnemyMover : MonoBehaviour
{
    [SerializeField] private float _speed = 5f;
    [SerializeField] private float _reachThreshold = 0.2f;
    [SerializeField] private float _rotationSpeed = 5f; 
    [SerializeField] private Flipper _flipper; 

    private EnemyPath _path;
    private int _currentWaypointIndex;
    private Transform _currentWaypoint;
    private bool _isMoving = true;

    private void Update()
    {
        if (_path == null || _isMoving == false)
            return;

        transform.position = MoveTowardsWaypoint(transform, _currentWaypoint, _speed);

        if (_currentWaypoint != null)
        {
            Vector3 direction = (_currentWaypoint.position - transform.position).normalized;

            if (_flipper != null && direction != Vector3.zero)
                _flipper.Flip(direction.x);
        }

        _currentWaypoint = IfWaypointReached(transform, _currentWaypoint, _reachThreshold, _path);
    }

    public void StopMovement()
    {
        _isMoving = false;
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