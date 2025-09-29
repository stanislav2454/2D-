using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(GroundDetector))]
public class Jumper : MonoBehaviour
{
    // [SerializeField] private float _jumpPower = 5;
    private PlayerSettings _playerSettings;

   // private float _jumpPower = 5;

    private Rigidbody2D _rigidbody;
    private GroundDetector _groundDetector;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _groundDetector = GetComponent<GroundDetector>();
    }

    public void Jump()
    {
        if (_groundDetector.IsGrounded)
           // _rigidbody.AddForce(Vector2.up * _jumpPower, ForceMode2D.Impulse);
            _rigidbody.AddForce(Vector2.up * _playerSettings.JumpForce, ForceMode2D.Impulse);
    }

    public void ApplyPlayerSettings(PlayerSettings settings) =>
        _playerSettings = settings;
}