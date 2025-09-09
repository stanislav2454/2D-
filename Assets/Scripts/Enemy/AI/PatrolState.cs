//using System.Collections;
//using System.Collections.Generic;
using UnityEngine;

public class PatrolState : EnemyState // ← ИЗМЕНЕНО: Состояние патрулирования
{
    private EnemyPath _patrolPath;
    private int _currentWaypointIndex = 0;
    private const float WaypointReachThreshold = 0.5f;
    private float _sqrWaypointReachThreshold;

    public PatrolState(EnemyAI ai, EnemyMover mover, EnemyAttacker attacker, EnemyPath patrolPath) : base(ai, mover, attacker)
    {
        _patrolPath = patrolPath;
        _sqrWaypointReachThreshold = WaypointReachThreshold * WaypointReachThreshold;
    }

    public override void Enter()
    {
        _mover.SetChasingSpeed(false);
       // _attacker.StopAttacking();

        if (_patrolPath != null && _patrolPath.Count > 0)
        {
            _currentWaypointIndex = 0;
        }
    }

    public override void Update()
    {
        if (_patrolPath != null && _patrolPath.Count > 0)
        {
            Transform waypoint = _patrolPath.GetWaypoint(_currentWaypointIndex);

            if (waypoint != null)
            {
                _mover.MoveToTarget(waypoint.position);

                if (Vector2.SqrMagnitude(_ai.transform.position - waypoint.position) < _sqrWaypointReachThreshold)
                {
                    _currentWaypointIndex = (_currentWaypointIndex + 1) % _patrolPath.Count;
                }
            }
        }
        else
        {
            // Если нет пути патрулирования, просто стоим на месте
            _mover.StopMovement();
        }

        //// Проверяем, не появился ли игрок в детекторе
        //if (_ai.Player != null)
        //{
        //    _ai.TransitionToChaseState();
        //}
    }

    public override void Exit()
    {
        _mover.StopMovement();
    }
}