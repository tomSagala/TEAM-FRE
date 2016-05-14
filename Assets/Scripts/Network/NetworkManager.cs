using ExitGames.Client.Photon;
using UnityEngine;
using System;

public class NetworkManager : PunGameSingleton<NetworkManager>
{
    private bool m_isSinglePlayer;

    public bool IsSinglePlayer
    {
        get
        {
            return m_isSinglePlayer;
        }
        set
        {
            m_isSinglePlayer = value;
            PhotonNetwork.offlineMode = m_isSinglePlayer;
        }
    }

    public void Connect()
    {
        PhotonNetwork.ConnectUsingSettings("1.0");
    }

    public void Disconnect()
    {
        PhotonNetwork.Disconnect();
    }

    public void JoinRandomRoom()
    {
        PhotonNetwork.JoinRandomRoom();
    }

    public void SetRoomStarted()
    {
        PhotonNetwork.room.open = false;
    }

    public void SetTeam(string team)
    {
        PhotonNetwork.player.SetCustomProperties(new Hashtable() { { "Team", team } });
    }

    public string GetPlayerTeam()
    {
        return PhotonNetwork.player.customProperties["Team"] == null ? null : PhotonNetwork.player.customProperties["Team"].ToString();
    }

    public string GetOtherPlayerTeam(PhotonPlayer otherPlayer)
    {
        return otherPlayer.customProperties["Team"] == null ?
            null :
            otherPlayer.customProperties["Team"].ToString();
    }

    public void SetPlayerName(string name)
    {
        PhotonNetwork.player.name = name;
    }

    public int GetViewId(GameObject gameObject)
    {
        PhotonView pv = gameObject.GetComponent<PhotonView>();
        if (pv == null)
            return 0;
        return pv.viewID;
    }

    public GameObject GetGameObjectWithView(int viewId)
    {
        PhotonView pv = PhotonView.Find(viewId);
        if (pv == null)
            return null;
        return pv.gameObject;
    }

    public string GetPlayerName()
    {
        return PhotonNetwork.player.name;
    }

    public void RPC(GameObject gameObject, string methodName, PhotonTargets target, params object[] parameters)
    {
        PhotonView pv = gameObject.GetComponent<PhotonView>();
        if (pv == null)
        {
            throw new UnityException("GameObject has no PhotonView. Cannot make RPC Call");
        }
        pv.RPC(methodName, target, parameters);
    }

    public GameObject Instantiate(GameObject prefab, Vector3 position, Quaternion rotation)
    {
        if (IsSinglePlayer)
            return GameObject.Instantiate(prefab, position, rotation) as GameObject;
        return PhotonNetwork.Instantiate(prefab.name, position, rotation, 0);
    }

    public void NetworkDestroy(GameObject gameObject)
    {
        if (IsSinglePlayer)
        {
            GameObject.Destroy(gameObject);
        }
        else
        {
            PhotonNetwork.Destroy(gameObject);
        }
    }

    public void LoadLevel(string sceneName)
    {
        PhotonNetwork.LoadLevel(sceneName);
    }

    public void SetAutomaticallySyncScene(bool automaticallySyncScene)
    {
        PhotonNetwork.automaticallySyncScene = automaticallySyncScene;
    }

    public bool IsConnected
    {
        get { return PhotonNetwork.connected; }
    }

    public int GetPlayerCount()
    {
        return PhotonNetwork.playerList.Length;
    }

    void OnPhotonRandomJoinFailed()
    {
        RoomOptions options = new RoomOptions();
        options.maxPlayers = 2;

        PhotonNetwork.CreateRoom(Guid.NewGuid().ToString(), options, null);
    }
}