using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(UserInputReader), typeof(CharacterMover))]
[RequireComponent(typeof(Jumper), typeof(Crawler), typeof(PlayerHealth))]
[RequireComponent(typeof(Attacker), typeof(Rigidbody2D), typeof(GroundDetector))]
public class Character : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private UserInputReader _input;
    [SerializeField] private CharacterMover _movement;
    [SerializeField] private Jumper _jumper;
    [SerializeField] private Crawler _crawler;
    [SerializeField] private Attacker _attacker;
    [SerializeField] private CharacterAnimator _animator;

    private PlayerHealth _playerHealth;
    private Rigidbody2D _rigidbody;
    private GroundDetector _groundDetector;

    private void Awake()
    {
        _input = GetComponent<UserInputReader>();
        _movement = GetComponent<CharacterMover>();
        _jumper = GetComponent<Jumper>();
        _crawler = GetComponent<Crawler>();
        _attacker = GetComponent<Attacker>();
        _playerHealth = GetComponent<PlayerHealth>();
        _rigidbody = GetComponent<Rigidbody2D>();
        _groundDetector = GetComponent<GroundDetector>();

        if (_animator == null)
            _animator = GetComponentInChildren<CharacterAnimator>();

        _playerHealth.Died += OnDead; 
        _attacker.Attacked += OnAttackPerformed;
    }

    private void Update()
    {
        HandleJump();
        HandleCrawling();
        HandleAttack();
        UpdateAnimations();
    }

    private void FixedUpdate()
    {
        HandleMovement();
    }

    private void OnDestroy()
    {
        _playerHealth.Died -= OnDead;
        _attacker.Attacked -= OnAttackPerformed;
    }

    private void HandleAttack()
    {
        if (_input.GetAttackTrigger())
            _attacker.StartAttacking();
        else if (_input.IsAttackPressed == false)
            _attacker.StopAttacking();
    }

    private void OnAttackPerformed()
    {
        _animator.PlayAttackAnimation();
    }

    private void OnDead() =>
        gameObject.SetActive(false);

    private void HandleMovement()
    {
        if (_input.HorizontalDirection != 0)
            _movement.Move(_input.HorizontalDirection, _input.IsCrawlPressed);
    }

    private void HandleJump()
    {
        if (_input.GetIsJump())
            _jumper.Jump();
    }

    private void HandleCrawling() =>
        _crawler.SetCrawling(_input.IsCrawlPressed, _input.HorizontalDirection);

    private void UpdateAnimations()
    {
        _animator.UpdateMovementAnimation(_input.HorizontalDirection, _input.IsCrawlPressed);
        _animator.UpdateJumpFallAnimation(_groundDetector.IsGrounded, _rigidbody.velocity.y);
    }
}