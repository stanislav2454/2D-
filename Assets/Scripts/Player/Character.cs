using UnityEngine;

[RequireComponent(typeof(UserInputReader), typeof(CharacterMovement))]
[RequireComponent(typeof(Jumper), typeof(GroundDetector), typeof(Crawler))]
public class Character : MonoBehaviour
{
    private const int MaxJumps = 2;

    [SerializeField] private UserInputReader _input;
    [SerializeField] private CharacterMovement _movement;
    [SerializeField] private Jumper _jumper;
    [SerializeField] private GroundDetector _groundDetector;
    [SerializeField] private Crawler _crawler;

    private int _availableJumps;

    private void Awake()
    {
        _input = GetComponent<UserInputReader>();
        _movement = GetComponent<CharacterMovement>();
        _jumper = GetComponent<Jumper>();
        _groundDetector = GetComponent<GroundDetector>();
        _crawler = GetComponent<Crawler>();
    }

    private void Update()
    {
        ResetAvailableJumps();
        HandleCrawling();
    }

    private void FixedUpdate()
    {
        HandleMovement();
        HandleJump();
    }

    private void ResetAvailableJumps()
    {
        if (_groundDetector.IsGrounded && _groundDetector.JustLanded)
        {
            _availableJumps = MaxJumps;
        }
    }

    private void HandleMovement()
    {
        if (_input.HorizontalDirection != 0)
            _movement.Move(_input.HorizontalDirection, _input.IsCrawlPressed);
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
        _crawler.SetCrawling(_input.IsCrawlPressed);
}