using UnityEngine;

public static class CharacterAnimatorData
{
    public static class Params
    {
        public static readonly int HorizontalVelocity = Animator.StringToHash(nameof(HorizontalVelocity));
        public static readonly int VerticalVelocity = Animator.StringToHash(nameof(VerticalVelocity));
        public static readonly int IsWalking = Animator.StringToHash(nameof(IsWalking));
        public static readonly int IsCrawling = Animator.StringToHash(nameof(IsCrawling));
        public static readonly int IsGrounded = Animator.StringToHash(nameof(IsGrounded));
        public static readonly int IsJumping = Animator.StringToHash(nameof(IsJumping));
        public static readonly int IsFalling = Animator.StringToHash(nameof(IsFalling));
        public static readonly int AttackTrigger = Animator.StringToHash(nameof(AttackTrigger));
        public static readonly int IsAttacking = Animator.StringToHash(nameof(IsAttacking));
    }
}