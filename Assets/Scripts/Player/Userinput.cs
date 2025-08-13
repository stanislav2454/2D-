using UnityEngine;

public class Userinput : MonoBehaviour
{
    public const string Horizontal = nameof(Horizontal);

    public float HorizontalDirection { get; private set; }
    public bool IsCrawlPressed { get; private set; }

    [SerializeField] private bool _isJump;

    private void Update()
    {
        UpdateMovement();
        UpdateJump();
        UpdateCrawl();
    }

    public bool GetIsJump() =>
        GetBoolAsTrigger(ref _isJump);

    private void UpdateMovement() =>
        HorizontalDirection = Input.GetAxis(Horizontal);

    private void UpdateJump()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            _isJump = true;
    }

    private bool GetBoolAsTrigger(ref bool value)
    {
        bool localValue = value;
        value = false;

        return localValue;
    }

    private void UpdateCrawl() =>
        IsCrawlPressed = Input.GetKey(KeyCode.LeftControl);
}