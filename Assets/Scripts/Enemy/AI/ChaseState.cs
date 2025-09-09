//using System.Collections;
//using System.Collections.Generic;
using UnityEngine;

public class ChaseState : EnemyState // ← ИЗМЕНЕНО: Состояние преследования
{
    private Transform _target;
    private float _sqrAttackRange;

    public ChaseState(EnemyAI ai, EnemyMover mover, EnemyAttacker attacker, Transform target)
        : base(ai, mover, attacker)
    {
        _target = target;
      //  _sqrAttackRange = _ai.AttackRange * _ai.AttackRange;
    }

    public override void Enter()
    {
        _mover.SetChasingSpeed(true);
       // _attacker.StopAttacking();
    }

    public override void Update()
    {
        if (_target == null)
        {
           // _ai.TransitionToPatrolState();
            return;
        }

        // Двигаемся к цели
        _mover.MoveToTarget(_target.position);

        // Проверяем дистанцию для атаки
        float sqrDistance = Vector2.SqrMagnitude(_target.position - _ai.transform.position);

        //if (sqrDistance <= _sqrAttackRange)
        //{
        //    _ai.TransitionToAttackState();
        //}

        //// Проверяем, не потеряли ли цель (дополнительная проверка)
        //if (sqrDistance > _ai.AttackRange * 4f) // Если слишком далеко
        //{
        //    _ai.TransitionToPatrolState();
        //}
    }

    public override void Exit()
    {
        _mover.SetChasingSpeed(false);
        _mover.StopMovement();
    }
}