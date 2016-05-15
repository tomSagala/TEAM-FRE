using UnityEngine;

public class Clover : MonoBehaviour
{
    public bool Targeted;

    [PunRPC]
    public void DestroyClover()
    {
        if (INetwork.Instance.IsMine(gameObject))
        {
            INetwork.Instance.NetworkDestroy(gameObject);
        }
    }
}