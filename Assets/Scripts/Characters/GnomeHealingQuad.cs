using UnityEngine;

public class GnomeHealingQuad : MonoBehaviour
{
    public float RotationSpeed;
    private float m_angle;
    [HideInInspector] public float HealRate;

    void Awake()
    {
        m_angle = 0;
    }

    void Update()
    {
        m_angle += RotationSpeed * Time.deltaTime;
        if (m_angle > 360)
            m_angle -= 360;
        transform.rotation = Quaternion.Euler(0, m_angle, 0); 
    }

    void OnTriggerStay(Collider collider)
    {
        Character character = collider.GetComponent<Character>();
        if (character != null)
        {
            INetwork.Instance.RPC(character.gameObject, "TakeDamage", PhotonTargets.All, -1.0f * HealRate * Time.deltaTime);
        }
    }
}