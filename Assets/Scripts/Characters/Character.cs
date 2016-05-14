using UnityEngine;
using System.Collections;

public abstract class Character : MonoBehaviour {
    [SerializeField] float m_healthPoints = 5f;
    [SerializeField] protected TeamsEnum m_team;
    [SerializeField] protected float m_autoAttackDamage = 1f;
    [SerializeField] protected float m_primaryAbilityCoolDown;
    [SerializeField] protected float m_secondaryAbilityCoolDown;
    [SerializeField] protected float m_autoAttackPerSeconds = 1f;
    protected bool m_primaryAbilityAvailable = true;
    protected bool m_secondaryAbilityAvailable = true;

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
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
        if (m_healthPoints < 0)
        {
            // DIE
            m_healthPoints = 0;
        }
    }

    public bool CanUsePrimaryAbility() { return m_primaryAbilityAvailable; }
    public bool CanUseSecondaryAbility() { return m_secondaryAbilityAvailable; }
    public virtual void Die() { Destroy(gameObject); }
    public virtual void Attack() { }
    public virtual void UsePrimaryAbility() { }
    public virtual void UseSecondaryAbility() { }
}
