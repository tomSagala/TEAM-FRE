using UnityEngine;

public abstract class AbstractProjectile : MonoBehaviour
{
    [HideInInspector]
    public int ownerViewId;
    public float speed;
    public float damage;
    public string ownerTeam;

    [PunRPC]
    public void DestroyProjectile()
    {
        if (INetwork.Instance.IsMine(gameObject))
            INetwork.Instance.NetworkDestroy(gameObject);
    }

    [PunRPC]
    public void DestroyProjectileAfterTime(float time)
    {
        if (INetwork.Instance.IsMine(gameObject))
        {
            Timer.Instance.Request(time, () =>
            {
                INetwork.Instance.NetworkDestroy(gameObject);
            });
        }
    }

    [PunRPC]
    public void AddVelocity(Vector3 vel)
    {
        GetComponent<Rigidbody>().velocity += vel;
    }

    [PunRPC]
    public void SetOwnerViewId(int viewId)
    {
        ownerViewId = viewId;
    }

    [PunRPC]
    public void SetOwnerTeam(string owner)
    {
        ownerTeam = owner;
    }
    [PunRPC]
    public void SetDamage(float dmg)
    {
        damage = dmg;
    }
    [PunRPC]
    public void Setspeed(float spd)
    {
        speed = spd;
    }
}
