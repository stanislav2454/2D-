using UnityEngine;

[DisallowMultipleComponent]
public class UserInputReader : MonoBehaviour
{
    private const string Horizontal = nameof(Horizontal);
    private const KeyCode JumpKey = KeyCode.Space;
    private const KeyCode CrawlKey = KeyCode.LeftControl;
    private const KeyCode AttackKey = KeyCode.LeftAlt;

    public float HorizontalDirection { get; private set; }
    public bool IsCrawlPressed { get; private set; }
    public bool IsAttackPressed { get; private set; }

    private bool _isJump;
    private bool _attackTrigger;

    private void Update()
    {
        UpdateMovement();
        UpdateJump();
        UpdateCrawl();
        UpdateAttack();
    }

    public bool GetIsJump() =>
        GetBoolAsTrigger(ref _isJump);

    public bool GetAttackTrigger() =>
    GetBoolAsTrigger(ref _attackTrigger);

    private void UpdateMovement() =>
        HorizontalDirection = Input.GetAxis(Horizontal);

    private void UpdateJump()
    {
        if (Input.GetKeyDown(JumpKey))
            _isJump = true;
    }

    private void UpdateAttack()
    {
        IsAttackPressed = Input.GetKey(AttackKey);

        if (Input.GetKeyDown(AttackKey))
            _attackTrigger = true;
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