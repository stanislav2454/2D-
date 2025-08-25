using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(UserInputReader), typeof(CharacterMover), typeof(Jumper))]
[RequireComponent(typeof(Crawler), typeof(PlayerHealth), typeof(Attacker))]

public class Character : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private UserInputReader _input;
    [SerializeField] private CharacterMover _movement;
    [SerializeField] private Jumper _jumper;
    [SerializeField] private Crawler _crawler;
    [SerializeField] private Attacker _attacker;

    private void Awake()
    {
        _input = GetComponent<UserInputReader>();
        _movement = GetComponent<CharacterMover>();
        _jumper = GetComponent<Jumper>();
        _crawler = GetComponent<Crawler>();
        _attacker = GetComponent<Attacker>();
    }

    private void Update()
    {
        HandleJump();
        HandleCrawling();
        HandleAttack();
    }

    private void FixedUpdate()
    {
        HandleMovement();
    }

    private void HandleMovement()
    {
        if (_input.HorizontalDirection != 0)
            _movement.Move(_input.HorizontalDirection, _input.IsCrawlPressed);
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
        if (_input.GetIsJump())
            _jumper.Jump();
    }

    private void HandleCrawling() =>
        _crawler.SetCrawling(_input.IsCrawlPressed, _input.HorizontalDirection);
}