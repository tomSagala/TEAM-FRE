using UnityEngine;
using System.Collections;
using System;
using System.Linq;

public class Gnome : Character 
{
    public float CloverRadius;
    public GameObject CloverProjectilePrefab;
    public GameObject CloverSmallPrefab;
    public GameObject CloverMediumPrefab;
    public GameObject CloverBigPrefab;
    public GameObject GoldBagPrefab;
    public GameObject TicketPrefab;
    public float GoldBagRadius;
    public float GoldBagHealRate;
    private GameObject m_healQuad;
    private GameObject m_ticketSphere;
    public float TicketStunDuration;
    
    void Start()
    {
        m_healQuad = transform.Find("HealingQuad").gameObject;
        m_healQuad.GetComponent<GnomeHealingQuad>().HealRate = GoldBagHealRate;
        m_ticketSphere = transform.Find("TicketSphere").gameObject;
        m_ticketSphere.GetComponent<GnomeTicketSphere>().Duration = TicketStunDuration;
    }

    public override void Attack()
    {
        m_autoAttackAvailable = false;
        m_autoAttackRemainingCoolDown = 1.0f / m_autoAttackPerSeconds;

        GnomeClover clover = INetwork.Instance.Instantiate(
            CloverProjectilePrefab, 
            Camera.main.transform.position + Camera.main.transform.forward, 
            Quaternion.LookRotation(transform.forward)).GetComponent<GnomeClover>();
        INetwork.Instance.RPC(clover.gameObject, "SetOwnerViewId", PhotonTargets.All, INetwork.Instance.GetViewId(gameObject));
        INetwork.Instance.RPC(clover.gameObject, "SetOwnerTeam", PhotonTargets.All, m_team);
    }

    public override void UsePrimaryAbility()
    {
        m_primaryAbilityAvailable = false;
        m_primaryAbilityRemainingCoolDown = m_primaryAbilityCoolDown;

        GnomeGoldBag goldBag = INetwork.Instance.Instantiate(
            GoldBagPrefab,
            Camera.main.transform.position + Camera.main.transform.forward,
            Quaternion.LookRotation(transform.forward)).GetComponent<GnomeGoldBag>();
        INetwork.Instance.RPC(goldBag.gameObject, "SetOwnerViewId", PhotonTargets.All, INetwork.Instance.GetViewId(gameObject));
        INetwork.Instance.RPC(goldBag.gameObject, "SetOwnerTeam", PhotonTargets.All, m_team);

    }

    public override void UseSecondaryAbility()
    {
        m_secondaryAbilityAvailable = false;
        m_secondaryAbilityRemainingCoolDown = m_secondaryAbilityCoolDown;

        GnomeTicket ticket = INetwork.Instance.Instantiate(
            TicketPrefab,
            Camera.main.transform.position + Camera.main.transform.forward,
            Quaternion.LookRotation(transform.forward)).GetComponent<GnomeTicket>();
        INetwork.Instance.RPC(ticket.gameObject, "SetOwnerViewId", PhotonTargets.All, INetwork.Instance.GetViewId(gameObject));
        INetwork.Instance.RPC(ticket.gameObject, "SetOwnerTeam", PhotonTargets.All, m_team);

    }

    [PunRPC]
    public void CloverCollision(Vector3 position)
    {
        if (!INetwork.Instance.IsMine(gameObject))
            return;

        RaycastHit[] hits = Physics.SphereCastAll(position, CloverRadius, Vector3.up);
        int nbCloverAlreadyThere = hits.Count(p => Helpers.CheckObjectTag(p.collider.gameObject, "Clover"));
        
        int nb = Mathf.Max(24 - nbCloverAlreadyThere, 0) / 3;
        Action<GameObject> spawn = (prefab) =>
        {
            float theta = UnityEngine.Random.value * 2 * Mathf.PI;
            float rho = UnityEngine.Random.value * CloverRadius;

            Vector3 pos = position + new Vector3(rho * Mathf.Cos(theta), 0, rho * Mathf.Sin(theta));
            INetwork.Instance.Instantiate(prefab, pos, Quaternion.Euler(new Vector3(0, UnityEngine.Random.value * 2 * Mathf.PI)));
        };
        for (int i = 0 ; i < nb ; ++i)
        {
            spawn(CloverSmallPrefab);
            spawn(CloverMediumPrefab);
            spawn(CloverBigPrefab);
        }
    }

    [PunRPC]
    public void GoldBagCollision(Vector3 position)
    {
        m_healQuad.transform.position = position;
        m_healQuad.transform.parent = null;
        m_healQuad.SetActive(true);
        NetworkAudioManager.Instance.PlayAudioClipLocally("HealingQuad", m_healQuad.transform.position, 1.0f);
    }

    [PunRPC]
    public void GoldBagEnd(Vector3 position)
    {
        m_healQuad.transform.parent = transform;
        m_healQuad.transform.localPosition = Vector3.zero;
        m_healQuad.SetActive(false);
    }

    [PunRPC]
    public void TicketCollision(Vector3 position)
    {
        m_ticketSphere.transform.position = position;
        m_ticketSphere.transform.parent = null;
        m_ticketSphere.SetActive(true);
    }

    [PunRPC]
    public void TicketEnd(Vector3 position)
    {
        m_ticketSphere.transform.parent = transform;
        m_ticketSphere.transform.localPosition = Vector3.zero;
        m_ticketSphere.SetActive(false);
    }
}
