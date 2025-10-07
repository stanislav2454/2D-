using UnityEngine;

[RequireComponent(typeof(EnemyMover), typeof(EnemyAttacker))]
public class EnemyAI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private EnemySettings _settings;
    [SerializeField] private EnemyMover _mover;
    [SerializeField] private Detector _detector;
    [SerializeField] private EnemyAttacker _attacker;

    private EnemyPath _patrolPath;
    private Transform _player;
    private EnemyStateMachine _stateMachine;

    public EnemyMover Mover => _mover;
    public EnemyAttacker Attacker => _attacker;
    public Detector Detector => _detector;
    public EnemySettings Settings => _settings;
    public Transform Player => _player;
    public EnemyPath PatrolPath => _patrolPath;

    private bool _isInitialized = false;

    private void Awake()
    {
        InitializeComponents();
    }

    private void Start()
    {
        if (_isInitialized)
            _stateMachine.ChangeState<PatrolState>();
    }

    private void OnEnable()
    {
        // Проверяем инициализацию перед подпиской на события
        if (_isInitialized == false)
            InitializeComponents();

        if (_detector != null)
        {
            _detector.TargetDetected += OnTargetDetected;
            _detector.TargetLost += OnTargetLost;
        }

        // Запускаем состояние только если компоненты инициализированы
        if (_isInitialized && _stateMachine != null)
        {
            _stateMachine.ChangeState<PatrolState>();
        }
    }

    private void OnDisable()
    {
        if (_detector != null)
        {
            _detector.TargetDetected -= OnTargetDetected;
            _detector.TargetLost -= OnTargetLost;
        }
    }

    private void Update()
    {
        if (_isInitialized)
            _stateMachine.Update();
    }

    private void FixedUpdate()
    {
        if (_isInitialized)
            _stateMachine.FixedUpdate();
    }

    public void ResetAI()
    {
        _player = null;

        if (_isInitialized && _stateMachine != null)
            _stateMachine.ChangeState<PatrolState>();
    }

    public void SetPatrolPath(EnemyPath path)    =>
        _patrolPath = path;    

    public void ApplySettings(EnemySettings settings)
    {
        _settings = settings;
        _attacker?.ApplyEnemySettings(settings);

        if (_detector != null && _settings != null)
            _detector.SetDetectionRadius(_settings.DetectionRadius);

        if (_mover != null && _settings != null)
            _mover.ApplySettings(settings);
    }

    private void InitializeComponents()
    {
        if (_mover == null)
            _mover = GetComponent<EnemyMover>();

        if (_attacker == null)
            _attacker = GetComponent<EnemyAttacker>();

        if (_detector == null)
            _detector = GetComponent<Detector>();

        _stateMachine = new EnemyStateMachine(this);
        _isInitialized = true;
    }

    private void OnTargetDetected(Transform playerTransform)
    {
        _player = playerTransform;
        _attacker?.Initialize(_player);
    }

    private void OnTargetLost() =>
        _player = null;

    private void OnDrawGizmosSelected()
    {
        if (_settings != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, _settings.AttackRange);
        }
    }
}