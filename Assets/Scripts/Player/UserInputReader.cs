using UnityEngine;

[DisallowMultipleComponent]
public class UserInputReader : MonoBehaviour
{
    private const string Horizontal = nameof(Horizontal);
    private const KeyCode JumpKey = KeyCode.Space;
    private const KeyCode CrawlKey = KeyCode.LeftControl;
    private const KeyCode AttackKey = KeyCode.LeftAlt;
    private const KeyCode VampirismKey = KeyCode.V;

    public float HorizontalDirection { get; private set; }
    public bool IsCrawlPressed { get; private set; }
    public bool IsAttackPressed { get; private set; }
    public bool IsVampirismPressed { get; private set; }

    private bool _isJump = false;
    private bool _attackTrigger;
    private bool _vampirismTrigger;

    private void Update()
    {
        UpdateMovement();
        UpdateJump();
        UpdateCrawl();
        UpdateAttack();
        UpdateVampirism();
    }

    public bool GetIsJump() =>
        GetBoolAsTrigger(ref _isJump);

    public bool GetAttackTrigger() =>
    GetBoolAsTrigger(ref _attackTrigger);

    public bool GetVampirismTrigger() =>
        GetBoolAsTrigger(ref _vampirismTrigger);

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

    private void UpdateVampirism()
    {
        IsVampirismPressed = Input.GetKey(VampirismKey);

        if (Input.GetKeyDown(VampirismKey))
            _vampirismTrigger = true;
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