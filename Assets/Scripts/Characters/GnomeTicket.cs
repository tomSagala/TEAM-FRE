using System;
using UnityEngine;

public class GnomeTicket : AbstractProjectile
{
    private bool m_grounded = false;
    private bool m_deleted = false;
    public float duration;
    private float m_timer;
    private Vector3 m_direction;
    private float m_upTime;
    private bool m_dropping;

    private Vector3 m_straightPos;
    private Vector3 m_axisRight;
    private Vector3 m_axisDown;
    private Vector3 m_axisUp;
    private float m_amplitudeX;
    private float m_amplitudeY;
    private float m_droppingTimer = 0f;
    private float m_frequency = 2;

    void Start()
    {
        m_grounded = false;
        m_timer = 0;
        m_direction = (transform.forward + transform.up).normalized;

        m_droppingTimer = UnityEngine.Random.Range(0f, 2 * Mathf.PI);
        m_amplitudeX = 1;
        m_amplitudeY = 0.5f;
        m_axisRight = transform.right;
        m_axisDown = -transform.up;
        m_axisUp = transform.up;
    }

    void Update()
    {
        if (m_deleted)
            return;

        if (m_grounded)
        {
            m_timer += Time.deltaTime;
            if (m_timer > duration)
            {
                INetwork.Instance.RPC(gameObject, "DestroyProjectile", PhotonTargets.All);
                GameObject ownerObj = INetwork.Instance.GetGameObjectWithView(ownerViewId);
                if (ownerObj != null)
                {
                    INetwork.Instance.RPC(ownerObj, "TicketEnd", PhotonTargets.All, transform.position);
                }
            }
        }
        else if (m_dropping)
        {
            m_droppingTimer += Time.fixedDeltaTime;
            m_straightPos += m_axisDown * speed * 0.2f * Time.fixedDeltaTime;

            transform.position = m_straightPos +
                m_axisRight * m_amplitudeX * Mathf.Sin((m_droppingTimer) * m_frequency) +
                m_axisUp * m_amplitudeY * -1f * Mathf.Abs(Mathf.Cos((m_droppingTimer) * m_frequency));

            RaycastHit[] hits = Physics.SphereCastAll(transform.position, 0.5f, Vector3.up);
            foreach (RaycastHit hit in hits)
            {
                if (hit.collider.name.Equals("Ground"))
                {
                    GameObject ownerObj = INetwork.Instance.GetGameObjectWithView(ownerViewId);
                    if (ownerObj != null)
                    {
                        INetwork.Instance.RPC(ownerObj, "TicketCollision", PhotonTargets.All, transform.position - Vector3.up * 0.2f);
                    }

                    INetwork.Instance.RPC(gameObject, "Stop", PhotonTargets.All);
                    m_grounded = true;
                    break;
                }
            }
        }
        else
        {
            transform.position += speed * m_direction * Time.deltaTime;
            m_upTime += Time.deltaTime;
            if (m_upTime > 2)
            {
                m_straightPos = transform.position;
                m_dropping = true;
            }
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (!INetwork.Instance.IsMaster())
            return;

        if (collision.collider.name == "Ground")
        {
            
        }
    }

    [PunRPC]
    public void Stop()
    {
        m_grounded = true;
    }
}