﻿using UnityEngine;

public class Clover : MonoBehaviour
{
    [PunRPC]
    public void DestroyClover()
    {
        if (INetwork.Instance.IsMine(gameObject))
        {
            INetwork.Instance.NetworkDestroy(gameObject);
        }
    }
}