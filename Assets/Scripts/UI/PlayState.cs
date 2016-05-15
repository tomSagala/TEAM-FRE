using UnityEngine;
using System.Collections;

public class PlayState : MonoBehaviour {
    public float MaxTimer;
    public float LuckBadLuckRatio;

    private float m_gameTimer;

	// Use this for initialization
	void Start () 
    {
        m_gameTimer = 0;
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (!INetwork.Instance.IsMaster())
            return;

        int nbClover = Transform.FindObjectsOfType<Clover>().Length;
        int nbCats = Transform.FindObjectsOfType<CatLadyCat>().Length;

        m_gameTimer += Time.deltaTime;

        if (m_gameTimer > MaxTimer)
        {

        }
	}
}
