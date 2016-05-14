using UnityEngine;

public class NetworkPlayerObserve : MonoBehaviour
{
    private INetwork m_network;
    private bool m_hadFirstPosition;
    private Vector3 m_realPosition;
    private Quaternion m_realRotation;

    //private Animator m_animator;
    //private int m_speedHash;

    void Awake()
    {
        m_network = INetwork.Instance;
        //m_animator = GetComponent<Animator>();
    }

    void Start()
    {
        m_hadFirstPosition = false;
        //m_speedHash = Animator.StringToHash("speed");
    }

    void Update()
    {
        if (m_network.IsMine(gameObject))
        {

        }
        else
        {
            transform.position = Vector3.Lerp(transform.position, m_realPosition, 6 * Time.deltaTime);
            transform.rotation = Quaternion.Lerp(transform.rotation, m_realRotation, 6 * Time.deltaTime);
        }
    }

    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
            //stream.SendNext(m_animator.GetFloat(m_speedHash));
        }
        else
        {
            if (!m_hadFirstPosition)
            {
                transform.position = (Vector3)stream.ReceiveNext();
                transform.rotation = (Quaternion)stream.ReceiveNext();
                m_realPosition = transform.position;
                m_realRotation = transform.rotation;
                m_hadFirstPosition = true;
            }
            else
            {
                m_realPosition = (Vector3)stream.ReceiveNext();
                m_realRotation = (Quaternion)stream.ReceiveNext();
                //m_animator.SetFloat(m_speedHash, (float)stream.ReceiveNext());
            }
        }
    }
}