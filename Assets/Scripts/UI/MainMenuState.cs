using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class MainMenuState : MonoBehaviour
{	
	void Update ()
    {
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
        SceneManager.LoadScene("Lobby");
    }

    public void CreditsButton()
    {
        StateManager.Instance.GoToState("Credits");
    }

    public void InstructionsButton()
    {
        StateManager.Instance.GoToState("Instructions");
    }
}
