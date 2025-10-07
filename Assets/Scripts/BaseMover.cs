using UnityEngine;

public abstract class BaseMover : MonoBehaviour
{
    [SerializeField] protected float Acceleration = 15f;
    [SerializeField] protected Flipper Flipper;

    protected Rigidbody2D Rigidbody;
    protected float CurrentSpeed;

    protected virtual void Awake()
    {
        Rigidbody = GetComponent<Rigidbody2D>();
        Flipper = GetComponent<Flipper>();
    }

    protected abstract float GetCurrentSpeed();

    protected virtual void ApplyMovement(float targetSpeed)
    {
        float newSpeed = Mathf.Lerp(Rigidbody.velocity.x, targetSpeed, Acceleration * Time.fixedDeltaTime);
        Rigidbody.velocity = new Vector2(newSpeed, Rigidbody.velocity.y);
    }

    public virtual void Move(float horizontalDirection)
    {
        float targetSpeedX = horizontalDirection * GetCurrentSpeed();
        ApplyMovement(targetSpeedX);

        if (horizontalDirection != 0)
            Flipper?.Flip(horizontalDirection);
    }

    public virtual void StopMovement()
    {
        if (Rigidbody != null)
            Rigidbody.velocity = new Vector2(0, Rigidbody.velocity.y);
    }
}