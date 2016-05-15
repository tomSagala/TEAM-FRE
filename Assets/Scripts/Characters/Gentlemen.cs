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
    [SerializeField] float m_secondaryAbilityMissedCooldown = 1f;

    private float m_passiveTimer = 0f;
    private int m_bulletChamber;
    private int m_shotCount = 0;
    private int m_numberOfChambers = 6;

    private double castTimer = 1f;
    private float timer = 0f;

    private bool m_shotsFired = false;

    private FireBug m_activeFireBug;

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

        timer += Time.deltaTime;
	}

    private void ActivatePassive()
    {
        AudioSource.PlayClipAtPoint(Laugh, transform.position, 0.3f);

        float ray = Random.Range(2.5f,5f);
        float angle = Random.Range(0, 360);

        Vector3 offset = new Vector3(ray * Mathf.Sin(angle), 0f, ray * Mathf.Cos(angle));

        INetwork.Instance.Instantiate(m_passivePrefab, transform.position + offset - transform.up * GetComponent<CapsuleCollider>().height/4f, transform.rotation);
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
        m_primaryAbilityRemainingCoolDown = m_primaryAbilityCoolDown;

        m_activeFireBug = INetwork.Instance.Instantiate(
            m_secondaryAbilityProjectilePrefab,
            Camera.main.transform.position + Camera.main.transform.forward * 1.5f,
            Quaternion.LookRotation(Camera.main.transform.forward)).GetComponent<FireBug>();
        INetwork.Instance.RPC(m_activeFireBug.gameObject, "SetOwnerViewId", PhotonTargets.All, INetwork.Instance.GetViewId(gameObject));
        INetwork.Instance.RPC(m_activeFireBug.gameObject, "SetOwnerTeam", PhotonTargets.All, m_team);
        INetwork.Instance.RPC(m_activeFireBug.gameObject, "Setup", PhotonTargets.All, Random.Range(0f, 2 * Mathf.PI), Random.Range(m_activeFireBug.m_minAmplitudeX, m_activeFireBug.m_maxAmplitudeX), Random.Range(m_activeFireBug.m_minAmplitudeY, m_activeFireBug.m_maxAmplitudeY));
        INetwork.Instance.RPC(m_activeFireBug.gameObject, "AddVelocity", PhotonTargets.All, GetComponent<Rigidbody>().velocity);

        timer = 0f;

    }

    public override void UseSecondaryAbility()
    {
        m_secondaryAbilityAvailable = false;
        m_secondaryAbilityRemainingCoolDown = m_secondaryAbilityMissedCooldown;

        Debug.Log(m_shotCount + " / "  + m_bulletChamber);
        if (m_bulletChamber <= m_shotCount || m_shotCount >= 6)
        {
            m_secondaryAbilityRemainingCoolDown = m_secondaryAbilityCoolDown;

            Projectile proj = INetwork.Instance.Instantiate(
            m_primaryAbilityProjectilePrefab,
            Camera.main.transform.position + Camera.main.transform.forward,
            Quaternion.LookRotation(Camera.main.transform.forward)).GetComponent<Projectile>();

            NetworkAudioManager.Instance.PlayAudioClipForAll("Bullet", proj.transform.position, 1.0f);

            INetwork.Instance.RPC(proj.gameObject, "SetOwnerViewId", PhotonTargets.All, INetwork.Instance.GetViewId(gameObject));
            INetwork.Instance.RPC(proj.gameObject, "SetOwnerTeam", PhotonTargets.All, m_team);
            INetwork.Instance.RPC(proj.gameObject, "SetOneHitKill", PhotonTargets.All, true);
            INetwork.Instance.RPC(proj.gameObject, "AddVelocity", PhotonTargets.All, GetComponent<Rigidbody>().velocity);
            m_shotsFired = true;
        }
        else
        {
            m_shotCount++;
            AudioSource.PlayClipAtPoint(BlankShotSound, this.transform.position, 0.1f);
        }
    }

    public override void UseDoubleActivatePrimary()
    {
        if (!INetwork.Instance.IsMaster() || timer > castTimer || !CanDoubleActivate() || m_activeFireBug == null)
            return;

        INetwork.Instance.RPC(m_activeFireBug.gameObject, "Explode", PhotonTargets.All, m_activeFireBug.transform.position);

    }

    public override void PrimaryReady()
    {
        base.PrimaryReady();
        if(m_activeFireBug)
        {
            INetwork.Instance.RPC(m_activeFireBug.gameObject, "Explode", PhotonTargets.All, m_activeFireBug.transform.position);
        }
    }

    public override void SecondaryReady()
    {
        m_secondaryAbilityAvailable = true;

        if (m_bulletChamber <= m_shotCount && m_shotsFired)
        {
            reloadCouroutine = StartCoroutine(ReloadCoroutine());
        }
    }

    public override void Reload()
    {
        
    }

    public override IEnumerator ReloadCoroutine()
    {
        m_bulletChamber = Random.Range(0, m_numberOfChambers - 1);
        m_shotCount = 0;
        AudioSource.PlayClipAtPoint(ReloadSound, this.transform.position, 0.1f);
        m_shotsFired = false;
        yield break;
    }
}
