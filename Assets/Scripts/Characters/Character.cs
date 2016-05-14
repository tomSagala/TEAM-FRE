using UnityEngine;
using System.Collections;

public abstract class Character : MonoBehaviour {
    [SerializeField] float m_healthPoints = 5f;
    [SerializeField] protected TeamsEnum m_team;
    [SerializeField] protected float m_autoAttackDamage = 1f;
    [SerializeField] protected float m_primaryAbilityCoolDown;
    [SerializeField] protected float m_secondaryAbilityCoolDown;
    [SerializeField] protected float m_autoAttackPerSeconds = 1f;
    [SerializeField] public int m_maxAmmo;
    [SerializeField] public int m_currentAmmo;
    [SerializeField] public bool m_isMelee;

    private float m_damageOverTimeTakenDPS = 0f;
    private float m_damageOverTimeTakenRemainingTime = 0f;


    protected bool m_primaryAbilityAvailable = true;
    protected bool m_secondaryAbilityAvailable = true;

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        if (m_damageOverTimeTakenRemainingTime > 0f)
        {
            TakeDamage(m_damageOverTimeTakenDPS * Time.fixedDeltaTime);
            m_damageOverTimeTakenRemainingTime -= Time.fixedDeltaTime;
        }
	}

    public void SetTeam(TeamsEnum team)
    {
        m_team = team;
    }

    public TeamsEnum GetTeam()
    {
        return m_team;
    }

    public void SetHealthPoints(float hp)
    {
        m_healthPoints = hp;
    }

    public float GetHealthPoints()
    {
        return m_healthPoints;
    }

    public virtual void TakeDamage(float damage)
    {
        m_healthPoints -= damage;
    }

    public virtual void TakeDamageOverTime(float dps, float duration)
    {
        m_damageOverTimeTakenDPS -= dps;
        m_damageOverTimeTakenRemainingTime = duration;
    }

    public bool CanUsePrimaryAbility() { return m_primaryAbilityAvailable; }
    public bool CanUseSecondaryAbility() { return m_secondaryAbilityAvailable; }
    public virtual void Die() { Destroy(gameObject); }
    public virtual void Attack() { }
    public virtual void UsePrimaryAbility() { }
    public virtual void UseSecondaryAbility() { }
}
