using UnityEngine;
using System.Collections;

public class FireBug : Projectile {
    [SerializeField] public float m_maxAmplitudeX = 1.5f;
    [SerializeField] public float m_minAmplitudeX = 0.5f;
    [SerializeField] public float m_maxAmplitudeY = 1.1f;
    [SerializeField] public float m_minAmplitudeY = 0.9f;
    [SerializeField] float m_frequency = 10;
    [SerializeField] GameObject m_explosionPrefab;
    private Vector3 straightPos;
    private Vector3 axisRight;
    private Vector3 axisForward;
    private Vector3 axisUp;
    private float m_amplitudeX;
    private float m_amplitudeY;
    private float timer = 0f;
    // Use this for initialization
    void Start ()
    {
        GetComponent<Rigidbody>().useGravity = false;
        straightPos = transform.position;
        axisRight = transform.right;
        axisForward = transform.forward;
        axisUp = transform.up;
    }

    [PunRPC]
    public void Setup(float timerOffset, float ampX, float ampY)
    {
        timer = timerOffset;
        m_amplitudeX = ampX;
        m_amplitudeY = ampY;
    }
	
	// Update is called once per frame
	void FixedUpdate ()
    {
        timer += Time.fixedDeltaTime;
        straightPos += axisForward * speed * Time.fixedDeltaTime;
        transform.position = straightPos + axisRight * m_amplitudeX * Mathf.Sin((timer) * m_frequency ) + axisUp * m_amplitudeY * -1f * Mathf.Abs(Mathf.Cos((timer) * m_frequency));
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (!INetwork.Instance.IsMaster())
            return;

        if (collision.collider.name == "Ground")
        {
            INetwork.Instance.RPC(gameObject, "Explode", PhotonTargets.All);
        }
        else if (collision.collider.GetComponent<Character>() != null && collision.collider.GetComponent<Character>().GetTeam() != ownerTeam)
        {
            Character character = collision.collider.GetComponent<Character>();
            INetwork.Instance.RPC(character.gameObject, "TakeDamage", PhotonTargets.All, damage);
            INetwork.Instance.RPC(gameObject, "Explode", PhotonTargets.All);
        }
    }
    [PunRPC]
    public void Explode()
    {
        INetwork.Instance.Instantiate(
            m_explosionPrefab,
            transform.position, Quaternion.identity);
        INetwork.Instance.RPC(gameObject, "DestroyProjectile", PhotonTargets.All);
    }
}
