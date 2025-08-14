using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class GroundDetector : MonoBehaviour
{
    [Header("Ground Settings"), Tooltip("Максимально допустимый угол поверхности, которая считается \"землёй\". 1 = 0° (пол), 0 = 90° (стена)")]
    [SerializeField, Range(0, 1)] private float _maxGroundAngle = 0.7f;
    // 1.0f (0°) - Только строго горизонтальные поверхности.
    // 0.7f (~45°)  cos(45°) ≈ 0.707 - Позволяет стоять на склонах до 45 градусов
    // 0.5f (~60°) cos(60°) = 0.5 - Персонаж сможет стоять на крутых склонах.
    // 0.0f (90°) cos(90°) = 0 - Любая поверхность. Персонаж будет "стоять" даже на вертикальных стенах
    private readonly HashSet<Collider2D> _groundContacts = new HashSet<Collider2D>();

    private bool _wasGroundedLastFrame;

    public bool IsGrounded => _groundContacts.Count > 0;
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
                _groundContacts.Add(collision.collider);
                break;
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (_groundContacts.Contains(collision.collider))
            _groundContacts.Remove(collision.collider);
    }

    private bool IsValidSurfaceAngle(Vector2 surfaceNormal) =>
         Vector2.Dot(surfaceNormal, Vector2.up) >= _maxGroundAngle;

}