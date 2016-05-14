using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityStandardAssets.ImageEffects;

public class Countdown : MonoBehaviour
{
    public bool blurs;
    public float duration;
    private Text m_text;
    private Camera m_camera;

     void Awake()
    {
        m_text = GetComponent<Text>();
        m_camera = FindObjectOfType<Camera>();
        m_camera.GetComponent<Blur>().enabled = blurs;
    }
    
     void Update()
    {
        duration -= Time.deltaTime;
        m_text.text = Mathf.Ceil(duration).ToString();
        if (duration < 0)
        {
            m_camera.GetComponent<Blur>().enabled = false;
            StateManager.Instance.GoToState("Play");
        }
    }
}
