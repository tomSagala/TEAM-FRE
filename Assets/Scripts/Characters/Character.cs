using UnityEngine;
using System.Collections;

public abstract class Character : MonoBehaviour {
    [SerializeField] float m_healthPoints;
    private TeamsEnum m_team;

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
    }

    public virtual void UsePrimaryAbility() { }
    public virtual void UseSecondaryAbility() { }
}
