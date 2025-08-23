using TMPro;
using UnityEngine;

public class Collector : MonoBehaviour
{
    private const string UIText = "Coins:";

    [SerializeField] private TextMeshProUGUI _coinsText;

    private int _coinsAmount;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<ICollectable>(out var collectable))
            collectable.Accept(this);
    }

    public void CollectCoin(Coin coin)
    {
        _coinsAmount++;
        Destroy(coin.gameObject);

        _coinsText.text = $"{UIText} {_coinsAmount}";
    }

    public void CollectMedkit(Medkit medkit)
    {
        if (TryGetComponent<PlayerHealth>(out PlayerHealth playerHealth))
        {
            playerHealth.Heal(medkit.HealAmount);

            if (medkit.HealSound != null)
                AudioSource.PlayClipAtPoint(medkit.HealSound, transform.position);

            if (medkit.HealEffect != null)
                Instantiate(medkit.HealEffect, transform.position, Quaternion.identity);

            Destroy(medkit.gameObject);
        }
    }
}