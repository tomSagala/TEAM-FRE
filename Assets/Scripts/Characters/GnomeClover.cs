using UnityEngine;

public class GnomeClover : MonoBehaviour
{
    public float speed;
    public Gnome Owner;

    void Start()
    {
        GetComponent<Rigidbody>().velocity = speed * transform.forward;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.name == "Ground")
        {
            Owner.CloverCollision(transform.position);
            Destroy(gameObject);
        }
    }
}