using UnityEngine;

public class Collector : MonoBehaviour
{
    [SerializeField] private CoinsCounterUI _coinUI;
    [SerializeField] private PlayerHealth _playerHealth;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<ICollectable>(out var collectable))
            collectable.Accept(this);
    }

    public void CollectCoin(Coin coin)
    {
        _coinUI.AddCoin();
        Destroy(coin.gameObject);
    }

    public void CollectMedkit(Medkit medkit)
    {
        if (_playerHealth != null)
        {
            _playerHealth.Heal(medkit.HealAmount);
            Destroy(medkit.gameObject);
        }
    }
}