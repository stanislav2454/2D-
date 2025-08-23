using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent, RequireComponent(typeof(Collider2D))]
public class AttackZone : MonoBehaviour
{
    public HashSet<IDamageable> TargetsInZone { get; private set; } = new HashSet<IDamageable>();

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent<IDamageable>(out var damageable))
            TargetsInZone.Add(damageable);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.TryGetComponent<IDamageable>(out var damageable))
            TargetsInZone.Remove(damageable);
    }


    public void CleanDestroyedTargets() =>
        TargetsInZone.RemoveWhere(target => target == null || target.Equals(null));

    public void ClearTargets() =>
        TargetsInZone.Clear();

    private void OnDrawGizmosSelected()
    {
        var collider = GetComponent<Collider2D>();

        if (collider == null)
            return;

        Gizmos.color = new Color(1f, 0f, 0f, 0.3f);

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