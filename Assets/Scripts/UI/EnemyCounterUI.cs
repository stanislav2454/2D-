public class EnemyCounterUI : BaseCounterUI
{
    private const int Item = 1;

    protected override string TextPrefix => "Enemies:";

    public void AddEnemy() =>
        AddValue(Item);

    public void AddEnemy(int item) =>
        AddValue(item);

    public void RemoveEnemy() =>
        SubtractValue(Item);

    public void RemoveEnemy(int item) =>
        SubtractValue(item);

    public void ResetCounter() =>
        ResetValue();
}