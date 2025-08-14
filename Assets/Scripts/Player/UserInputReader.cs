using UnityEngine;

[DisallowMultipleComponent]
public class UserInputReader : MonoBehaviour
{
    public const string Horizontal = nameof(Horizontal);
    private const KeyCode JumpKey = KeyCode.Space;
    private const KeyCode CrawlKey = KeyCode.LeftControl;

    public float HorizontalDirection { get; private set; }
    public bool IsCrawlPressed { get; private set; }

    private bool _isJump;

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
        if (Input.GetKeyDown(JumpKey))
            _isJump = true;
    }

    private bool GetBoolAsTrigger(ref bool value)
    {
        bool localValue = value;
        value = false;

        return localValue;
    }

    private void UpdateCrawl() =>
        IsCrawlPressed = Input.GetKey(CrawlKey);
}