using UnityEngine;

public class PatrolState : EnemyState
{
    private const float WaypointReachThreshold = 0.5f;
    private float _sqrWaypointReachThreshold;
    private int _currentWaypointIndex = 0;

    public PatrolState(EnemyStateMachine stateMachine, EnemyAI enemyAI)
        : base(stateMachine, enemyAI)
    {
        _sqrWaypointReachThreshold = WaypointReachThreshold * WaypointReachThreshold;
    }

    public override void Enter()
    {
        Mover.SetChasingSpeed(false);
        Mover.StartPatrol(EnemyAI.PatrolPath);
        Attacker.StopAttacking();
    }

    public override void Update()
    {
        PatrolBehavior();

        if (EnemyAI.Player != null)
            StateMachine.ChangeState<ChaseState>();
    }

    public override void Exit()
    {
        Mover.StopMovement();
    }

    private void PatrolBehavior()
    {
        if (EnemyAI.PatrolPath == null || EnemyAI.PatrolPath.Count == 0)
            return;

        Transform waypoint = EnemyAI.PatrolPath.GetWaypoint(_currentWaypointIndex);
        Mover.MoveToTarget(waypoint.position);

        if (Vector2.SqrMagnitude(EnemyAI.transform.position - waypoint.position) < _sqrWaypointReachThreshold)
            _currentWaypointIndex = (_currentWaypointIndex + 1) % EnemyAI.PatrolPath.Count;
    }
}