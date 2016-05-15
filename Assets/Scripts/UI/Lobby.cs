using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class Lobby : MonoBehaviour
{
    public string gameSceneName;
    public AudioClip readySound;

    private List<Text> m_PlayerTexts;
    private List<Image> m_PlayerCharacterImages;
    private List<Button> m_CharacterButtons;
    private Button m_readyButton;
    private int m_nbPlayerReady;

    void Start()
    {
        INetwork network = INetwork.Instance;
        if (!network.IsConnected)
        {
            SceneManager.LoadScene("MainMenu");
            return;
        }

        m_CharacterButtons = new List<Button>();
        m_CharacterButtons.Add(transform.Find("CharacterSelection").Find("HeroButton1").GetComponent<Button>());
        m_CharacterButtons.Add(transform.Find("CharacterSelection").Find("HeroButton2").GetComponent<Button>());
        m_CharacterButtons.Add(transform.Find("CharacterSelection").Find("HeroButton3").GetComponent<Button>());
        m_CharacterButtons.Add(transform.Find("CharacterSelection").Find("HeroButton4").GetComponent<Button>());

        int nbPlayers = network.GetPlayerCount() - 1;
        int nbPlayersBottom = network.GetPlayerCountInTeam(TeamsEnum.BadLuckTeam);
        if (nbPlayersBottom <= (nbPlayers - nbPlayersBottom))
        {
            network.SetTeam(TeamsEnum.BadLuckTeam);
            m_CharacterButtons[2].interactable = false;
            m_CharacterButtons[3].interactable = false;
        }
        else
        {
            network.SetTeam(TeamsEnum.GoodLuckTeam);
            m_CharacterButtons[0].interactable = false;
            m_CharacterButtons[1].interactable = false;
        }
        network.SetId(nbPlayers);

        m_PlayerTexts = new List<Text>();
        string luck = "Luck";
        string badLuck = "BadLuck";
        string playerThumbnail1 = "PlayerThumbnail1";
        string playerThumbnail2 = "PlayerThumbnail2";
        string text = "Text";
        string image = "Image";
        m_PlayerTexts.Add(transform.Find(luck).Find(playerThumbnail1).Find(text).GetComponent<Text>());
        m_PlayerTexts.Add(transform.Find(badLuck).Find(playerThumbnail1).Find(text).GetComponent<Text>());
        m_PlayerTexts.Add(transform.Find(luck).Find(playerThumbnail2).Find(text).GetComponent<Text>());
        m_PlayerTexts.Add(transform.Find(badLuck).Find(playerThumbnail2).Find(text).GetComponent<Text>());

        m_PlayerCharacterImages = new List<Image>();
        m_PlayerCharacterImages.Add(transform.Find(luck).Find(playerThumbnail1).Find(image).GetComponent<Image>());
        m_PlayerCharacterImages.Add(transform.Find(badLuck).Find(playerThumbnail1).Find(image).GetComponent<Image>());
        m_PlayerCharacterImages.Add(transform.Find(luck).Find(playerThumbnail2).Find(image).GetComponent<Image>());
        m_PlayerCharacterImages.Add(transform.Find(badLuck).Find(playerThumbnail2).Find(image).GetComponent<Image>());

        m_PlayerTexts[nbPlayers].text = network.GetPlayerName();
        network.RPC(gameObject, "NotifyNewPlayer", PhotonTargets.OthersBuffered, network.GetPlayerName(), nbPlayers);

        m_readyButton = transform.Find("Ready").Find("ReadyButton").GetComponent<Button>();
        m_readyButton.interactable = false;
    }

    public void ReadyButton()
    {
        AudioSource.PlayClipAtPoint(readySound, Vector3.zero, 0.7f);
        
        m_readyButton.interactable = false;
        m_readyButton.transform.Find("Text").GetComponent<Text>().text = "Waiting...";
        INetwork.Instance.RPC(gameObject, "NotifyReady", PhotonTargets.MasterClient);
    }

    public void ChooseCatLady()
    {
        SendChooseCharacterRPC(0);
    }

    public void ChooseGentleman()
    {
        SendChooseCharacterRPC(1);
    }

    public void ChooseLeprechaun()
    {
        SendChooseCharacterRPC(2);
    }

    public void ChooseKnight()
    {
        SendChooseCharacterRPC(3);
    }

    private void SendChooseCharacterRPC(int characterId)
    {
        INetwork.Instance.SetCharacterId(characterId);
        INetwork.Instance.RPC(gameObject, "ChooseCharacter", PhotonTargets.AllBuffered, characterId, INetwork.Instance.GetId());
        m_readyButton.interactable = true;
    }

    [PunRPC]
    public void ChooseCharacter(int characterId, int playerId)
    {
        m_PlayerCharacterImages[playerId].sprite = m_CharacterButtons[characterId].image.sprite;
    }

    [PunRPC]
    public void NotifyNewPlayer(string username, int id)
    {
        m_PlayerTexts[id].text = username;
    }

    [PunRPC]
    public void NotifyReady()
    {
        m_nbPlayerReady++;
        if (m_nbPlayerReady == INetwork.Instance.GetPlayerCount())
        {
            StartCoroutine(StartGame());
        }
    }

    private IEnumerator StartGame()
    {
        yield return new WaitForSeconds(2.5f);

        INetwork.Instance.SetRoomStarted();
        INetwork.Instance.LoadLevel(gameSceneName);
    }
}
