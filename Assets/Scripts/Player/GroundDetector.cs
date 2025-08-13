using UnityEngine;

public class GroundDetector : MonoBehaviour
{
    private const int GroundContactDecrement = 1;
    private const float angleDeviation = 0.3f;

    [SerializeField] private int _groundContactCount;
    [SerializeField] private bool _wasGrounded;

    public bool IsGrounded => _groundContactCount > 0;
    public bool JustLanded { get; private set; }
    public bool JustLeftGround { get; private set; }

    private void FixedUpdate()
    {
        JustLanded = _wasGrounded == false && IsGrounded;
        JustLeftGround = _wasGrounded && !IsGrounded;
        _wasGrounded = IsGrounded;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (IsGround(collision.collider) == false)
            return;

        if (HasValidContact(collision))
            _groundContactCount++;
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (IsGround(collision.collider) == false)
            return;

        bool hadValidContact = false;

        foreach (var contact in collision.contacts)
        {
            if (CheckAngleDeviation(contact))
            {
                hadValidContact = true;
                break;
            }
        }

        if (hadValidContact == false && _groundContactCount > 0)
            _groundContactCount--;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (IsGround(collision.collider) == false)
            return;

        _groundContactCount = Mathf.Max(0, _groundContactCount - GroundContactDecrement);
    }

    private bool IsGround(Collider2D collider) =>
         collider.TryGetComponent<Ground>(out _) != null;
        // collider.GetComponent<Ground>() != null;

    private bool HasValidContact(Collision2D collision)
    {
        foreach (var contact in collision.contacts)
            if (CheckAngleDeviation(contact))
                return true;

        return false;
    }

    private bool CheckAngleDeviation(ContactPoint2D contact) =>
        Vector2.Dot(contact.normal, Vector2.up) > angleDeviation;
}