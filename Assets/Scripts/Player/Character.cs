using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(UserInputReader), typeof(CharacterMover))]
[RequireComponent(typeof(Jumper), typeof(GroundDetector))]
[RequireComponent(typeof(Crawler), typeof(PlayerHealth))]

public class Character : MonoBehaviour
{
    private const int MaxJumps = 2;

    [Header("References")]
    [SerializeField] private UserInputReader _input;
    [SerializeField] private CharacterMover _movement;
    [SerializeField] private Jumper _jumper;
    [SerializeField] private GroundDetector _groundDetector;
    [SerializeField] private Crawler _crawler;
    [SerializeField] private AttackZone _attackZone;

    [Header("Attack Settings")]
    [SerializeField] private float _attackDamageInterval = 0.5f;
    [SerializeField] private int _attackDamage = 1;

    [Header("Lifesteal Settings")]
    [SerializeField] [Range(0f, 1f)] private float _healRatio = 0.5f;

    [Header("Jump Settings")]
    [SerializeField] private int _availableJumps;

    private Coroutine _attackCoroutine;
    private PlayerHealth _playerHealth;

    private void Awake()
    {
        _input = GetComponent<UserInputReader>();
        _movement = GetComponent<CharacterMover>();
        _jumper = GetComponent<Jumper>();
        _groundDetector = GetComponent<GroundDetector>();
        _crawler = GetComponent<Crawler>();
        _playerHealth = GetComponent<PlayerHealth>();

        _availableJumps = MaxJumps;

        if (_attackZone == null)
        {
            _attackZone = GetComponentInChildren<AttackZone>();

            if (_attackZone == null)
                Debug.LogError("AttackZone не найден! Добавьте дочерний объект с компонентом AttackZone.");
        }
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

    private void OnDestroy()
    {
        if (_attackCoroutine != null)
        {
            StopCoroutine(_attackCoroutine);
            _attackCoroutine = null;
        }

        _attackZone.ClearTargets();
    }

    private void HandleAttack()
    {
        if (_input.GetAttackTrigger())
        {
            if (_attackCoroutine == null)
                _attackCoroutine = StartCoroutine(AttackRoutine());
        }
        else if (_input.IsAttackPressed == false && _attackCoroutine != null)
        {
            if (_attackCoroutine != null)
            {
                StopCoroutine(_attackCoroutine);
                _attackCoroutine = null;
            }
        }
    }

    private IEnumerator AttackRoutine()
    {
        while (true)
        {
            _attackZone.CleanDestroyedTargets();

            yield return new WaitForSeconds(_attackDamageInterval);

            if (_attackZone != null && _attackZone.TargetsInZone.Count > 0)
                Attack();
        }
    }

    private void Attack()
    {
        if (_attackZone == null || _attackZone.TargetsInZone.Count == 0)
            return;

        int totalDamageDeal = CalculateDamageToTargets(_attackZone, _attackDamage);
        Lifesteal(totalDamageDeal);
    }

    private int CalculateDamageToTargets(AttackZone attackZone, int attackDamage)
    {
        int totalDamageDeal = 0;
        var targets = new List<IDamageable>(attackZone.TargetsInZone);

        foreach (var target in targets)
        {
            if (target != null)
            {
                int damageDeal = target.TakeDamage(attackDamage);
                totalDamageDeal += damageDeal;
            }
        }

        return totalDamageDeal;
    }

    private void Lifesteal(int totalDamageDeal)
    {
        if (totalDamageDeal > 0)
        {
            int healAmount = Mathf.RoundToInt(totalDamageDeal * _healRatio);
            _playerHealth.Heal(healAmount);
        }
    }

    private void UpdateJumps()
    {
        if (_groundDetector.IsGrounded)
        {
            if (_groundDetector.JustLanded)
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
        _crawler.SetCrawling(_input.IsCrawlPressed, _input.HorizontalDirection);
}