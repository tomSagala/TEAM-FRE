using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour {
    [SerializeField] float m_damage;
    [SerializeField] TeamsEnum m_firedBy;
    [SerializeField] bool isOneHitKill = false;
    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }

    public void SetDamage(float damage)
    {
        m_damage = damage;
    }

    public void SetFiredBy(TeamsEnum firedBy)
    {
        m_firedBy = firedBy;
    }

    public void SetOneHitKill(bool isOneHitKill)
    {

    }

    void OnCollisionEnter(Collision collision)
    {
        if (Helpers.CheckObjectTag(collision.gameObject, "Player") && collision.gameObject.GetComponent<Character>().GetTeam() != m_firedBy)
        {
            if (isOneHitKill)
            {
                collision.gameObject.GetComponent<Character>().Die();
            }
            else
            {
                collision.gameObject.GetComponent<Character>().TakeDamage(m_damage);
            }
        }
    }

}
