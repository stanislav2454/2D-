using UnityEngine;

[RequireComponent(typeof(CapsuleCollider2D))]
public class Crawler : MonoBehaviour
{
    private const float PositionYoffset = 0.3f;
    private const float ScaleYoffset = 0.25f;

    [SerializeField] private Vector3 _crawlTransformScale = new Vector3(1f, 0.5f, 1f);
    [SerializeField] private Transform _playerView;

    private CharacterAnimator _animator;
    private CapsuleCollider2D _collider;
    private Vector3 _normalScale;
    private Vector2 _originalColliderSize;
    private Vector2 _originalColliderOffset;
    private float _crawlColliderHeight = 0.35f;
    private bool _isCrawling;

    public bool IsCrawling => _isCrawling;

    private void Awake()
    {
        if (_playerView != null)
            _animator = _playerView.GetComponent<CharacterAnimator>();
        _normalScale = transform.localScale;

        _collider = GetComponent<CapsuleCollider2D>();
        _originalColliderSize = _collider.size;
        _originalColliderOffset = _collider.offset;
    }

    public void SetCrawling(bool crawl)
    {
        _isCrawling = crawl;

        if (_isCrawling)
        {
            _playerView.localScale = _crawlTransformScale;
            _playerView.position = new Vector3(transform.position.x, transform.position.y - PositionYoffset, transform.position.z);

            _collider.size = new Vector2(_originalColliderSize.x, _isCrawling ? _crawlColliderHeight : _originalColliderSize.y);
            _collider.offset = new Vector2(_originalColliderOffset.x, _originalColliderOffset.y - _originalColliderSize.y * ScaleYoffset);

            _animator.UpdateMovementAnimation(0, _isCrawling);
        }
        else
        {
            _playerView.position = new Vector3(transform.position.x, gameObject.transform.position.y, transform.position.z);
            _playerView.localScale = _normalScale;

            _collider.size = _originalColliderSize;
            _collider.offset = _originalColliderOffset;
        }
    }
}