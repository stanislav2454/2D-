using UnityEngine;

public class SearchState : EnemyState
{
    private float _searchTimer;
    private Vector2 _lastKnownPosition;
    private float _sqrSearchReachThreshold;

    public SearchState(EnemyStateMachine stateMachine, EnemyAI enemyAI)
        : base(stateMachine, enemyAI)
    {
        _sqrSearchReachThreshold = EnemySettings.SqrSearchReachThreshold;
    }

    public override void Enter()
    {
        _lastKnownPosition = EnemyAI.Player?.position ?? EnemyAI.transform.position;
        _searchTimer = EnemySettings.SearchDuration;

        Mover.SetChasingSpeed(true);
        Mover.StartChasing(_lastKnownPosition);
        Attacker.StopAttacking();
    }

    public override void Update()
    {
        _searchTimer -= Time.deltaTime;

        if (EnemyAI.Player != null)
        {
            StateMachine.ChangeState<ChaseState>();
            return;
        }

        if (_searchTimer <= 0 || HasReachedLastPosition())
        {
            StateMachine.ChangeState<PatrolState>();
        }
    }

    private bool HasReachedLastPosition()
    {
        float sqrDistance = ((Vector2)EnemyAI.transform.position - _lastKnownPosition).sqrMagnitude;
        return sqrDistance < _sqrSearchReachThreshold;
    }

    public override void Exit()
    {
        Mover.StopMovement();
    }
}