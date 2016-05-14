using UnityEngine;
using System.Collections;

public class Gnome : Character 
{
    public float CreepDuration;
    public float CreepRadius;
    public GameObject cloverPrefab;
    
    public override void Attack()
    {
        GnomeClover clover = INetwork.Instance.Instantiate(
            cloverPrefab, 
            Camera.main.transform.position + Camera.main.transform.forward, 
            Quaternion.LookRotation(transform.forward)).GetComponent<GnomeClover>();
        INetwork.Instance.RPC(clover.gameObject, "SetOwnerViewId", PhotonTargets.All, INetwork.Instance.GetViewId(gameObject));
    }

    [PunRPC]
    public void FireClover()
    {

    }

    [PunRPC]
    public void CloverCollision(Vector3 position)
    {
        StartCoroutine(CreepPosition(position));
    }

    private IEnumerator CreepPosition(Vector3 position)
    {
        float currentCreepTime = 0;

        while (currentCreepTime <= CreepDuration)
        {
            float ratio = currentCreepTime / CreepDuration;
            Creep.Instance.AddCreepAtPosition(position, (int)CreepRadius, ratio);
            currentCreepTime += Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }
    }
}
