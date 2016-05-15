using UnityEngine;
using System.Collections;

public class Gentlemen : Character 
{
    [SerializeField] float m_PassiveCooldown = 10f;
    [SerializeField] GameObject m_passivePrefab;
    [SerializeField] GameObject m_primaryAbilityProjectilePrefab;
    [SerializeField] GameObject m_secondaryAbilityProjectilePrefab;
    [SerializeField] float m_autoAttackRange = 1f;
    [SerializeField] float m_damageOverTimeTotalDamage = 1f;
    [SerializeField] float m_damageOverTimeDuration = 1f;
    [SerializeField] [Range(0, 1)] float m_chanceToDotOnHit;
    [SerializeField] float m_primaryAbilityMissedCooldown = 1f;

    private float m_passiveTimer = 0f;
    private int m_bulletChamber;
    private int m_shotCount = 0;
    private int m_numberOfChambers = 6;

    private AudioSource footSteps;

    [SerializeField] AudioClip ReloadSound;
    [SerializeField] AudioClip BlankShotSound;
    [SerializeField] AudioClip Laugh;

	// Use this for initialization
	void Start ()
    {
        m_bulletChamber = Random.Range(0, m_numberOfChambers - 1);
        footSteps = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (m_passiveTimer < m_PassiveCooldown)
        {
            m_passiveTimer += Time.deltaTime;
        }
        else
        {
            ActivatePassive();
            m_passiveTimer = 0f;
        }

        if (!footSteps.isPlaying && 
            GetComponent<RigidbodyFirstPersonController>().Velocity.magnitude > 0.15f &&
            GetComponent<RigidbodyFirstPersonController>().Grounded)
        {
            footSteps.Play();
        }
	}

    private void ActivatePassive()
    {
        AudioSource.PlayClipAtPoint(Laugh, transform.position, 0.3f);
        Instantiate(m_passivePrefab, transform.position - transform.up * GetComponent<CapsuleCollider>().height/2.0f, transform.rotation);
    }

    public override void Attack()
    {
        m_autoAttackAvailable = false;
        m_autoAttackRemainingCoolDown = 1.0f / m_autoAttackPerSeconds;

        Transform cam = GetComponentInChildren<Camera>().transform;

        RaycastHit hitlow;
        RaycastHit hitmid;
        RaycastHit hithigh;

        bool collision = Physics.Raycast(cam.position, Quaternion.AngleAxis(30, Vector3.Normalize(cam.up + cam.right)) * cam.forward, out hitlow, m_autoAttackRange);
        collision = Physics.Raycast(cam.position, cam.forward, out hitmid, m_autoAttackRange) || collision;
        collision = Physics.Raycast(cam.position, Quaternion.AngleAxis(30, Vector3.Normalize(cam.up + cam.right)) * cam.forward, out hithigh, m_autoAttackRange) || collision;

        if (collision)
        {
            if (hitlow.collider != null && hitlow.collider.GetComponent<Character>() != null && hitlow.collider.GetComponent<Character>().GetTeam() != m_team)
            {
                INetwork.Instance.RPC(hitlow.collider.gameObject, "TakeDamage", PhotonTargets.All, m_autoAttackDamage);
                if (Random.Range(0, 1) < m_chanceToDotOnHit)
                {
                    INetwork.Instance.RPC(hitlow.collider.gameObject, "TakeDamageOverTime", PhotonTargets.All, m_damageOverTimeTotalDamage / m_damageOverTimeDuration, m_damageOverTimeDuration);
                }
            }

            if (hitmid.collider != null && hitlow.collider != hitmid.collider && hitmid.collider.GetComponent<Character>() != null && hitmid.collider.GetComponent<Character>().GetTeam() != m_team)
            {
                INetwork.Instance.RPC(hitmid.collider.gameObject, "TakeDamage", PhotonTargets.All, m_autoAttackDamage);
                if (Random.Range(0, 1) < m_chanceToDotOnHit)
                {
                    INetwork.Instance.RPC(hitmid.collider.gameObject, "TakeDamageOverTime", PhotonTargets.All, m_damageOverTimeTotalDamage / m_damageOverTimeDuration, m_damageOverTimeDuration);
                }
            }

            if (hithigh.collider != null && hitlow.collider != hithigh.collider && hitmid.collider != hithigh.collider && hithigh.collider.GetComponent<Character>() != null && hithigh.collider.GetComponent<Character>().GetTeam() != m_team)
            {
                INetwork.Instance.RPC(hithigh.collider.gameObject, "TakeDamage", PhotonTargets.All, m_autoAttackDamage);
                if (Random.Range(0, 1) < m_chanceToDotOnHit)
                {
                    INetwork.Instance.RPC(hithigh.collider.gameObject, "TakeDamageOverTime", PhotonTargets.All, m_damageOverTimeTotalDamage / m_damageOverTimeDuration, m_damageOverTimeDuration);
                }
            }
        }

    }

    public override void UsePrimaryAbility()
    {
        m_primaryAbilityAvailable = false;
        m_primaryAbilityRemainingCoolDown = m_primaryAbilityMissedCooldown;
        if (m_bulletChamber == m_shotCount)
        {
            m_primaryAbilityRemainingCoolDown = m_primaryAbilityCoolDown;

            Projectile proj = INetwork.Instance.Instantiate(
            m_primaryAbilityProjectilePrefab,
            Camera.main.transform.position + Camera.main.transform.forward,
            Quaternion.LookRotation(transform.forward)).GetComponent<Projectile>();

            NetworkAudioManager.Instance.PlayAudioClipForAll("Bullet", proj.transform.position, 1.0f);

            INetwork.Instance.RPC(proj.gameObject, "SetOwnerViewId", PhotonTargets.All, INetwork.Instance.GetViewId(gameObject));
            INetwork.Instance.RPC(proj.gameObject, "SetOwnerTeam", PhotonTargets.All, m_team);
            INetwork.Instance.RPC(proj.gameObject, "SetOneHitKill", PhotonTargets.All, true);
        }
        else
        {
            m_shotCount++;
            AudioSource.PlayClipAtPoint(BlankShotSound, this.transform.position, 0.1f);
        }
    }

    public override void UseSecondaryAbility()
    {
        m_secondaryAbilityAvailable = false;
        m_secondaryAbilityRemainingCoolDown = m_secondaryAbilityCoolDown;

        FireBug proj = INetwork.Instance.Instantiate(
            m_secondaryAbilityProjectilePrefab,
            Camera.main.transform.position + Camera.main.transform.forward,
            Quaternion.LookRotation(transform.forward)).GetComponent<FireBug>();
        INetwork.Instance.RPC(proj.gameObject, "SetOwnerViewId", PhotonTargets.All, INetwork.Instance.GetViewId(gameObject));
        INetwork.Instance.RPC(proj.gameObject, "SetOwnerTeam", PhotonTargets.All, m_team);
        INetwork.Instance.RPC(proj.gameObject, "Setup", PhotonTargets.All, Random.Range(0f, 2 * Mathf.PI), Random.Range(proj.m_minAmplitudeX, proj.m_maxAmplitudeX), Random.Range(proj.m_minAmplitudeY, proj.m_maxAmplitudeY));
    }

    public override void PrimaryReady()
    {
        m_primaryAbilityAvailable = true;

        if ((m_bulletChamber <= m_shotCount))
        {
            Reload();
        }
    }

    private void Reload()
    {
        m_bulletChamber = Random.Range(0, m_numberOfChambers - 1);
        m_shotCount = 0;
        AudioSource.PlayClipAtPoint(ReloadSound, this.transform.position, 0.1f);
    }
}
