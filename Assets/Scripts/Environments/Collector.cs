using TMPro;
using UnityEngine;

public class Collector : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI coinsText;

    private int _coinsAmount;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<ICollectable>(out var collectable))
        {
            collectable.Accept(this);
        }
    }

    public void Collect(Coin coin)
    {
        _coinsAmount++;
        Destroy(coin.gameObject);

        coinsText.text = $"Coins: {_coinsAmount}";
    }
}
