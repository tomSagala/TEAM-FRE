using UnityEngine;

public class GnomeHealingQuad : MonoBehaviour
{
    public float RotationSpeed;
    private float m_angle;
    private float m_healRate;

    void Start()
    {
        m_angle = 0;
        m_healRate = GetComponentInParent<Gnome>().GoldBagHealRate;
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
        INetwork.Instance.RPC(character.gameObject, "TakeDamage", PhotonTargets.All, GetComponentInParent<Gnome>().GoldBagHealRate * Time.deltaTime);
    }
}