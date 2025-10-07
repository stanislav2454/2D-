using UnityEngine;

public class AttackState : EnemyState
{
    public AttackState(EnemyStateMachine stateMachine, EnemyAI enemyAI)
        : base(stateMachine, enemyAI) { }

    public override void Enter()
    {
        Mover.StopMovement();
        Mover.SetChasingSpeed(false);
        Attacker.StartAttacking();
    }

    public override void Update()
    {
        if (EnemyAI.Player == null)
        {
            StateMachine.ChangeState<PatrolState>();
            return;
        }

        float sqrDistance = ((Vector2)EnemyAI.transform.position - (Vector2)EnemyAI.Player.position).sqrMagnitude;

        if (sqrDistance > EnemySettings.SqrAttackRangeWithMargin)
        {
            StateMachine.ChangeState<ChaseState>();
            return;
        }
    }

    public override void Exit()
    {
        Attacker.StopAttacking();
    }
}