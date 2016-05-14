using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ResultsState : MonoBehaviour
{
    public Image luckImage;
    public Image badLuckImage;

    private bool m_winningTeamIsLuck;
    public void SetWinningTeam(bool luckWins)
    {
        m_winningTeamIsLuck = luckWins;
    }

    void Update()
    {
        luckImage.enabled = m_winningTeamIsLuck;
        badLuckImage.enabled = !m_winningTeamIsLuck;
    }
}
