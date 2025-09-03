using UnityEngine;

[DisallowMultipleComponent, RequireComponent(typeof(CapsuleCollider2D))]
public class Crawler : MonoBehaviour
{
    private const float PositionYoffset = 0.3f;
    private const float ScaleYoffset = 0.25f;

    [SerializeField] private float _crawlColliderHeight = 0.35f;
    [SerializeField] private Vector3 _crawlTransformScale = new Vector3(1.5f, 0.5f, 1f);
    [SerializeField] private Transform _playerView;

    private CharacterAnimator _animator;
    private CapsuleCollider2D _collider;
    private Vector3 _normalScale;
    private Vector2 _originalColliderSize;
    private Vector2 _originalColliderOffset;
    private bool _isCrawling;

    private void Awake()
    {
        if (_playerView.TryGetComponent(out _animator) == false) { }

        _collider = GetComponent<CapsuleCollider2D>();
        CacheOriginalValues();
    }

    public void SetCrawling(bool crawl, float speedX)
    {
        _isCrawling = crawl;

        if (_isCrawling)
            ApplyCrawlState(speedX);
        else
            ResetCrawlState();
    }

    private void ApplyCrawlState(float speedX)
    {
        _playerView.localScale = _crawlTransformScale;
        _playerView.position = new Vector3(transform.position.x, transform.position.y - PositionYoffset, transform.position.z);

        _collider.size = new Vector2(_originalColliderSize.x, _isCrawling ? _crawlColliderHeight : _originalColliderSize.y);
        _collider.offset = new Vector2(_originalColliderOffset.x, _originalColliderOffset.y - _originalColliderSize.y * ScaleYoffset);

        _animator?.UpdateMovementAnimation(speedX, _isCrawling);
    }

    private void ResetCrawlState()
    {
        _playerView.position = new Vector3(transform.position.x, gameObject.transform.position.y, transform.position.z);
        _playerView.localScale = _normalScale;

        _collider.size = _originalColliderSize;
        _collider.offset = _originalColliderOffset;
    }

    private void CacheOriginalValues()
    {
        _normalScale = transform.localScale;
        _originalColliderSize = _collider.size;
        _originalColliderOffset = _collider.offset;
    }
}