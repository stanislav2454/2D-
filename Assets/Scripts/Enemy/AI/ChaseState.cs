using UnityEngine;

public class ChaseState : EnemyState
{
    private const float AttackCheckInterval = 0.2f;

    private float _lastAttackCheckTime;
    private float _sqrMaxChaseDistance;

    public ChaseState(EnemyStateMachine stateMachine, EnemyAI enemyAI)
        : base(stateMachine, enemyAI)
    {
        _sqrMaxChaseDistance = EnemySettings.SqrMaxChaseDistance;
    }

    public override void Enter()
    {
        Mover.SetChasingSpeed(true);
        Attacker.StopAttacking();
        _lastAttackCheckTime = 0f;
    }

    public override void Update()
    {
        if (EnemyAI.Player == null)
        {
            StateMachine.ChangeState<SearchState>();
            return;
        }

        Mover.StartChasing(EnemyAI.Player.position);

        _lastAttackCheckTime += Time.deltaTime;

        if (_lastAttackCheckTime >= AttackCheckInterval)
        {
            _lastAttackCheckTime = 0f;

            if (Attacker.CanAttack())
            {
                StateMachine.ChangeState<AttackState>();
                return;
            }
        }

        float sqrDistanceToPlayer = ((Vector2)EnemyAI.transform.position - (Vector2)EnemyAI.Player.position).sqrMagnitude;

        if (sqrDistanceToPlayer > _sqrMaxChaseDistance)
            StateMachine.ChangeState<SearchState>();
    }

    public override void Exit() { }
}