using UnityEngine;

[DisallowMultipleComponent, RequireComponent(typeof(Rigidbody2D), typeof(Flipper))]
public class CharacterMover : BaseMover
{
    [SerializeField] private PlayerSettings _settings;
    [SerializeField] private Transform _playerView;

    private CharacterAnimator _animator;
    private bool _isCrawling;

    private new void Awake()
    {
        base.Awake();

        if (_playerView.TryGetComponent(out _animator) == false)
            Debug.LogError($"CharacterAnimator Component, not found for \"{GetType().Name}.cs\" on \"{gameObject.name}\" GameObject", this);
    }

    public void ApplyPlayerSettings(PlayerSettings settings) =>
        _settings = settings;

    public void MoveCharacter(float horizontalDirection, bool isCrawling)
    {
        _isCrawling = isCrawling;

        Move(horizontalDirection);

        _animator?.UpdateMovementAnimation(horizontalDirection, isCrawling);
    }

    protected override float GetCurrentSpeed() =>
        _isCrawling ? _settings.CrawlSpeed : _settings.WalkSpeed;
}