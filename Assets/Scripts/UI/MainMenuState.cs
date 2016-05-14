using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using Photon;
using UnityEngine.UI;

public class MainMenuState : PunBehaviour
{
    private Text m_usernameTextbox;
    private Button m_playButton;
    private bool m_connecting;

    void Awake()
    {
        INetwork.Instance.SetAutomaticallySyncScene(true);
        m_usernameTextbox = transform.Find("UsernameTextbox").Find("Text").GetComponent<Text>();
        m_playButton = transform.Find("PlayButton").GetComponent<Button>();
    }

    void Start()
    {
        m_connecting = false;
    }

	void Update ()
    {
        m_playButton.interactable = !m_connecting && m_usernameTextbox.text.Length != 0; 
        if (Input.GetKey("escape"))
        {
            QuitButton();
        }
	}

    public void QuitButton()
    {
        Application.Quit();
        Debug.Log("Quit Button pushed!");
    }

    public void PlayButton()
    {
        INetwork.Instance.Connect();
        m_connecting = true;
    }

    public void CreditsButton()
    {
        StateManager.Instance.GoToState("Credits");
    }

    public void InstructionsButton()
    {
        StateManager.Instance.GoToState("Instructions");
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("Photon : Joined lobby");
        INetwork.Instance.SetPlayerName(m_usernameTextbox.text);
        INetwork.Instance.JoinRandomRoom();
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Photon : Joined Room");
        if (INetwork.Instance.IsSinglePlayer)
            SceneManager.LoadScene("SinglePlayerLobby");
        else
            INetwork.Instance.LoadLevel("Lobby");
    }
}
