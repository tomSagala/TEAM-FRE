using UnityEngine;
using System.Collections;

public class Pointer : MonoBehaviour
{
    public float position;
    public float size;

	void Update ()
    {
        Vector3 currentPos = GetComponent<RectTransform>().localPosition;
        float newX = (size * position * -0.5f);
        GetComponent<RectTransform>().localPosition = new Vector3(newX, currentPos.y, currentPos.z);
    }
}
