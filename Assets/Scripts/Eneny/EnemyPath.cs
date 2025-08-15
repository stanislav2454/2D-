using UnityEngine;

public class EnemyPath : MonoBehaviour
{
    [SerializeField] private Transform[] _waypoints;

    public Transform[] Waypoints => _waypoints;
    public int Count => _waypoints.Length;

    public Transform GetWaypoint(int index)
    {
        if (_waypoints == null || index < 0 || index >= _waypoints.Length)
            return null;

        return _waypoints[index];
    }

    public Vector3 GetFirstWaypointPosition() =>
        _waypoints[0].position;
}