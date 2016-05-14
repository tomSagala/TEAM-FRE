using UnityEngine;

public class GnomeClover : MonoBehaviour
{
    public float speed;
    public float damage;
    [HideInInspector] public int ownerViewId;

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

            INetwork.Instance.RPC(gameObject, "DestroyClover", PhotonTargets.All);
        }
        else if (collision.collider.GetComponent<Character>() != null)
        {
            Character character = collision.collider.GetComponent<Character>();
            INetwork.Instance.RPC(character.gameObject, "TakeDamage", PhotonTargets.All, damage);
            INetwork.Instance.RPC(gameObject, "DestroyClover", PhotonTargets.All);
        }
    }

    [PunRPC]
    public void DestroyClover()
    {
        if (INetwork.Instance.IsMine(gameObject))
            INetwork.Instance.NetworkDestroy(gameObject);
    }

    [PunRPC]
    public void SetOwnerViewId(int viewId)
    {
        ownerViewId = viewId;
    }
}