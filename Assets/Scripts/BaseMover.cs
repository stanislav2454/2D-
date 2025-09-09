using UnityEngine;

public abstract class BaseMover : MonoBehaviour
{
    [SerializeField] protected float _acceleration = 15f;

    protected Rigidbody2D _rigidbody;
    protected bool _isCrawling;

    protected virtual void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    public virtual void Move(float horizontalDirection, bool isCrawling)
    {
        _isCrawling = isCrawling;
        float targetSpeed = horizontalDirection * GetCurrentSpeed();
        ApplyMovement(targetSpeed);
    }

    protected virtual void ApplyMovement(float targetSpeed)
    {
        float newSpeed = Mathf.Lerp(_rigidbody.velocity.x, targetSpeed,
                                  _acceleration * Time.fixedDeltaTime);
        _rigidbody.velocity = new Vector2(newSpeed, _rigidbody.velocity.y);
    }

    protected virtual float GetCurrentSpeed()
    {
        Debug.LogWarning("GetCurrentSpeed() not implemented in derived class!");
        return 0f;
    }

    public virtual void StopMovement()
    {
        if (_rigidbody != null)
            _rigidbody.velocity = new Vector2(0, _rigidbody.velocity.y);
    }

    public virtual void MoveToTarget(Vector2 targetPosition)
    {
        Vector2 direction = (targetPosition - (Vector2)transform.position).normalized;
        Move(direction.x, false);
    }

    public virtual bool IsMoving()
    {
        if (_rigidbody != null)
            return Mathf.Abs(_rigidbody.velocity.x) > 0.1f;
        return false;
    }
}