using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(GroundDetector))]
public class Jumper : MonoBehaviour
{
    [SerializeField] private float _jumpPower = 5;
    [SerializeField] private Transform _playerView;

    private Rigidbody2D _rigidbody;
    private CharacterAnimator _animator;
    private GroundDetector _groundDetector;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _groundDetector = GetComponent<GroundDetector>();

        if (_playerView != null)
            _animator = _playerView.GetComponent<CharacterAnimator>();
    }

    public void Jump()
    {
        if (_groundDetector.IsGrounded)
        {
            _rigidbody.AddForce(Vector2.up * _jumpPower, ForceMode2D.Impulse);
            _animator?.UpdateJumpFallAnimation(false, _rigidbody.velocity.y);
        }
    }

    private void Update()
    {
        _animator?.UpdateJumpFallAnimation(_groundDetector.IsGrounded, _rigidbody.velocity.y);
        _animator?.UpdateVerticalAnimation(_rigidbody.velocity.y);
    }
}
