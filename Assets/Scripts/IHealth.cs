using System;

public interface IHealth
{
    int Current { get; }
    int Max { get; }
    bool IsDead { get; }
    public float Normalized { get; }

    event Action<int, int> Changed;
    event Action <BaseHealth>Died;
    event Action Revived;
}