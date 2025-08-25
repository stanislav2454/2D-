using TMPro;
using UnityEngine;

public class CoinsCounterUI : MonoBehaviour
{
    private const string UIText = "Coins:";

    [SerializeField] private TextMeshProUGUI _coinsText;
    private int _coinsAmount;

    public void AddCoin()
    {
        _coinsAmount++;
        UpdateUI();
    }

    public void ResetCoins()
    {
        _coinsAmount = 0;
        UpdateUI();
    }

    private void UpdateUI()
    {
        _coinsText.text = $"{UIText} {_coinsAmount}";
    }
}