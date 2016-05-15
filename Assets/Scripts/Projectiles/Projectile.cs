using UnityEngine;
using System.Collections;

public class Projectile : AbstractProjectile
{
    [SerializeField] bool m_isOneHitKill = false;

    void Start()
    {
        GetComponent<Rigidbody>().velocity = speed * transform.forward;
        GetComponent<Rigidbody>().useGravity = false;
    }
    public void SetOneHitKill(bool isOneHitKill)
    {
        m_isOneHitKill = isOneHitKill;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (!INetwork.Instance.IsMaster())
            return;

        if (collision.collider.name == "Ground")
        {
            INetwork.Instance.RPC(gameObject, "DestroyProjectile", PhotonTargets.All);
        }
        else if (collision.collider.GetComponent<Character>() != null && collision.collider.GetComponent<Character>().GetTeam() != ownerTeam)
        {
            Character character = collision.collider.GetComponent<Character>();
            if (m_isOneHitKill)
            {
                INetwork.Instance.RPC(character.gameObject, "Die", PhotonTargets.All);
            }
            else
            {
                INetwork.Instance.RPC(character.gameObject, "TakeDamage", PhotonTargets.All, damage);
            }
            INetwork.Instance.RPC(gameObject, "DestroyProjectile", PhotonTargets.All);
        }
    }

}
