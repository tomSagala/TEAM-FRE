using UnityEngine;
using System.Collections;

public class Knight : Character {

    public GameObject m_horseShoeProjectilePrefab;

    public GameObject m_rabbotFootPrefab;
    public float rabbitFootThrowSpread;
    public int rabbitFootPerThrow;

    public float m_chargeSpeed;
    public float m_chargeLength;
    public float m_chargeFOVModifier;

    public bool isCharging = false;

    [SerializeField]
    float footStepsDuration;
    private float footStepsTimer;
    private AudioSource footSteps;
    private Animator m_animator;

    void Start()
    {
        footSteps = GetComponent<AudioSource>();
        m_animator = GetComponentInChildren<Animator>();
    }

    new void FixedUpdate()
    {
        base.FixedUpdate();
        if (isCharging)
        {
            GetComponent<Rigidbody>().velocity = m_chargeSpeed * Camera.main.transform.forward;
        }
        m_animator.SetBool("IsCharging", isCharging);
    }

    void Update()
    {
        if (!footSteps.isPlaying &&
        GetComponent<RigidbodyFirstPersonController>().Velocity.magnitude > 0.15f &&
        GetComponent<RigidbodyFirstPersonController>().Grounded)
        {
            footSteps.Play();
        }
        else if (footSteps.isPlaying)
        {
            if (footStepsTimer < 0)
            {
                footSteps.Stop();
                footStepsTimer = footStepsDuration;
            }
            else
            {
                footStepsTimer -= Time.deltaTime;
            }
        }
    }

    public override void Attack()
    {
        if (reloadCouroutine != null) return;

        if (m_currentAmmo <= 0)
        {
            reloadCouroutine = StartCoroutine(ReloadCoroutine());
            return;
        }

        m_autoAttackAvailable = false;
        m_autoAttackRemainingCoolDown = 1.0f / m_autoAttackPerSeconds;
        HorseShoe hs = INetwork.Instance.Instantiate(
            m_horseShoeProjectilePrefab,
            Camera.main.transform.position + Camera.main.transform.forward,
            Quaternion.LookRotation(Camera.main.transform.forward)).GetComponent<HorseShoe>();
        INetwork.Instance.RPC(hs.gameObject, "SetOwnerViewId", PhotonTargets.All, INetwork.Instance.GetViewId(gameObject));
        INetwork.Instance.RPC(hs.gameObject, "SetOwnerTeam", PhotonTargets.All, m_team);
        INetwork.Instance.RPC(hs.gameObject, "AddVelocity", PhotonTargets.All, GetComponent<Rigidbody>().velocity);

        m_currentAmmo--;
    }

    public override void UsePrimaryAbility()
    {
        m_primaryAbilityAvailable = false;
        m_primaryAbilityRemainingCoolDown = m_primaryAbilityCoolDown;

        m_actionblocked = true;
        isCharging = true;

        GetComponent<Rigidbody>().useGravity = false;

        Camera.main.fieldOfView *= m_chargeFOVModifier;
        StartCoroutine(DashTimer());
    }

    public override void UseSecondaryAbility()
    {
        m_secondaryAbilityAvailable = false;
        m_secondaryAbilityRemainingCoolDown = m_secondaryAbilityCoolDown;
       
        for (int i = 0; i < rabbitFootPerThrow; i++)
        {
            float angle = ((float)i) / rabbitFootPerThrow * rabbitFootThrowSpread - rabbitFootThrowSpread / 2f;
            Quaternion dir = Quaternion.AngleAxis(angle, Camera.main.transform.up);
            RabbitFoot rf = INetwork.Instance.Instantiate(
            m_rabbotFootPrefab,
            Camera.main.transform.position + dir * Camera.main.transform.forward,
            dir * Camera.main.transform.rotation).GetComponent<RabbitFoot>();
            INetwork.Instance.RPC(rf.gameObject, "SetOwnerViewId", PhotonTargets.All, INetwork.Instance.GetViewId(gameObject));
            INetwork.Instance.RPC(rf.gameObject, "SetOwnerTeam", PhotonTargets.All, m_team);
            INetwork.Instance.RPC(rf.gameObject, "AddVelocity", PhotonTargets.All, GetComponent<Rigidbody>().velocity);

        }

    }

    IEnumerator DashTimer()
    {
        yield return new WaitForSeconds(m_chargeLength);

        isCharging = false;
        m_actionblocked = false;
        Camera.main.fieldOfView /= m_chargeFOVModifier;
        GetComponent<Rigidbody>().useGravity = true;
        yield return null;
    }
}
