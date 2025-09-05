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

    public void UpdateJumpFallAnimation(bool isGrounded, float verticalVelocity)
    {
        _animator.SetBool(CharacterAnimatorData.Params.IsGrounded, isGrounded);
        _animator.SetFloat(CharacterAnimatorData.Params.VerticalVelocity, verticalVelocity);

        if (isGrounded)
        {
            _animator.SetBool(CharacterAnimatorData.Params.IsJumping, false);
            _animator.SetBool(CharacterAnimatorData.Params.IsFalling, false);
        }
        else
        {
            if (verticalVelocity > 0)
            {
                _animator.SetBool(CharacterAnimatorData.Params.IsJumping, true);
                _animator.SetBool(CharacterAnimatorData.Params.IsFalling, false);
            }
            else
            {
                _animator.SetBool(CharacterAnimatorData.Params.IsJumping, false);
                _animator.SetBool(CharacterAnimatorData.Params.IsFalling, true);
            }
        }
    }

    public void PlayAttackAnimation()
    {
        _animator.SetTrigger(CharacterAnimatorData.Params.AttackTrigger);
    }

    public void StopAttackAnimation()
    {
        _animator.ResetTrigger(CharacterAnimatorData.Params.AttackTrigger);
        _animator.SetBool(CharacterAnimatorData.Params.IsAttacking, false);
    }
}