using UnityEngine;

public class CatLadyMirrorPiece : MonoBehaviour
{
    public float Damage;

    void OnTriggerEnter(Collider collider)
    {
        Character character = collider.GetComponent<Character>();
        if (character != null)
        {
            INetwork.Instance.RPC(character.gameObject, "TakeDamage", PhotonTargets.All, Damage);
        }
    }
}