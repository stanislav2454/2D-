public class EnemyState
{
    protected EnemyStateMachine StateMachine;
    protected EnemyAI EnemyAI;
    protected EnemyMover Mover;
    protected EnemyAttacker Attacker;
    protected Detector Detector;
    protected EnemySettings EnemySettings;

    public EnemyState(EnemyStateMachine stateMachine, EnemyAI enemyAI)
    {
        StateMachine = stateMachine;
        EnemyAI = enemyAI;
        Mover = enemyAI.Mover;
        Attacker = enemyAI.Attacker;
        Detector = enemyAI.Detector;
        EnemySettings = enemyAI.Settings;
    }

    public virtual void Enter() { }
    public virtual void Update() { }
    public virtual void FixedUpdate() { }
    public virtual void Exit() { }
}