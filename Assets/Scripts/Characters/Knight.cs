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

    void Start()
    {
       
    }

    void FixedUpdate()
    {
        if (isCharging)
        {
            GetComponent<Rigidbody>().velocity = m_chargeSpeed * Camera.main.transform.forward;
        }
    }

    public override void Attack()
    {
        HorseShoe hs = INetwork.Instance.Instantiate(
            m_horseShoeProjectilePrefab,
            Camera.main.transform.position + Camera.main.transform.forward,
            Quaternion.LookRotation(transform.forward)).GetComponent<HorseShoe>();
        INetwork.Instance.RPC(hs.gameObject, "SetOwnerViewId", PhotonTargets.All, INetwork.Instance.GetViewId(gameObject));
        INetwork.Instance.RPC(hs.gameObject, "SetOwnerTeam", PhotonTargets.All, m_team);
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
            Quaternion dir = Quaternion.Euler(0, angle, 0) * Quaternion.LookRotation(transform.forward);
            RabbitFoot rf = INetwork.Instance.Instantiate(
            m_rabbotFootPrefab,
            Camera.main.transform.position + dir * Camera.main.transform.forward,
            dir).GetComponent<RabbitFoot>();
            INetwork.Instance.RPC(rf.gameObject, "SetOwnerViewId", PhotonTargets.All, INetwork.Instance.GetViewId(gameObject));
            INetwork.Instance.RPC(rf.gameObject, "SetOwnerTeam", PhotonTargets.All, m_team);
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
