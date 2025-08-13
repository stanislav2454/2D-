using UnityEngine;

[RequireComponent(typeof(Animator), typeof(SpriteRenderer))]
//[RequireComponent(typeof(Rigidbody2D), typeof(GroundDetector), typeof(Crawler))]
public class CharacterAnimator : MonoBehaviour
{
    private const float MinimalDetectionSpeed = 0.1f;

    [SerializeField] private bool _isGrounded;
    [SerializeField] private float _flipThreshold = 0.1f;

    [SerializeField] private Animator _animator;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private Rigidbody2D _rigidbody;
    [SerializeField] private GroundDetector _groundDetector;
    [SerializeField] private Crawler _crawler;

    private string _walkParam = "IsWalking";
    private string _crawlParam = "IsCrawling";
    private string _jumpParam = "IsJumping";
    private string _fallParam = "IsFalling";
    private string _speedX = "HorizontalVelocity";
    private string _speedY = "VerticalVelocity";
    private string _isGround = "IsGrounded";
    public bool _isFacingRight = true;

    private void Awake()
    {
        _animator = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        UpdateMovementAnimations();
        UpdateJumpFallAnimations();
        //HandleSpriteFlip();
        Flip();
    }

    private void UpdateMovementAnimations()
    {
        float speedX = Mathf.Abs(_rigidbody.velocity.x);
        _animator.SetFloat(_speedX, speedX);

        float speedY = Mathf.Abs(_rigidbody.velocity.y);
        _animator.SetFloat(_speedY, speedY);

        bool isMoving = speedX > MinimalDetectionSpeed;
        _animator.SetBool(_walkParam, isMoving && _crawler.IsCrawling == false);

        _animator.SetBool(_crawlParam, _crawler.IsCrawling);

    }

    private void Flip()
    {
        if (_rigidbody.velocity.x > 0)
            _spriteRenderer.flipX = false;
        else if (_rigidbody.velocity.x < 0)
            _spriteRenderer.flipX = true;
    }

    //private void HandleSpriteFlip(float horizontalInput)
    //{
    //    if (Mathf.Abs(horizontalInput) > _flipThreshold)
    //    {
    //        bool shouldFaceRight = horizontalInput > 0;

    //        // if (shouldFaceRight != _isFacingRight)
    //        //Flip(shouldFaceRight);
    //    }
    //}
    ////private void Flip(bool faceRight)
    ////{
    ////    _isFacingRight = faceRight;

    ////    if (_isFacingRight)
    ////        _spriteRenderer.flipX = false;
    ////    else if (_isFacingRight == false)
    ////        _spriteRenderer.flipX = true;
    ////}

    private void UpdateJumpFallAnimations()
    {
        bool isGrounded = _groundDetector.IsGrounded;
        _animator.SetBool(_isGround, isGrounded);

        bool isJumping = isGrounded == false && _rigidbody.velocity.y > 0;
        bool isFalling = isGrounded == false && _rigidbody.velocity.y < 0;

        _animator.SetBool(_jumpParam, isJumping);
        _animator.SetBool(_fallParam, isFalling);
    }

    // Метод для принудительного сброса анимаций (например, при смерти)
    public void ResetAllAnimations()
    {
        _animator.SetBool(_walkParam, false);
        _animator.SetBool(_crawlParam, false);
        _animator.SetBool(_jumpParam, false);
        _animator.SetBool(_fallParam, false);
        _animator.SetFloat(_speedX, 0);
        _animator.SetFloat(_speedY, 0);
    }
}