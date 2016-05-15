using UnityEngine;
using System.Collections;

public class Pointer : MonoBehaviour
{
    public float position;
    public float size;

    private PlayState m_playState;

    void Awake()
    {
        m_playState = FindObjectOfType<PlayState>();
    }

	void Update ()
    {
        Vector3 currentPos = GetComponent<RectTransform>().localPosition;
        float newX = (size * BenTraductor(m_playState.LuckBadLuckRatio) * -0.5f);

        string currentMusic = "";
        if (m_playState.LuckBadLuckRatio < 0.2f && m_playState.LuckBadLuckRatio > -0.2f)
        {
            currentMusic = NetworkAudioManager.NEUTRAL;
        }
        else if (m_playState.LuckBadLuckRatio > 0.2f)
        {
            currentMusic = NetworkAudioManager.BADLUCK;
        }
        else if (m_playState.LuckBadLuckRatio < -0.2f)
        {
            currentMusic = NetworkAudioManager.LUCK;
        }

        if (!NetworkAudioManager.CURRENT.Equals(currentMusic))
        {
            NetworkAudioManager.Instance.ModifyCurrentSongForAll(currentMusic);
            NetworkAudioManager.CURRENT = currentMusic;
        }


        GetComponent<RectTransform>().localPosition = new Vector3(newX, currentPos.y, currentPos.z);
    }

    private float BenTraductor(float input)
    {
        return -1 * input;
    }
}
