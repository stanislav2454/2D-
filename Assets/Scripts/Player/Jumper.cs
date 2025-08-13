using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Jumper : MonoBehaviour
{
    [SerializeField] private float _jumpPower = 5;
    [SerializeField] private float _doubleJumpMultiplier = 0.8f;

    private Rigidbody2D _rigidbody;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    public void Jump()
    {
       // Сбрасываем вертикальную скорость перед прыжком (если нужно)
       // _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, 0); 
        _rigidbody.AddForce(Vector2.up * _jumpPower, ForceMode2D.Impulse);
    }
}
