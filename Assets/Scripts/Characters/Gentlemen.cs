using UnityEngine;
using System.Collections;

public class Gentlemen : Character {
    [SerializeField] float m_PassiveCooldown = 10f;
    [SerializeField] GameObject m_passivePrefab;
    [SerializeField] GameObject m_primaryAbilityProjectilePrefab;
    [SerializeField] float m_primaryAbilityProjectileSpeed = 45f;
    [SerializeField] GameObject m_secondaryAbilityProjectilePrefab;
    [SerializeField] float m_secondaryAbilityProjectileSpeed = 25f;
    [SerializeField] float m_autoAttackRange = 1f;
    [SerializeField] float m_damageOverTimeTotalDamage = 1f;
    [SerializeField] float m_damageOverTimeDuration = 1f;
    [SerializeField] [Range(0, 1)] float m_chanceToDotOnHit;

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

    public override void Attack()
    {
        Debug.Log("attacking");
        Transform cam = GetComponentInChildren<Camera>().transform;

        RaycastHit hitlow;
        RaycastHit hitmid;
        RaycastHit hithigh;

        bool collision = Physics.Raycast(cam.position, Quaternion.AngleAxis(30, Vector3.Normalize(cam.up + cam.right)) * cam.forward, out hitlow, m_autoAttackRange);
        collision = Physics.Raycast(cam.position, cam.forward, out hitmid, m_autoAttackRange) || collision;
        collision = Physics.Raycast(cam.position, Quaternion.AngleAxis(30, Vector3.Normalize(cam.up + cam.right)) * cam.forward, out hithigh, m_autoAttackRange) || collision;

        if (collision)
        {
            if (hitlow.collider.GetComponent<Character>() != null && hitlow.collider.GetComponent<Character>().GetTeam() != m_team)
            {
                hitlow.collider.gameObject.GetComponent<Character>().TakeDamage(m_autoAttackDamage);
                if (Random.Range(0, 1) < m_chanceToDotOnHit)
                {
                    hitlow.collider.gameObject.GetComponent<Character>().TakeDamageOverTime(m_damageOverTimeTotalDamage/m_damageOverTimeDuration, m_damageOverTimeDuration);
                }
            }

            if (hitlow.collider != hitmid.collider && hitmid.collider.GetComponent<Character>() != null && hitmid.collider.GetComponent<Character>().GetTeam() != m_team)
            {
                hitmid.collider.gameObject.GetComponent<Character>().TakeDamage(m_autoAttackDamage);
                if (Random.Range(0, 1) < m_chanceToDotOnHit)
                {
                    hitmid.collider.gameObject.GetComponent<Character>().TakeDamageOverTime(m_damageOverTimeTotalDamage / m_damageOverTimeDuration, m_damageOverTimeDuration);
                }
            }

            if (hitlow.collider != hithigh.collider && hitmid.collider != hithigh.collider && hithigh.collider.GetComponent<Character>() != null && hithigh.collider.GetComponent<Character>().GetTeam() != m_team)
            {
                hithigh.collider.gameObject.GetComponent<Character>().TakeDamage(m_autoAttackDamage);
                if (Random.Range(0, 1) < m_chanceToDotOnHit)
                {
                    hithigh.collider.gameObject.GetComponent<Character>().TakeDamageOverTime(m_damageOverTimeTotalDamage / m_damageOverTimeDuration, m_damageOverTimeDuration);
                }
            }
        }

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
        }
        else
        {
            Debug.Log("Click");
            m_shotCount++;
        }
    }

    public override void UseSecondaryAbility()
    {
        Debug.Log("FireBug");

        m_secondaryAbilityAvailable = false;

        Vector3 pos = GetComponentInChildren<Camera>().transform.position;
        Quaternion orientation = GetComponentInChildren<Camera>().transform.rotation;

        GameObject fireBug = Instantiate(m_secondaryAbilityProjectilePrefab, pos, orientation) as GameObject;
        fireBug.GetComponent<Rigidbody>().useGravity = false;
        fireBug.GetComponent<FireBug>().SetFiredBy(m_team);
        fireBug.GetComponent<FireBug>().SetSpeed(m_secondaryAbilityProjectileSpeed);
        fireBug.GetComponent<FireBug>().Setup();
    }

    public override void PrimaryReady()
    {
        m_bulletChamber = Random.Range(0, m_numberOfChambers - 1);
        m_shotCount = 0;
        m_primaryAbilityAvailable = true;
    }
}
