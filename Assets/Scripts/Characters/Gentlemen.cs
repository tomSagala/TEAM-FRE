using UnityEngine;
using System.Collections;

public class Gentlemen : Character {
    [SerializeField] float m_PassiveCooldown = 10f;
    [SerializeField] GameObject m_passivePrefab;
    [SerializeField] GameObject m_primaryAbilityProjectilePrefab;
    [SerializeField] float m_primaryAbilityProjectileSpeed = 45f;
    private float m_passiveTimer = 0f;
    private int m_bulletChamber;
    private int m_shotCount = 0;
    private int m_numberOfChambers = 6;

	// Use this for initialization
	void Start () {
        m_bulletChamber = Random.Range(0, m_numberOfChambers - 1);
	}
	
	// Update is called once per frame
	void Update () {
        if (m_passiveTimer < m_PassiveCooldown)
        {
            m_passiveTimer += Time.deltaTime;
        }
        else
        {
            ActivatePassive();
            m_passiveTimer = 0f;
        }
	}

    private void ActivatePassive()
    {
        Instantiate(m_passivePrefab, transform.position - transform.up * GetComponent<CapsuleCollider>().height/2.0f, transform.rotation);
    }

    public override void UsePrimaryAbility()
    {
        Debug.Log("Shoot Count : " + m_shotCount + " bullet in chamber : " + m_bulletChamber);
        if (m_bulletChamber == m_shotCount)
        {
            Debug.Log("Bang");
            m_primaryAbilityAvailable = false;
            Vector3 pos = GetComponentInChildren<Camera>().transform.position;
            Quaternion orientation = GetComponentInChildren<Camera>().transform.rotation;

            GameObject bullet = Instantiate(m_primaryAbilityProjectilePrefab, pos, orientation) as GameObject;
            bullet.GetComponent<Rigidbody>().velocity = GetComponentInChildren<Camera>().transform.forward * m_primaryAbilityProjectileSpeed;
            bullet.GetComponent<Rigidbody>().useGravity = false;
            bullet.GetComponent<Projectile>().SetOneHitKill(true);
            bullet.GetComponent<Projectile>().SetFiredBy(m_team);

            Timer.Instance.Request(m_primaryAbilityCoolDown, Reload);
        }
        else
        {
            Debug.Log("Click");
            m_shotCount++;
        }
    }

    void Reload()
    {
        m_bulletChamber = Random.Range(0, m_numberOfChambers - 1);
        m_shotCount = 0;
        m_primaryAbilityAvailable = true;
    }

}
