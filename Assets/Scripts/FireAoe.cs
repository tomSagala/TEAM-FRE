using UnityEngine;
using System.Collections;

public class FireAoe : MonoBehaviour
{
    [SerializeField]
    float m_duration = 3f;
    [SerializeField]
    float m_dps = 1f;
    // Use this for initialization
    void Start()
    {
        Timer.Instance.Request(m_duration, ()=> { INetwork.Instance.NetworkDestroy(gameObject); });
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnTriggerStay(Collider other)
    {
        if (other.GetComponent<Character>() != null)
        {
            Character character = other.GetComponent<Character>();
            INetwork.Instance.RPC(character.gameObject, "TakeDamage", PhotonTargets.All, m_dps * Time.fixedDeltaTime);
            NetworkAudioManager.Instance.PlayAudioClipForAll("Fireball", this.transform.position, 0.25f);
        }
    }
}
