using System;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class Detector : MonoBehaviour
{
    [SerializeField] private float _detectionRadius = 4f;
    private CircleCollider2D _detectZone;

    public event Action<Transform> TargetDetected;
    public event Action TargetLost;

    private void Awake()
    {
        _detectZone = GetComponent<CircleCollider2D>();
        _detectZone.isTrigger = true;
        _detectZone.radius = _detectionRadius;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<PlayerHealth>())
            TargetDetected?.Invoke(collision.transform);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<PlayerHealth>())
            TargetLost?.Invoke();
    }

    private void OnValidate()
    {
        if (_detectZone != null)
            _detectZone.radius = _detectionRadius;
    }

    public void SetDetectionRadius(float radius)
    {
        _detectionRadius = radius;

        if (_detectZone != null)
            _detectZone.radius = _detectionRadius;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, _detectionRadius);
    }
}