using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class ResultsState : MonoBehaviour
{
    public Image background;
    public Sprite winSprite;
    public Sprite lostSprite;
    public static string winningTeam;

    private float m_time;

    void Update()
    {
        if (winningTeam == null)
            return;

        m_time += Time.deltaTime;

        int id = INetwork.Instance.GetId();

        if (id == 0 || id == 2) 
        {
            background.sprite = winningTeam.Equals(TeamsEnum.BadLuckTeam) ? winSprite : lostSprite;
        }
        else
        {
            background.sprite = winningTeam.Equals(TeamsEnum.GoodLuckTeam) ? winSprite : lostSprite;
        }

        if (m_time > 2 && Input.anyKeyDown)
        {
            EndGame();
        }
    }

    public void EndGame()
    {
        winningTeam = null;
        INetwork.Instance.Disconnect();
        SceneManager.LoadScene("MainMenu");
        Cursor.visible = true;
    }
}
