using UnityEngine;

public class CatLadyCat : AbstractProjectile
{
    public float Dps;
    public float DpsDuration;
    public float CatMeowThreshold = 100.0f;
    private bool m_grounded;
    private Vector3 m_groundedPosition;
    private Transform m_target;
    private int m_nbCloverEaten;

    void Start()
    {
        GetComponent<Rigidbody>().velocity = speed * transform.forward;
        m_nbCloverEaten = 0;
    }

    void Update()
    {
        if (!m_grounded)
            return;

        float value = Random.Range(0, 100000);
        if (value < CatMeowThreshold)
        {
            NetworkAudioManager.Instance.PlayAudioClipLocally("HappyCat", this.transform.position, 1.0f);
        }

        if (m_target == null)
        {
            if (INetwork.Instance.IsMaster())
            {
                RaycastHit[] hits = Physics.SphereCastAll(m_groundedPosition, 3f, Vector3.up);
                foreach (RaycastHit hit in hits)
                {
                    if (Helpers.CheckObjectTag(hit.collider.gameObject, "Clover"))
                    {
                        Clover clover = hit.collider.GetComponent<Clover>();
                        if (!clover.Targeted)
                        {
                            clover.Targeted = true;
                            INetwork.Instance.RPC(gameObject, "TargetClover", PhotonTargets.Others, INetwork.Instance.GetViewId(clover.gameObject));
                            m_target = clover.transform;
                            break;
                        }
                    }
                }
            }
        }
        else
        {
            Vector3 direction = (m_target.position - transform.position).normalized;
            transform.position += direction * Time.deltaTime;

            if (INetwork.Instance.IsMaster())
            {
                RaycastHit[] hits = Physics.SphereCastAll(transform.position, 0.5f, Vector3.up);
                foreach (RaycastHit hit in hits)
                {
                    if (hit.transform == m_target)
                    {
                        INetwork.Instance.RPC(m_target.gameObject, "DestroyClover", PhotonTargets.All);
                        m_target = null;

                        m_nbCloverEaten++;
                        if (m_nbCloverEaten >= 5)
                        {
                            INetwork.Instance.RPC(gameObject, "DestroyProjectile", PhotonTargets.All);
                        } 
                    }
                }
            }
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (!INetwork.Instance.IsMaster())
            return;

        if (m_grounded)
            return;
        
        if (collision.collider.name == "Ground")
        {
            m_grounded = true;
            m_groundedPosition = transform.position;
        }
        else if (collision.collider.GetComponent<Character>() != null)
        {
            Character character = collision.collider.GetComponent<Character>();
            INetwork.Instance.RPC(character.gameObject, "TakeDamageOverTime", PhotonTargets.All, Dps, DpsDuration);
            INetwork.Instance.RPC(gameObject, "Attach", PhotonTargets.All, INetwork.Instance.GetViewId(character.gameObject), collision.contacts[0].point);
            INetwork.Instance.RPC(gameObject, "DestroyProjectileAfterTime", PhotonTargets.All, DpsDuration);
            NetworkAudioManager.Instance.PlayAudioClipForAll("AggressiveCat", this.transform.position, 1.0f);
        }
    }

    [PunRPC]
    public void TargetClover(int viewId)
    {
        GameObject clover = INetwork.Instance.GetGameObjectWithView(viewId);
        m_target = clover.transform;
        m_grounded = true;
    }

    [PunRPC]
    public void Attach(int viewId, Vector3 position)
    {
        transform.position = position;
        GameObject gameObject = INetwork.Instance.GetGameObjectWithView(viewId);
        transform.parent = gameObject.transform;
        GetComponent<Rigidbody>().detectCollisions = false;
        GetComponent<Rigidbody>().useGravity = false;
        GetComponent<Rigidbody>().velocity = Vector3.zero;
        GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
    }
}