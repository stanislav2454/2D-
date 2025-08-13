using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private float _speed = 5f;
    [SerializeField] private float _rotationSpeed = 5f;
    [SerializeField] private float _reachThreshold = 0.2f;
    [SerializeField] private float _rotationThreshold = 0.01f;

    private EnemyPath _path;
    private int _currentWaypointIndex;
    private Transform _currentWaypoint;

    private void Start()
    {
        transform.rotation = GetComponentInParent<Enemy>().transform.rotation;
    }

    private void Update()
    {
        if (_path == null)
            return;

        transform.position = MoveTowardsWaypoint(transform, _currentWaypoint, _speed);
        _currentWaypoint = CheckWaypointReached(transform, _currentWaypoint, _reachThreshold, _path);
    }

    public void Initialize(EnemyPath path)
    {
        if (path == null || path.Count == 0)
            return;

        _path = path;
        _currentWaypointIndex = 0;
        _currentWaypoint = _path.GetWaypoint(_currentWaypointIndex);
    }

    private Vector3 MoveTowardsWaypoint(Transform current, Transform currentWaypoint, float speed)
    {
        Vector3 position = Vector3.MoveTowards(
            current.position, currentWaypoint.position, speed * Time.deltaTime);

        return position;
    }


    private Transform CheckWaypointReached(
        Transform current, Transform currentWaypoint, float reachThreshold, EnemyPath path)
    {
        float distance = Vector3.Distance(current.position, currentWaypoint.position);

        if (distance < reachThreshold)
        {
            _currentWaypointIndex++;

            if (_currentWaypointIndex >= path.Count)
                _currentWaypointIndex = 0;

            return path.GetWaypoint(_currentWaypointIndex);
        }

        return currentWaypoint;
    }
}