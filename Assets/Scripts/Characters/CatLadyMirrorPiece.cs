using UnityEngine;

public class CatLadyMirrorPiece : MonoBehaviour
{
    public float Damage;
    public string teamOwner;
    void OnTriggerEnter(Collider collider)
    {
        Character character = collider.GetComponent<Character>();
        if (character != null && character.GetComponent<Character>().GetTeam() != teamOwner)
        {
            INetwork.Instance.RPC(character.gameObject, "TakeDamage", PhotonTargets.All, Damage);
        }
    }

    void SetOwnerTeam(string team)
    {
        teamOwner = team;
    }
}