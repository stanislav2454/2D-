using UnityEngine;

[DisallowMultipleComponent, RequireComponent(typeof(Animator))]
public class CharacterAnimator : MonoBehaviour
{
    private const float MinimalDetectionSpeed = 0.2f;

    [SerializeField] private Animator _animator;

    private void Awake()
    {
        _animator = GetComponentInChildren<Animator>();
    }

    public void UpdateMovementAnimation(float horizontalSpeed, bool isCrawling)
    {
        float absSpeedX = Mathf.Abs(horizontalSpeed);
        _animator.SetFloat(CharacterAnimatorData.Params.HorizontalVelocity, absSpeedX);

        bool isMoving = absSpeedX > MinimalDetectionSpeed;
        _animator.SetBool(CharacterAnimatorData.Params.IsWalking, isMoving && !isCrawling);
        _animator.SetBool(CharacterAnimatorData.Params.IsCrawling, isCrawling);
    }

    public void UpdateVerticalAnimation(float verticalSpeed)
    {
        float absSpeedY = Mathf.Abs(verticalSpeed);
        _animator.SetFloat(CharacterAnimatorData.Params.VerticalVelocity, absSpeedY);
    }

    public void UpdateJumpFallAnimation(bool isGrounded, float verticalVelocity)
    {
        _animator.SetBool(CharacterAnimatorData.Params.IsGrounded, isGrounded);
        _animator.SetBool(CharacterAnimatorData.Params.IsJumping, isGrounded == false && verticalVelocity > 0);
        _animator.SetBool(CharacterAnimatorData.Params.IsFalling, isGrounded == false && verticalVelocity < 0);
    }

    public static class CharacterAnimatorData
    {
        public static class Params
        {
            public static readonly int IsWalking = Animator.StringToHash(nameof(IsWalking));
            public static readonly int IsCrawling = Animator.StringToHash(nameof(IsCrawling));
            public static readonly int IsJumping = Animator.StringToHash(nameof(IsJumping));
            public static readonly int IsFalling = Animator.StringToHash(nameof(IsFalling));
            public static readonly int HorizontalVelocity = Animator.StringToHash(nameof(HorizontalVelocity));
            public static readonly int VerticalVelocity = Animator.StringToHash(nameof(VerticalVelocity));
            public static readonly int IsGrounded = Animator.StringToHash(nameof(IsGrounded));
        }
    }
}