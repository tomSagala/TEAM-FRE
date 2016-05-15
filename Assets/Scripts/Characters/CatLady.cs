using UnityEngine;
using System.Collections;

public class CatLady : Character {

    public GameObject CatProjectilePrefab;
    public GameObject MirrorPrefab;
    public GameObject MirrorPiecePrefab;
    public float MirrorBurst;

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
        m_secondaryAbilityAvailable = false;
        m_secondaryAbilityRemainingCoolDown = m_secondaryAbilityCoolDown;

        var mirrorObj = INetwork.Instance.Instantiate(
            MirrorPrefab,
            Camera.main.transform.position + Camera.main.transform.forward,
            Quaternion.LookRotation(transform.forward));
        var mirror = mirrorObj.GetComponent<CatLadyMirror>();
        INetwork.Instance.RPC(mirror.gameObject, "SetOwnerViewId", PhotonTargets.All, INetwork.Instance.GetViewId(gameObject));
    }

    public override void UseSecondaryAbility()
    {
        m_primaryAbilityAvailable = false;
        m_primaryAbilityRemainingCoolDown = m_primaryAbilityCoolDown;

        // CIRCULEZ RIEN ICI POUR L'INSTANT :D
    }

    [PunRPC]
    public void MirrorCollision(Vector3 position)
    {
        if (!INetwork.Instance.IsMine(gameObject))
            return;

        for (int i = 0 ; i < 6; ++i)
        {
            GameObject obj = INetwork.Instance.Instantiate(MirrorPiecePrefab, position, Quaternion.Euler(new Vector3(0, UnityEngine.Random.value * 180, 0)));
            float theta = UnityEngine.Random.value * 360;
            float dist = UnityEngine.Random.value * 0.5f;
            Vector3 offset = dist * new Vector3(Mathf.Cos(theta), 0, Mathf.Sin(theta)); 
            obj.GetComponent<Rigidbody>().velocity = MirrorBurst * (Vector3.up + offset);
        }
    }
}
