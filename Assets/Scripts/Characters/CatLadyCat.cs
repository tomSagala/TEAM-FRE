using UnityEngine;

public class CatLadyCat : AbstractProjectile
{
    public float Dps;
    public float DpsDuration;
    private bool m_grounded;

    void Start()
    {
        GetComponent<Rigidbody>().velocity = speed * transform.forward;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (m_grounded)
            return;

        if (!INetwork.Instance.IsMaster())
            return;

        if (collision.collider.name == "Ground")
        {
            m_grounded = true;
        }
        else if (collision.collider.GetComponent<Character>() != null)
        {
            Character character = collision.collider.GetComponent<Character>();
            INetwork.Instance.RPC(character.gameObject, "TakeDamageOverTime", PhotonTargets.All, Dps, DpsDuration);
            INetwork.Instance.RPC(gameObject, "Attach", PhotonTargets.All, INetwork.Instance.GetViewId(character.gameObject), collision.contacts[0].point);
            INetwork.Instance.RPC(gameObject, "DestroyProjectileAfterTime", PhotonTargets.All, DpsDuration);
        }
    }

    [PunRPC]
    public void Attach(int viewId, Vector3 position)
    {
        transform.position = position;
        GameObject gameObject = INetwork.Instance.GetGameObjectWithView(viewId);
        transform.parent = gameObject.transform;
        GetComponent<Rigidbody>().detectCollisions = false;
        GetComponent<Rigidbody>().useGravity = false;
        GetComponent<Rigidbody>().velocity = Vector3.zero;
        GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
    }
}