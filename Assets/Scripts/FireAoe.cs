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
        Timer.Instance.Request(m_duration, ()=> 
        { 
            if (gameObject && INetwork.Instance.IsMine(gameObject))
                INetwork.Instance.NetworkDestroy(gameObject); 
        });
        NetworkAudioManager.Instance.PlayAudioClipForAll("FireAoe", this.transform.position, 1.0f);
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
        }
    }
}
