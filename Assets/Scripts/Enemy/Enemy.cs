using UnityEngine;

[DisallowMultipleComponent, RequireComponent(typeof(EnemyMover), typeof(EnemyHealth), typeof(EnemyAI))]
public class Enemy : MonoBehaviour
{
    [SerializeField] private EnemySettings _settings;

    private EnemyMover _mover;
    private EnemyHealth _health;
    private EnemyAI _ai;

    public event System.Action<Enemy> Died; 

    public EnemyMover Movement => _mover;

    private void Awake()
    {
        _mover = GetComponent<EnemyMover>();
        _health = GetComponent<EnemyHealth>();
        _ai = GetComponent<EnemyAI>();

        _health.Died += OnHealthDied;

        ApplyEnemySettings();
    }

    private void OnDestroy()
    {
        if (_health != null)
            _health.Died -= OnHealthDied;
    }

    private void OnHealthDied(BaseHealth health)
    {
        Died?.Invoke(this);
    }

    public void ResetEnemy()
    {
        transform.rotation = Quaternion.identity;
        _health.Init(); 
        _ai.ResetAI();
    }

    public void Initialize(EnemyPath path)
    {
        if (path != null)
        {
            _mover.Initialize(path);
            _ai.SetPatrolPath(path);
        }
    }

    public void ApplySettings(EnemySettings settings)
    {
        _settings = settings;
        ApplyEnemySettings();
    }

    private void ApplyEnemySettings()
    {
        if (_settings != null)
        {
            _mover?.ApplySettings(_settings);
            _health?.ApplySettings(_settings);
            _ai?.ApplySettings(_settings);

            transform.localScale = Vector3.one * _settings.EnemyScale;
        }
    }
}