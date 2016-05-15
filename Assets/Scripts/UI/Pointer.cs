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
        GetComponent<RectTransform>().localPosition = new Vector3(newX, currentPos.y, currentPos.z);
    }

    private float BenTraductor(float input)
    {
        return -1 * input;
    }
}
