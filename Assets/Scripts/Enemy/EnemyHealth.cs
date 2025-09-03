public class EnemyHealth : BaseHealth
{
    private EnemyPool _pool;

    public void SetPool(EnemyPool pool) =>
        _pool = pool;

    public override void Die()
    {
        base.Die();

        _pool?.ReleaseEnemy(GetComponent<Enemy>());
    }
}