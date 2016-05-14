using UnityEngine;

public class GnomeTicketSphere : MonoBehaviour
{
    [HideInInspector] public float Duration;

    void OnTriggerEnter(Collider collider)
    {
        Character character = collider.GetComponent<Character>();
        if (character == null)
            return;

        INetwork.Instance.RPC(character.gameObject, "Stun", PhotonTargets.All, Duration);
    }
}