using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour {
    [SerializeField] float m_damage;
    [SerializeField] TeamsEnum m_firedBy;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void SetDamage(float damage)
    {
        m_damage = damage;
    }

    void SetFiredBy(TeamsEnum firedBy)
    {
        m_firedBy = firedBy;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (Helpers.CheckObjectTag(collision.gameObject, "Player") && collision.gameObject.GetComponent<Character>().GetTeam() != m_firedBy)
        {
            collision.gameObject.GetComponent<Character>().TakeDamage(m_damage);
        }
    }
}
