using UnityEngine;

public abstract class BaseMover : MonoBehaviour
{
    private const float SpeedThreshold = 0.1f;

    [SerializeField] protected float _acceleration = 15f;
    [SerializeField] protected Flipper _flipper;

    protected Rigidbody2D _rigidbody;
    protected bool _isCrawling;
    protected float _currentSpeed;

    protected virtual void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _flipper = GetComponent<Flipper>();
    }

    public virtual void Move(float horizontalDirection, bool isCrawling)
    {
        _isCrawling = isCrawling;
        float targetSpeed = horizontalDirection * GetCurrentSpeed();
        ApplyMovement(targetSpeed);

        if (_flipper != null && horizontalDirection != 0)
            _flipper.Flip(horizontalDirection);
    }

    protected virtual void ApplyMovement(float targetSpeed)
    {
        float newSpeed = Mathf.Lerp(_rigidbody.velocity.x, targetSpeed, _acceleration * Time.fixedDeltaTime);
        _rigidbody.velocity = new Vector2(newSpeed, _rigidbody.velocity.y);
    }

    protected abstract float GetCurrentSpeed();

    public virtual void StopMovement()
    {
        if (_rigidbody != null)
            _rigidbody.velocity = new Vector2(0, _rigidbody.velocity.y);
    }

    public virtual bool IsMoving()
    {
        return _rigidbody != null && Mathf.Abs(_rigidbody.velocity.x) > SpeedThreshold;
    }
}