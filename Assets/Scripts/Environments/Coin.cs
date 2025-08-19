using UnityEngine;

public class Coin : MonoBehaviour, ICollectable
{
    public void Accept(Collector collector)
    {
        collector.Collect(this);
    }
}