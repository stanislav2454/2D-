//using System.Collections;
//using System.Collections.Generic;
using UnityEngine;

public class AttackState : EnemyState // ← ИЗМЕНЕНО: Состояние атаки
{
    private Transform _target;
    //private const float AttackRange = 0.12f;
    private float _sqrAttackRange;

    public AttackState(EnemyAI ai, EnemyMover mover, EnemyAttacker attacker, Transform target)
        : base(ai, mover, attacker)
    {
        _target = target;
       // _sqrAttackRange = _ai.AttackRange * _ai.AttackRange;
    }

    public override void Enter()
    {
        _mover.SetChasingSpeed(false);
        _mover.StopMovement();
       // _attacker.StartAttacking();
    }

    public override void Update()
    {
        if (_target == null)
        {
           // _ai.TransitionToPatrolState();
            return;
        }

        // Проверяем, не ушел ли игрок из радиуса атаки
        float sqrDistance = Vector2.SqrMagnitude(_target.position - _ai.transform.position);

        if (sqrDistance > _sqrAttackRange)
        {
           // _ai.TransitionToChaseState();
        }

        // Поворачиваемся к цели (если нужно)
        Vector2 direction = (_target.position - _ai.transform.position).normalized;
        // Здесь можно добавить логику поворота врага
    }

    public override void Exit()
    {
       // _attacker.StopAttacking();
    }
}