using UnityEngine;
using System.Collections;

public class Pointer : MonoBehaviour
{
    public float position;
    public float size;
    private float m_initialX;
    private RectTransform m_meterSize;

    void Start()
    {
        m_meterSize = GetComponentInParent<RectTransform>();
        m_initialX = GetComponent<RectTransform>().position.x;
    }

	void Update ()
    {
        Vector3 currentPos = GetComponent<RectTransform>().position;
        float newX = (size * position * -0.5f) + m_initialX;
        GetComponent<RectTransform>().position = new Vector3(newX, currentPos.y, currentPos.z);
    }
}
