﻿using UnityEngine;
using System.Collections;

public class RabbitFoot : AbstractProjectile {

    void Start()
    {
        GetComponent<Rigidbody>().velocity = speed * transform.forward;
    }
    void OnCollisionEnter(Collision collision)
    {
        if (!INetwork.Instance.IsMaster())
            return;

        if (collision.collider.name == "Ground")
        {
            INetwork.Instance.RPC(gameObject, "DestroyProjectile", PhotonTargets.All);
        }
        else if (collision.collider.GetComponent<Character>() != null && collision.collider.GetComponent<Character>().GetTeam() != ownerTeam)
        {
            NetworkAudioManager.Instance.PlayAudioClipForAll("RabbitExplosion", this.transform.position, 1.0f);

            Character character = collision.collider.GetComponent<Character>();
            INetwork.Instance.RPC(character.gameObject, "TakeDamage", PhotonTargets.All, damage);
            INetwork.Instance.RPC(gameObject, "DestroyProjectile", PhotonTargets.All);
        }
    }
}
