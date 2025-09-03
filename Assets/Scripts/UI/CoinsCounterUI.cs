public class CoinsCounterUI : BaseCounterUI
{
    private const int Item = 1;

    protected override string TextPrefix => "Coins:";

    public void AddCoin() =>
        AddValue(Item);

    public void AddCoin(int item) =>
        AddValue(item);

    public void ResetCoins() =>
        ResetValue();
}