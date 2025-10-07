using System;
using System.Collections.Generic;

public class EnemyStateMachine
{
    private Dictionary<Type, EnemyState> _states;
    private EnemyState _currentState;

    public event Action<Type> OnStateChanged;

    public EnemyStateMachine(EnemyAI enemyAI)
    {
        _states = new Dictionary<Type, EnemyState>
        {
            [typeof(PatrolState)] = new PatrolState(this, enemyAI),
            [typeof(ChaseState)] = new ChaseState(this, enemyAI),
            [typeof(AttackState)] = new AttackState(this, enemyAI),
            [typeof(SearchState)] = new SearchState(this, enemyAI),
        };
    }

    public void ChangeState<T>() where T : EnemyState
    {
        var newStateType = typeof(T);

        _currentState?.Exit();
        _currentState = _states[newStateType];
        _currentState.Enter();
        OnStateChanged?.Invoke(newStateType);
    }

    public void Update() =>
        _currentState?.Update();

    public void FixedUpdate() =>
        _currentState?.FixedUpdate();
}