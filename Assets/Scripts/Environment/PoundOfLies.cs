using UnityEngine;
using System.Collections;

public class PoundOfLies : MonoBehaviour 
{
	void OnTriggerEnter(Collider collider)
    {
        CatLadyCat cat = collider.GetComponent<CatLadyCat>();
        if (cat != null)
        {
            INetwork.Instance.RPC(cat.gameObject, "DestroyProjectile", PhotonTargets.All);
        }
        Knight knight = collider.GetComponent<Knight>();
        if (knight != null)
        {
            INetwork.Instance.RPC(knight.gameObject, "TakeDamage", PhotonTargets.All, 100f);
        }
    }
}
