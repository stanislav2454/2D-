using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(Rigidbody2D), typeof(BoxCollider2D))]
public class PhysicsMaterialConfigurator : MonoBehaviour
{
    [Tooltip("Настройка трения для разных поверхностей (1 = нормальное)")]
    [Range(0.1f, 2f)]
    [SerializeField] private float _friction = 1f;

    [SerializeField] private Rigidbody2D _rigidbody;
    [SerializeField] private BoxCollider2D _collider;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _collider = GetComponent<BoxCollider2D>();
    }

    private void Start()
    {
        ConfigureMaterial(_rigidbody);
        ConfigureMaterial(_collider);
    }

    private void ConfigureMaterial(Collider2D collider)
    {
        var material = collider.sharedMaterial ?? new PhysicsMaterial2D();
        material.friction = _friction;
        collider.sharedMaterial = material;
    }

    private void ConfigureMaterial(Rigidbody2D rigidbody)
    {
        var material = rigidbody.sharedMaterial ?? new PhysicsMaterial2D();
        material.friction = _friction;
        rigidbody.sharedMaterial = material;
    }
}