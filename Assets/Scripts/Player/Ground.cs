using UnityEngine;

[DisallowMultipleComponent]
public class Ground : MonoBehaviour
{
    [Tooltip("Настройка трения для разных поверхностей (1 = нормальное)")]
    [Range(0.1f, 2f)]
    [SerializeField] private float _friction = 1f;

    public float Friction => _friction;
}
