using UnityEngine;

public abstract class AbstractProjectile : MonoBehaviour
{
    [HideInInspector]
    public int ownerViewId;
    public float speed;
    public float damage;

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
    public void SetOwnerViewId(int viewId)
    {
        ownerViewId = viewId;
    }
}
