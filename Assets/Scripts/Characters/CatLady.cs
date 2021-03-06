﻿using UnityEngine;
using System.Collections;

public class CatLady : Character {

    public GameObject CatProjectilePrefab;
    public GameObject MirrorPrefab;
    public GameObject MirrorPiecePrefab;
    public float MirrorBurst;
    public float SpeedBoost;
    public float SpeedBoostDuration;

    public override void Attack()
    {
        if (m_reloading)
            return;

        if (m_currentAmmo <= 0)
        {
            m_reloading = true;
            StartCoroutine(ReloadCoroutine());
            return;
        }

        m_autoAttackAvailable = false;
        m_autoAttackRemainingCoolDown = 1.0f / m_autoAttackPerSeconds;

        CatLadyCat cat = INetwork.Instance.Instantiate(
            CatProjectilePrefab,
            Camera.main.transform.position + Camera.main.transform.forward,
            Quaternion.LookRotation(Camera.main.transform.forward)).GetComponent<CatLadyCat>();
        INetwork.Instance.RPC(cat.gameObject, "SetOwnerViewId", PhotonTargets.All, INetwork.Instance.GetViewId(gameObject));
        INetwork.Instance.RPC(cat.gameObject, "SetOwnerTeam", PhotonTargets.All, m_team);
        INetwork.Instance.RPC(cat.gameObject, "AddVelocity", PhotonTargets.All, GetComponent<Rigidbody>().velocity);

        m_currentAmmo--;
    }

    public override void UsePrimaryAbility()
    {
        m_primaryAbilityAvailable = false;
        m_primaryAbilityRemainingCoolDown = m_primaryAbilityCoolDown;

        var mirrorObj = INetwork.Instance.Instantiate(
            MirrorPrefab,
            Camera.main.transform.position + Camera.main.transform.forward,
            Quaternion.LookRotation(Camera.main.transform.forward));
        var mirror = mirrorObj.GetComponent<CatLadyMirror>();
        INetwork.Instance.RPC(mirror.gameObject, "SetOwnerViewId", PhotonTargets.All, INetwork.Instance.GetViewId(gameObject));
        INetwork.Instance.RPC(mirror.gameObject, "SetOwnerTeam", PhotonTargets.All, m_team);
        INetwork.Instance.RPC(mirror.gameObject, "AddVelocity", PhotonTargets.All, GetComponent<Rigidbody>().velocity);
    }

    public override void UseSecondaryAbility()
    {
        m_secondaryAbilityAvailable = false;
        m_secondaryAbilityRemainingCoolDown = m_secondaryAbilityCoolDown;

        GetComponent<RigidbodyFirstPersonController>().movementSettings.ForwardSpeed *= SpeedBoost;
        Timer.Instance.Request(SpeedBoostDuration, () =>
        {
            GetComponent<RigidbodyFirstPersonController>().movementSettings.ForwardSpeed /= SpeedBoost;
        });

        NetworkAudioManager.Instance.PlayAudioClipForAll("CatLadyCry", this.transform.position, 1.0f);
    }

    [PunRPC]
    public void MirrorCollision(Vector3 position)
    {
        if (!INetwork.Instance.IsMine(gameObject))
            return;

        for (int i = 0 ; i < 6; ++i)
        {
            GameObject obj = INetwork.Instance.Instantiate(MirrorPiecePrefab, position, Quaternion.Euler(new Vector3(0, UnityEngine.Random.value * 180, 0)));
            INetwork.Instance.RPC(obj, "SetOwnerTeam", PhotonTargets.All, m_team);
            float theta = UnityEngine.Random.value * 360;
            float dist = UnityEngine.Random.value * 0.5f;
            Vector3 offset = dist * new Vector3(Mathf.Cos(theta), 0, Mathf.Sin(theta)); 
            obj.GetComponent<Rigidbody>().velocity = MirrorBurst * (Vector3.up + offset);
        }
    }
}
