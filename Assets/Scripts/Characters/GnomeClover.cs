using UnityEngine;

public class GnomeClover : AbstractProjectile
{
    void Start()
    {
        GetComponent<Rigidbody>().velocity = speed * transform.forward;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (!INetwork.Instance.IsMaster())
            return;

        if (collision.collider.name == "Ground")
        {
            GameObject ownerObj = INetwork.Instance.GetGameObjectWithView(ownerViewId);
            if (ownerObj != null)
            {
                INetwork.Instance.RPC(ownerObj, "CloverCollision", PhotonTargets.All, transform.position); 
            }

            INetwork.Instance.RPC(gameObject, "DestroyProjectile", PhotonTargets.All);
        }
        else if (collision.collider.GetComponent<Character>() != null)
        {
            Character character = collision.collider.GetComponent<Character>();
            INetwork.Instance.RPC(character.gameObject, "TakeDamage", PhotonTargets.All, damage);
            INetwork.Instance.RPC(gameObject, "DestroyProjectile", PhotonTargets.All);
        }
    }
}