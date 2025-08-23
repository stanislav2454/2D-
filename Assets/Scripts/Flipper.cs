using UnityEngine;

[DisallowMultipleComponent]
public class Flipper : MonoBehaviour
{
    [SerializeField] private Transform _view;
    private SpriteRenderer _spriteRenderer;

    private void Awake()
    {
        if (_view != null)
            _spriteRenderer = _view.GetComponent<SpriteRenderer>();
    }

    private void OnValidate()
    {
        if (_view == null)
            Debug.LogWarning("View Transform is not set!", this);

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