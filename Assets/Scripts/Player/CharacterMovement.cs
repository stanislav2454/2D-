using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class CharacterMovement : MonoBehaviour
{
    //[SerializeField] private float _flipThreshold = 0.1f;
    [SerializeField] private float _acceleration = 15f;
    [SerializeField] private MovementSettings _settings;
    //[SerializeField] private SpriteRenderer _spriteRenderer;

    private Rigidbody2D _rigidbody;

    private bool _isCrawling;
    //public bool _isFacingRight = true;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    public void Move(float horizontalDirection, bool isCrawling)
    {
        _isCrawling = isCrawling;

        float targetSpeed = horizontalDirection * GetCurrentSpeed();
        ApplyMovement(targetSpeed);
        //HandleSpriteFlip(horizontalDirection);
    }

    private void ApplyMovement(float targetSpeed)
    {
        float newSpeed = Mathf.Lerp(_rigidbody.velocity.x, targetSpeed, _acceleration * Time.fixedDeltaTime);

        _rigidbody.velocity = new Vector2(newSpeed, _rigidbody.velocity.y);
    }

    //private void HandleSpriteFlip(float horizontalInput)
    //{
    //    if (Mathf.Abs(horizontalInput) > _flipThreshold)
    //    {
    //        bool shouldFaceRight = horizontalInput > 0;

    //        if (shouldFaceRight != _isFacingRight)
    //            Flip(shouldFaceRight);
    //    }
    //}

    //private void Flip(bool faceRight)
    //{
    //    _isFacingRight = faceRight;

    //    if (_isFacingRight)
    //        _spriteRenderer.flipX = false;
    //    else if (_isFacingRight == false)
    //        _spriteRenderer.flipX = true;
    //}

    private float GetCurrentSpeed() =>
        _isCrawling ? _settings.CrawlSpeed : _settings.WalkSpeed;

    // Для внешнего доступа к текущей скорости
    public float GetCurrentVelocity() =>
        Mathf.Abs(_rigidbody.velocity.x);
}
