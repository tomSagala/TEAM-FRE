using UnityEngine;
using System.Collections;

public abstract class Character : MonoBehaviour {
    [SerializeField] float m_maxHealthPoints = 5f;
    [SerializeField] float m_healthPoints = 5f;
    [SerializeField] protected TeamsEnum m_team;
    [SerializeField] protected float m_autoAttackDamage = 1f;
    [SerializeField] public float m_primaryAbilityCoolDown;
    [SerializeField] public float m_primaryAbilityRemainingCoolDown;
    [SerializeField] public float m_secondaryAbilityCoolDown;
    [SerializeField] public float m_secondaryAbilityRemainingCoolDown;
    [SerializeField] public float m_autoAttackPerSeconds = 1f;
    [SerializeField] public int m_maxAmmo;
    [SerializeField] public int m_currentAmmo;
    [SerializeField] public bool m_isMelee;

    private float m_damageOverTimeTakenDPS = 0f;
    private float m_damageOverTimeTakenRemainingTime = 0f;


    protected bool m_primaryAbilityAvailable = true;
    protected bool m_secondaryAbilityAvailable = true;
    protected bool m_actionblocked = false;

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

        if (!m_primaryAbilityAvailable)
        {
            m_primaryAbilityRemainingCoolDown -= Time.fixedDeltaTime;
            if (m_primaryAbilityRemainingCoolDown <= 0f)
            {
                PrimaryReady();
            }
        }

        if (!m_secondaryAbilityAvailable)
        {
            m_secondaryAbilityRemainingCoolDown -= Time.fixedDeltaTime;
            if (m_secondaryAbilityRemainingCoolDown <= 0f)
            {
                secondaryReady();
            }
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

    [PunRPC]
    public virtual void TakeDamage(float damage)
    {
        m_healthPoints -= damage;
        if (m_healthPoints > m_maxHealthPoints)
            m_healthPoints = m_maxHealthPoints;

        if (INetwork.Instance.IsMaster() && m_healthPoints < 0)
        {
            // DIE
            m_healthPoints = 0;
            INetwork.Instance.RPC(gameObject, "Die", PhotonTargets.All);
        }
    }

    public virtual void TakeDamageOverTime(float dps, float duration)
    {
        m_damageOverTimeTakenDPS -= dps;
        m_damageOverTimeTakenRemainingTime = duration;
    }

    public bool CanUsePrimaryAbility() { return m_primaryAbilityAvailable && !m_actionblocked; }
    public bool CanUseSecondaryAbility() { return m_secondaryAbilityAvailable && !m_actionblocked; }

    [PunRPC]
    public virtual void Die() 
    {
        if (!INetwork.Instance.IsMine(gameObject))
            return;

        INetwork.Instance.NetworkDestroy(gameObject); 
    }

    public virtual void Attack() { }
    public virtual void UsePrimaryAbility() { }
    public virtual void UseSecondaryAbility() { }

    public virtual void PrimaryReady() { m_primaryAbilityAvailable = true; }
    public virtual void secondaryReady() { m_secondaryAbilityAvailable = true; }
}
