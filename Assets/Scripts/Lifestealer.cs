using UnityEngine;

[DisallowMultipleComponent]
public class Lifestealer : MonoBehaviour
{
    [SerializeField] [Range(0f, 1f)] private float _healRatio = 0.5f;
    [SerializeField] private BaseHealth _ownerHealth;
    [SerializeField] private Attacker _attacker;

    private void Awake()
    {
        if (_attacker == null)
            _attacker = GetComponent<Attacker>();

        if (_ownerHealth == null)
            _ownerHealth = GetComponent<BaseHealth>();

        _attacker.OnDamageDealt += HandleDamageDealt;
    }

    private void HandleDamageDealt(int damageDealt)
    {
        if (damageDealt > 0 && _healRatio > 0)
        //if (damageDealt > 0)
        {
            int healAmount = Mathf.RoundToInt(damageDealt * _healRatio);
            _ownerHealth.Heal(healAmount);
        }
    }

    private void OnDestroy()
    {
        if (_attacker != null)
            _attacker.OnDamageDealt -= HandleDamageDealt;
    }
}