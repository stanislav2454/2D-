using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D)), DisallowMultipleComponent]
public class GroundDetector : MonoBehaviour
{
    private readonly HashSet<Collider2D> GroundContacts = new HashSet<Collider2D>();

    [Header("Ground Settings"), Tooltip("Максимально допустимый угол поверхности, которая считается \"землёй\". 1 = 0° (пол), 0 = 90° (стена)")]
    [SerializeField, Range(0, 1)] private float _maxGroundAngle = 0.7f;

    private bool _wasGroundedLastFrame;

    public bool IsGrounded => GroundContacts.Count > 0;
    public bool JustLanded { get; private set; }
    public bool JustLeftGround { get; private set; }

    private void FixedUpdate()
    {
        JustLanded = _wasGroundedLastFrame == false && IsGrounded;
        JustLeftGround = _wasGroundedLastFrame && IsGrounded == false;
        _wasGroundedLastFrame = IsGrounded;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.TryGetComponent<Ground>(out _) == false)
            return;

        foreach (ContactPoint2D contact in collision.contacts)
        {
            if (IsValidSurfaceAngle(contact.normal))
            {
                GroundContacts.Add(collision.collider);
                break;
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (GroundContacts.Contains(collision.collider))
            GroundContacts.Remove(collision.collider);
    }

    private bool IsValidSurfaceAngle(Vector2 surfaceNormal) =>
         Vector2.Dot(surfaceNormal, Vector2.up) >= _maxGroundAngle;
}