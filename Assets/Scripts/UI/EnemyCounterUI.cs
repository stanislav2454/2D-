using TMPro;
using UnityEngine;

public class EnemyCounterUI : MonoBehaviour
{
    private const string UIText = "Enemies:";

    [SerializeField] private TextMeshProUGUI _enemyCountText;
    private int _enemyCount;

    public void AddEnemy()
    {
        _enemyCount++;
        UpdateUI();
    }

    public void RemoveEnemy()
    {
        _enemyCount--;
        UpdateUI();
    }

    public void ResetCounter()
    {
        _enemyCount = 0;
        UpdateUI();
    }

    private void UpdateUI()
    {
        _enemyCountText.text = $"{UIText} {_enemyCount}";
    }
}