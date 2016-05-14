using UnityEngine;

public class GnomeGoldBag : AbstractProjectile
{
    private bool m_grounded = false;
    private bool m_deleted = false;
    public float duration;
    private float m_timer;

    void Start()
    {
        GetComponent<Rigidbody>().velocity = speed * (transform.forward + transform.up).normalized;
        m_grounded = false;
        m_timer = 0;
    }

    void Update()
    {
        if (m_deleted)
            return;

        if (m_grounded)
        {
            GetComponent<Rigidbody>().velocity = Vector3.zero;
            m_timer += Time.deltaTime;
            if (m_timer > duration)
            {
                INetwork.Instance.RPC(gameObject, "DestroyProjectile", PhotonTargets.All);
                GameObject ownerObj = INetwork.Instance.GetGameObjectWithView(ownerViewId);
                if (ownerObj != null)
                {
                    INetwork.Instance.RPC(ownerObj, "GoldBagEnd", PhotonTargets.All, transform.position);
                }
            }
        }
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
                INetwork.Instance.RPC(ownerObj, "GoldBagCollision", PhotonTargets.All, transform.position - Vector3.up*0.2f); 
            }

            INetwork.Instance.RPC(gameObject, "Stop", PhotonTargets.All);
        }
    }

    [PunRPC]
    public void Stop()
    {
        m_grounded = true;
    }
}