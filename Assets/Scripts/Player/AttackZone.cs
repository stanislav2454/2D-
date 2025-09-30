using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent, RequireComponent(typeof(Collider2D))]
public class AttackZone : MonoBehaviour
{
    private HashSet<IDamageable> _targetsInZone = new HashSet<IDamageable>();

    public int TargetsInZoneCount => _targetsInZone.Count;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent<IDamageable>(out var damageable))
            _targetsInZone.Add(damageable);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.TryGetComponent<IDamageable>(out var damageable))
            _targetsInZone.Remove(damageable);
    }

    public IReadOnlyCollection<IDamageable> Targets
    {
        get
        {
            CleanDestroyedTargets();
            return new List<IDamageable>(_targetsInZone).AsReadOnly();
        }
    }

    public IDamageable FindNearestTarget() =>
         FindNearestTarget(transform.position);

    public IDamageable FindNearestTarget(Vector3 fromPosition)
    {
        CleanDestroyedTargets();

        if (_targetsInZone.Count == 0)
            return null;

        IDamageable nearestTarget = null;
        float nearestDistance = float.MaxValue;

        foreach (var target in _targetsInZone)
        {
            if (target == null || target is MonoBehaviour behaviour && behaviour == null)
                continue;

            Vector3 targetPosition = ((MonoBehaviour)target).transform.position;
            float sqrDistance = Vector3.SqrMagnitude(targetPosition - fromPosition);

            if (sqrDistance < nearestDistance)
            {
                nearestDistance = sqrDistance;
                nearestTarget = target;
            }
        }

        return nearestTarget;
    }

    public void CleanDestroyedTargets() =>
          _targetsInZone.RemoveWhere(target =>
                target == null || target.Equals(null) ||
               (target is MonoBehaviour behaviour && behaviour == null));

    public void ClearTargets() =>
        _targetsInZone.Clear();

    private void OnDrawGizmosSelected()
    {
        var collider = GetComponent<Collider2D>();

        if (collider == null)
            return;

        Gizmos.color = new Color(1f, 0f, 0f, 0.1f);

        switch (collider)
        {
            case BoxCollider2D boxCollider:
                Gizmos.DrawCube(transform.position + (Vector3)boxCollider.offset, boxCollider.size);
                break;
            case CircleCollider2D circleCollider:
                Gizmos.DrawSphere(transform.position + (Vector3)circleCollider.offset, circleCollider.radius);
                break;
        }
    }
}