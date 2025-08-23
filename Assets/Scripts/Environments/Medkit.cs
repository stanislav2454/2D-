using UnityEngine;

public class Medkit : MonoBehaviour, ICollectable
{
    [Header("Healing Settings")]
    [SerializeField] private int _healAmount = 25;
    [SerializeField] private AudioClip _healSound;
    [SerializeField] private GameObject _healEffect;

    public int HealAmount => _healAmount;
    public AudioClip HealSound => _healSound;
    public GameObject HealEffect => _healEffect;

    public void Accept(Collector collector) =>
        collector.CollectMedkit(this);

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;

        if (TryGetComponent<CircleCollider2D>(out CircleCollider2D collider))
            Gizmos.DrawSphere(transform.position + (Vector3)collider.offset, collider.radius);
    }
}