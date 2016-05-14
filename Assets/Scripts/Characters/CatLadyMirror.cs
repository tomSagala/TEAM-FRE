using UnityEngine;

public class CatLadyMirror : AbstractProjectile
{
    void Start()
    {
        GetComponent<Rigidbody>().velocity = speed * transform.forward;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (!INetwork.Instance.IsMaster())
            return;

        GameObject ownerObj = INetwork.Instance.GetGameObjectWithView(ownerViewId);
        if (ownerObj != null)
        {
            INetwork.Instance.RPC(ownerObj, "MirrorCollision", PhotonTargets.All, transform.position - Vector3.up * 0.2f);
        }
    }
}
