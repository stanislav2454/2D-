using UnityEngine;

[DisallowMultipleComponent, RequireComponent(typeof(Rigidbody2D), typeof(Flipper))]
public class CharacterMover : BaseMover
{
    [SerializeField] private PlayerSettings _settings;
    [SerializeField] private Transform _playerView;

    private CharacterAnimator _animator;

    private void Awake()
    {
        base.Awake();

        if (_playerView.TryGetComponent(out _animator) == false)
            Debug.LogError($"CharacterAnimator Component, not found for \"{GetType().Name}.cs\" on \"{gameObject.name}\" GameObject", this);
    }

    public void ApplyPlayerSettings(PlayerSettings settings) =>
        _settings = settings;

    public override void Move(float horizontalDirection, bool isCrawling)
    {
        base.Move(horizontalDirection, isCrawling);
        _animator?.UpdateMovementAnimation(horizontalDirection, _isCrawling);
    }

    protected override float GetCurrentSpeed() =>
        _isCrawling ? _settings.CrawlSpeed : _settings.WalkSpeed;
}