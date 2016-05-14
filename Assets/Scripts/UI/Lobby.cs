using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class Lobby : MonoBehaviour
{
    private List<Text> m_PlayerTexts;

    void Start()
    {
        INetwork network = INetwork.Instance;
        if (!network.IsConnected)
        {
            SceneManager.LoadScene("MainMenu");
            return;
        }

        int nbPlayers = network.GetPlayerCount() - 1;
        int nbPlayersBottom = network.GetPlayerCountInTeam(TeamsEnum.BadLuckTeam);

        if (nbPlayersBottom <= (nbPlayers - nbPlayersBottom))
            network.SetTeam(TeamsEnum.BadLuckTeam);
        else
            network.SetTeam(TeamsEnum.GoodLuckTeam);
        network.SetId(nbPlayers);

        m_PlayerTexts = new List<Text>();
        m_PlayerTexts.Add(transform.Find("Luck").Find("PlayerThumbnail1").Find("Text").GetComponent<Text>());
        m_PlayerTexts.Add(transform.Find("BadLuck").Find("PlayerThumbnail1").Find("Text").GetComponent<Text>());
        m_PlayerTexts.Add(transform.Find("Luck").Find("PlayerThumbnail2").Find("Text").GetComponent<Text>());
        m_PlayerTexts.Add(transform.Find("BadLuck").Find("PlayerThumbnail2").Find("Text").GetComponent<Text>());

        m_PlayerTexts[nbPlayers].text = network.GetPlayerName();
        network.RPC(gameObject, "NotifyNewPlayer", PhotonTargets.Others, network.GetPlayerName(), nbPlayers);
    }

    public void ReadyButton()
    {
        // Implement Ready button
    }

    public void ChooseCatLady()
    {

    }

    public void ChooseGentleman()
    {

    }

    public void ChooseLeprechaun()
    {

    }

    public void ChooseKnight()
    {

    }

    [PunRPC]
    public void NotifyNewPlayer(string username, int id)
    {
        m_PlayerTexts[id].text = username;
    }

    [PunRPC]
    public void NotifyAlreadyLoggedPlayer(string username, int id)
    {

    }
}
