using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(GroundDetector))]
public class Jumper : MonoBehaviour
{
    [SerializeField] private CharacterAnimator _animator;

    private PlayerSettings _playerSettings;
    private Rigidbody2D _rigidbody;
    private GroundDetector _groundDetector;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _groundDetector = GetComponent<GroundDetector>();

        if (_animator == null)
            Debug.LogError($"CharacterAnimator Component, not found for \"{GetType().Name}.cs\" on \"{gameObject.name}\" GameObject", this);
    }

    private void Update()
    {
        UpdateJumpAnimation();
    }

    public void Jump()
    {
        if (_groundDetector.IsGrounded)
            _rigidbody.AddForce(Vector2.up * _playerSettings.JumpForce, ForceMode2D.Impulse);
    }

    public void ApplyPlayerSettings(PlayerSettings settings) =>
        _playerSettings = settings;

    private void UpdateJumpAnimation()
    {
        if (_animator != null)
            _animator.UpdateJumpFallAnimation(_groundDetector.IsGrounded, _rigidbody.velocity.y);
    }
}