using UnityEngine;

public class Collector : MonoBehaviour
{
    [SerializeField] private CoinsCounterUI _coinUI;

    private int _coinsAmount;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<ICollectable>(out var collectable))
            collectable.Accept(this);
    }

    public void CollectCoin(Coin coin)
    {
        const int NumberCoinsCollect = 1;
        _coinsAmount += NumberCoinsCollect;
        _coinUI.AddCoin(NumberCoinsCollect);

        Destroy(coin.gameObject);
    }

    public void CollectMedkit(Medkit medkit)
    {
        if (TryGetComponent<PlayerHealth>(out var playerHealth))
        {
            playerHealth.Heal(medkit.HealAmount);

            Destroy(medkit.gameObject);
        }
    }
}