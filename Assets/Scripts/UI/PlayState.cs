using UnityEngine;
using System.Collections;

public class PlayState : MonoBehaviour 
{
    public float MaxTimer;
    [HideInInspector] public float LuckBadLuckRatio;
    [HideInInspector] public float GameTimer;

    private int m_nbLuckDeaths;
    private int m_nbBadLuckDeaths;
    private bool m_won;

	// Use this for initialization
	void Start () 
    {
        GameTimer = 0;
        m_won = false;
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (!INetwork.Instance.IsMaster() || m_won)
            return;

        int nbClover = Transform.FindObjectsOfType<Clover>().Length;
        int nbCats = Transform.FindObjectsOfType<CatLadyCat>().Length;

        LuckBadLuckRatio = (nbCats * 24 + m_nbLuckDeaths * 100 - (nbClover + m_nbBadLuckDeaths * 100)) / 500f;
        if (LuckBadLuckRatio > 1)
            LuckBadLuckRatio = 1;
        else if (LuckBadLuckRatio < -1)
            LuckBadLuckRatio = -1;

        GameTimer += Time.deltaTime;
        INetwork.Instance.RPC(gameObject, "NotifyGameState", PhotonTargets.All, LuckBadLuckRatio, GameTimer);

        if (GameTimer > MaxTimer || LuckBadLuckRatio == 1 || LuckBadLuckRatio == -1)
        {
            m_won = true;
            if (LuckBadLuckRatio > 0)
            {
                INetwork.Instance.RPC(gameObject, "NotifyWinner", PhotonTargets.All, TeamsEnum.BadLuckTeam);
            }
            else
            {
                INetwork.Instance.RPC(gameObject, "NotifyWinner", PhotonTargets.All, TeamsEnum.GoodLuckTeam);
            }
        }
	}

    public void AddLuckDeath()
    {
        m_nbLuckDeaths++;
    }

    public void AddBadLuckDeath()
    {
        m_nbBadLuckDeaths++;
    }

    [PunRPC]
    public void NotifyGameState(float luckBadLuckRatio, float gameTimer)
    {
        LuckBadLuckRatio = luckBadLuckRatio;
        GameTimer = gameTimer;
    }

    [PunRPC]
    public void NotifyWinner(string team)
    {
        Debug.Log(team + " won");
    }
}
