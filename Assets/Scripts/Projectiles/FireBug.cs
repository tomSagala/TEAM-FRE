using UnityEngine;
using System.Collections;

public class FireBug : Projectile {
    [SerializeField] float m_maxAmplitudeX = 1.5f;
    [SerializeField] float m_minAmplitudeX = 0.5f;
    [SerializeField] float m_maxAmplitudeY = 1.1f;
    [SerializeField] float m_minAmplitudeY = 0.9f;
    [SerializeField] float m_travelSpeed = 25f;
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
        
	}

    public void Setup()
    {
        timer = Random.Range(0f, 2 * Mathf.PI);
        m_amplitudeX = Random.Range(m_minAmplitudeX, m_maxAmplitudeX);
        m_amplitudeY = Random.Range(m_minAmplitudeY, m_maxAmplitudeY);
        straightPos = transform.position;
        axisRight = transform.right;
        axisForward = transform.forward;
        axisUp = transform.up;
    }
	
	// Update is called once per frame
	void FixedUpdate ()
    {
        timer += Time.fixedDeltaTime;
        straightPos += axisForward * m_travelSpeed * Time.fixedDeltaTime;
        transform.position = straightPos + axisRight * m_amplitudeX * Mathf.Sin((timer) * m_frequency ) + axisUp * m_amplitudeY * -1f * Mathf.Abs(Mathf.Cos((timer) * m_frequency));
    }

    public void SetSpeed(float speed)
    {
        m_travelSpeed = speed;
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<Character>() != null && collision.gameObject.GetComponent<Character>().GetTeam() != m_firedBy)
        {
            collision.gameObject.GetComponent<Character>().TakeDamage(m_damage);
        }
        else if (collision.gameObject.GetComponent<Character>() != null && collision.gameObject.GetComponent<Character>().GetTeam() == m_firedBy)
        {
            return;
        }
        Explode();
    }
    void Explode()
    {
        Instantiate(m_explosionPrefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
