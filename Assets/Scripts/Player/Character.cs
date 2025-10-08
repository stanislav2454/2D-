using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(UserInputReader), typeof(CharacterMover))]
[RequireComponent(typeof(Jumper), typeof(Crawler), typeof(PlayerHealth))]
[RequireComponent(typeof(PlayerAttacker), typeof(Rigidbody2D), typeof(GroundDetector))]
public class Character : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private UserInputReader _input;
    [SerializeField] private CharacterMover _movement;
    [SerializeField] private Jumper _jumper;
    [SerializeField] private Crawler _crawler;
    [SerializeField] private PlayerAttacker _attacker;
    [SerializeField] private VampirismAbility _vampirismAbility;
    [SerializeField] private PlayerSettings _playerSettings;

    private PlayerHealth _playerHealth;

    private void Awake()
    {
        _input = GetComponent<UserInputReader>();
        _movement = GetComponent<CharacterMover>();
        _jumper = GetComponent<Jumper>();
        _crawler = GetComponent<Crawler>();
        _attacker = GetComponent<PlayerAttacker>();
        _playerHealth = GetComponent<PlayerHealth>();

        if (_vampirismAbility == null)
            _vampirismAbility = GetComponentInChildren<VampirismAbility>();

        _playerHealth.Died += OnDead;

        ApplyPlayerSettings();
    }

    private void OnValidate()
    {
        if (_vampirismAbility == null)
            Debug.LogError("Vampirism Ability is not set!", this);
    }

    private void OnDestroy()
    {
        _playerHealth.Died -= OnDead;
    }

    private void Update()
    {
        HandleJump();
        HandleCrawling();
        HandleAttack();
        HandleVampirism();
    }

    private void FixedUpdate()
    {
        HandleMovement();
    }

    private void ApplyPlayerSettings()
    {
        if (_playerSettings != null)
        {
            _movement.ApplyPlayerSettings(_playerSettings);
            _jumper.ApplyPlayerSettings(_playerSettings);
            _attacker.ApplyPlayerSettings(_playerSettings);

            if (_playerSettings.VampirismDuration > 0)
            {
                _vampirismAbility.ApplySettings(
                    _playerSettings.VampirismDuration,
                    _playerSettings.VampirismCooldown,
                    _playerSettings.VampirismTickInterval,
                    _playerSettings.VampirismDamagePerTick,
                    _playerSettings.VampirismHealRatio);
            }
        }
    }

    private void HandleAttack()
    {
        if (_input.GetAttackTrigger())
            _attacker.StartAttacking();
        else if (_input.IsAttackPressed == false)
            _attacker.StopAttacking();
    }

    private void HandleVampirism()
    {
        if (_input.GetVampirismTrigger() && _vampirismAbility.IsAbilityReady)
            _vampirismAbility.StartAbility();
    }

    private void HandleMovement()
    {
        if (_input.HorizontalDirection != 0)
            _movement.MoveCharacter(_input.HorizontalDirection, _input.IsCrawlPressed);
        else
            _movement.StopMovement();
    }

    private void HandleJump()
    {
        if (_input.GetIsJump())
            _jumper.Jump();
    }

    private void HandleCrawling() =>
        _crawler.SetCrawling(_input.IsCrawlPressed, _input.HorizontalDirection);

    private void OnDead(BaseHealth health) =>
        gameObject.SetActive(false);
}