using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class CharacterMovement : MonoBehaviour
{
    [SerializeField] private float _acceleration = 15f;
    [SerializeField] private MovementSettings _settings;

    private Rigidbody2D _rigidbody;
    private bool _isCrawling;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    public void Move(float horizontalDirection, bool isCrawling)
    {
        _isCrawling = isCrawling;

        float targetSpeed = horizontalDirection * GetCurrentSpeed();
        ApplyMovement(targetSpeed);
    }

    private void ApplyMovement(float targetSpeed)
    {
        float newSpeed = Mathf.Lerp(_rigidbody.velocity.x, targetSpeed, _acceleration * Time.fixedDeltaTime);
        _rigidbody.velocity = new Vector2(newSpeed, _rigidbody.velocity.y);
    }

    private float GetCurrentSpeed() =>
        _isCrawling ? _settings.CrawlSpeed : _settings.WalkSpeed;
}
