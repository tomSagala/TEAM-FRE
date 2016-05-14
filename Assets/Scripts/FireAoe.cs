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
        Destroy(gameObject, m_duration);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnTriggerStay(Collider other)
    {
        if (other.gameObject.GetComponent<Character>() != null)
        {
            other.gameObject.GetComponent<Character>().TakeDamage(m_dps * Time.fixedDeltaTime);
        }
    }
}
