using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(UserInputReader), typeof(CharacterMover))]
[RequireComponent(typeof(Jumper), typeof(GroundDetector))]
[RequireComponent(typeof(Crawler), typeof(PlayerHealth), typeof(Attacker))]

public class Character : MonoBehaviour
{
    private const int MaxJumps = 2;

    [Header("References")]
    [SerializeField] private UserInputReader _input;
    [SerializeField] private CharacterMover _movement;
    [SerializeField] private Jumper _jumper;
    [SerializeField] private GroundDetector _groundDetector;
    [SerializeField] private Crawler _crawler;
    [SerializeField] private Attacker _attacker;

    [Header("Jump Settings")]
    [SerializeField] private int _availableJumps;

    private void Awake()
    {
        _input = GetComponent<UserInputReader>();
        _movement = GetComponent<CharacterMover>();
        _jumper = GetComponent<Jumper>();
        _groundDetector = GetComponent<GroundDetector>();
        _crawler = GetComponent<Crawler>();
        _attacker = GetComponent<Attacker>();

        _availableJumps = MaxJumps;
    }

    private void Update()
    {
        HandleJump();
        HandleCrawling();
        HandleAttack();
    }

    private void FixedUpdate()
    {
        UpdateJumps();
        HandleMovement();
    }

    private void HandleMovement()
    {
        if (_input.HorizontalDirection != 0)
            _movement.Move(_input.HorizontalDirection, _input.IsCrawlPressed);
    }

    private void UpdateJumps()
    {
        if (_groundDetector.IsGrounded && _groundDetector.JustLanded)
            _availableJumps = MaxJumps;
    }

    private void HandleAttack()
    {
        if (_input.GetAttackTrigger())
            _attacker?.StartAttacking();
        else if (_input.IsAttackPressed == false)
            _attacker?.StopAttacking();
    }

    private void HandleJump()
    {
        if (_input.GetIsJump() && _availableJumps > 0)
        {
            _jumper.Jump();
            _availableJumps--;
        }
    }

    private void HandleCrawling() =>
        _crawler.SetCrawling(_input.IsCrawlPressed, _input.HorizontalDirection);
}