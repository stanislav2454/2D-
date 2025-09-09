// ← ДОБАВЛЕНО: Запасное состояние для бездействия
public class IdleState : EnemyState
{
    public IdleState(EnemyAI ai, EnemyMover mover, EnemyAttacker attacker)
        : base(ai, mover, attacker) { }

    public override void Enter()
    {
        _mover.StopMovement();
       // _attacker.StopAttacking();
    }

    public override void Update()
    {
        // Ничего не делаем - просто стоим на месте
    }

    public override void Exit() { }
}
