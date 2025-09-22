using System;

public interface IHealthProvider
{
    int CurrentHealth { get; }
    int MaxHealth { get; }
    //bool IsAlive { get; }
    bool IsDead { get; }

    event Action<int, int> HealthChanged;
   // event Action<int, int> MaxHealthChanged;
    event Action <BaseHealth>Died;
    event Action Revived;
}