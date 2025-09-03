using UnityEngine;

[DisallowMultipleComponent]
public class Flipper : MonoBehaviour
{
    [SerializeField] private Transform _view;

    private Quaternion _originalRotation;
    private Vector3 _turnAroundAngle = new Vector3(0f, 180f, 0f);

    private void Awake()
    {
        if (_view != null)
            _originalRotation = _view.localRotation;
    }

    private void OnValidate()
    {
        if (_view == null)
            Debug.LogWarning("View Transform is not set!", this);
    }

    public void Flip(float horizontalDirection)
    {
        if (_view == null)
            return;

        if (horizontalDirection > 0)
            _view.localRotation = _originalRotation;
        else if (horizontalDirection < 0)
            _view.localRotation = _originalRotation * Quaternion.Euler(_turnAroundAngle);
    }
}