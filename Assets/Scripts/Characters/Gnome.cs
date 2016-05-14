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
    public float GoldBagRadius;
    private Creep m_healCreep;
    private Vector3 m_initialGoldPosition;
    
    void Start()
    {
        m_healCreep = Helpers.GetObjectWithTag("HealCreep").GetComponent<Creep>();
    }

    public override void Attack()
    {
        GnomeClover clover = INetwork.Instance.Instantiate(
            CloverProjectilePrefab, 
            Camera.main.transform.position + Camera.main.transform.forward, 
            Quaternion.LookRotation(transform.forward)).GetComponent<GnomeClover>();
        INetwork.Instance.RPC(clover.gameObject, "SetOwnerViewId", PhotonTargets.All, INetwork.Instance.GetViewId(gameObject));
    }

    public override void UsePrimaryAbility()
    {
        GnomeGoldBag goldBag = INetwork.Instance.Instantiate(
            GoldBagPrefab,
            Camera.main.transform.position + Camera.main.transform.forward,
            Quaternion.LookRotation(transform.forward)).GetComponent<GnomeGoldBag>();
        INetwork.Instance.RPC(goldBag.gameObject, "SetOwnerViewId", PhotonTargets.All, INetwork.Instance.GetViewId(gameObject));
    }

    [PunRPC]
    public void CloverCollision(Vector3 position)
    {
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
        Debug.Log("ok");
    }

    [PunRPC]
    public void GoldBagCollision(Vector3 position)
    {
        m_healCreep.AddCreepAtPosition(position, (int)GoldBagRadius, 1);
        m_initialGoldPosition = position;
    }

    [PunRPC]
    public void GoldBagEnd(Vector3 position)
    {
        m_healCreep.AddCreepAtPosition(m_initialGoldPosition, (int)GoldBagRadius, 0);
    } 
}
