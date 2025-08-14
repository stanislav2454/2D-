using UnityEngine;

[RequireComponent(typeof(Animator), typeof(SpriteRenderer))]
public class CharacterAnimator : MonoBehaviour
{
    private const float MinimalDetectionSpeed = 0.2f;
    private const string IsWalking = nameof(IsWalking);
    private const string IsCrawling = nameof(IsCrawling);
    private const string IsJumping = nameof(IsJumping);
    private const string IsFalling = nameof(IsFalling);
    private const string HorizontalVelocity = nameof(HorizontalVelocity);
    private const string VerticalVelocity = nameof(VerticalVelocity);
    private const string IsGrounded = nameof(IsGrounded);

    [SerializeField] private Animator _animator;
    [SerializeField] private SpriteRenderer _spriteRenderer;

    private void Awake()
    {
        _animator = GetComponentInChildren<Animator>();
    }

    public void UpdateMovementAnimation(float horizontalSpeed, bool isCrawling)
    {
        float absSpeedX = Mathf.Abs(horizontalSpeed);
        _animator.SetFloat(HorizontalVelocity, absSpeedX);

        bool isMoving = absSpeedX > MinimalDetectionSpeed;
        _animator.SetBool(IsWalking, isMoving && !isCrawling);
        _animator.SetBool(IsCrawling, isCrawling);
    }

    public void UpdateVerticalAnimation(float verticalSpeed)
    {
        float absSpeedY = Mathf.Abs(verticalSpeed);
        _animator.SetFloat(VerticalVelocity, absSpeedY);
    }

    public void UpdateJumpFallAnimation(bool isGrounded, float verticalVelocity)
    {
        _animator.SetBool(IsGrounded, isGrounded);
        _animator.SetBool(IsJumping, isGrounded == false && verticalVelocity > 0);
        _animator.SetBool(IsFalling, isGrounded == false && verticalVelocity < 0);
    }

    public void FlipSprite(float horizontalDirection)
    {
        if (horizontalDirection > 0)
            _spriteRenderer.flipX = false;
        else if (horizontalDirection < 0)
            _spriteRenderer.flipX = true;
    }
}