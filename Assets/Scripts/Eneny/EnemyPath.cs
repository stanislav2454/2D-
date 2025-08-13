using UnityEngine;

public class EnemyPath : MonoBehaviour
{
    [SerializeField] private Transform[] _waypoints;

    public Transform[] Waypoints => _waypoints;
    public int Count => _waypoints.Length;
    public Transform GetWaypoint(int index) => _waypoints[index];
    public Vector3 GetFirstWaypointPosition() => _waypoints[0].position;
}
