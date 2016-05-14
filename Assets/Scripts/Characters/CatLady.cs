using UnityEngine;
using System.Collections;

public class CatLady : Character {

    public GameObject CatProjectilePrefab;
    public GameObject MirrorPrefab;

    public override void Attack()
    {
        CatLadyCat cat = INetwork.Instance.Instantiate(
            CatProjectilePrefab,
            Camera.main.transform.position + Camera.main.transform.forward,
            Quaternion.LookRotation(transform.forward)).GetComponent<CatLadyCat>();
        INetwork.Instance.RPC(cat.gameObject, "SetOwnerViewId", PhotonTargets.All, INetwork.Instance.GetViewId(gameObject));
    }

    public override void UsePrimaryAbility()
    {
        var mirrorObj = INetwork.Instance.Instantiate(
            MirrorPrefab,
            Camera.main.transform.position + Camera.main.transform.forward,
            Quaternion.LookRotation(transform.forward));
        var mirror = mirrorObj.GetComponent<CatLadyMirror>();
        INetwork.Instance.RPC(mirror.gameObject, "SetOwnerViewId", PhotonTargets.All, INetwork.Instance.GetViewId(gameObject));
    } 
}
