using UnityEngine;
using System;
using System.Collections;

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

        StartCoroutine(Shatter());
    }

    private IEnumerator Shatter()
    {
        yield return new WaitForSeconds(0.5f);
        GameObject ownerObj = INetwork.Instance.GetGameObjectWithView(ownerViewId); 
        if (ownerObj != null)
        {
            INetwork.Instance.RPC(ownerObj, "MirrorCollision", PhotonTargets.All, transform.position);
        }
        INetwork.Instance.RPC(gameObject, "DestroyProjectile", PhotonTargets.All); 
    }
}
