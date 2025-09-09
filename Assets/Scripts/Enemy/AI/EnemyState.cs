//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

public abstract class EnemyState // ← ИЗМЕНЕНО: Базовый класс для состояний AI
{
    protected EnemyAI _ai;
    protected EnemyMover _mover;
    protected EnemyAttacker _attacker;

    public EnemyState(EnemyAI ai, EnemyMover mover, EnemyAttacker attacker)
    {
        _ai = ai;
        _mover = mover;
        _attacker = attacker;
    }

    public abstract void Enter();
    public abstract void Update();
    public abstract void Exit();
}
