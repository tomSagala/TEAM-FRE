using UnityEngine;

public class GnomeTicketSphere : MonoBehaviour
{
    [HideInInspector] public float Duration;

    void Start()
    {
        NetworkAudioManager.Instance.PlayAudioClipLocally("HealingQuad", this.transform.position, 1.0f);
    }

    void OnTriggerEnter(Collider collider)
    {
        Character character = collider.GetComponent<Character>();
        if (character == null || character.GetTeam() == TeamsEnum.BadLuckTeam)
            return;

        INetwork.Instance.RPC(character.gameObject, "Stun", PhotonTargets.All, Duration);
    }
}