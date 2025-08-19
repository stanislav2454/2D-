using UnityEngine;

[DisallowMultipleComponent]
public class Flipper : MonoBehaviour
{
    [SerializeField] private Transform _playerView;
    private SpriteRenderer _spriteRenderer;

    private void Awake()
    {
        if (_playerView != null)
            _spriteRenderer = _playerView.GetComponent<SpriteRenderer>();
    }

    private void OnValidate()
    {
        if (_playerView == null)
            Debug.LogWarning("playerView Transform is not set!", this);
        if (_spriteRenderer == null)
            Debug.LogWarning("SpriteRenderer is not set!", this);
    }

    public void Flip(float horizontalDirection)
    {
        if (horizontalDirection > 0)
            _spriteRenderer.flipX = false;
        else if (horizontalDirection < 0)
            _spriteRenderer.flipX = true;
    }
}