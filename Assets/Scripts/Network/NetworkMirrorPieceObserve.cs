using UnityEngine;

public class NetworkMirrorPieceObserve : MonoBehaviour
{
    private INetwork m_network;
    private bool m_hadFirstPosition;
    private Vector3 m_realPosition;
    private Quaternion m_realRotation;

    void Awake()
    {
        m_network = INetwork.Instance;
    }

    void Start()
    {
        m_hadFirstPosition = false;
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
            }
        }
    }
}