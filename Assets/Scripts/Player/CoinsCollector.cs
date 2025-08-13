using UnityEngine;

public class CoinsCollector : MonoBehaviour
{
    [SerializeField] private int _coinsAmount = 0;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<Coin>(out _))
        {
            _coinsAmount++;
            Destroy(collision.gameObject);
        }
    }
}
