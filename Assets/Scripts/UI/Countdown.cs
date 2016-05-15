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
    private Character m_character;
    private float initialDuration;

    public void Setup()
    {
        m_text = GetComponent<Text>();
        if (m_camera)
            m_camera.GetComponent<Blur>().enabled = blurs;

        if (m_character != null)
        {
            m_character.m_actionblocked = true;
            if(m_character.GetComponent<RigidbodyFirstPersonController>() != null)
                m_character.GetComponent<RigidbodyFirstPersonController>().enabled = false;
        }
        initialDuration = duration;
    }

    public void SetCharacter(Character player)
    {
        m_camera = player.GetComponentInChildren<Camera>();
        m_character = player;
    }
    
    void Update()
    {
        if (m_camera != null)
        {
            duration -= Time.deltaTime;
            m_text.text = Mathf.Ceil(duration).ToString();
            if (duration < 0)
            {
                m_camera.GetComponent<Blur>().enabled = false;
                m_character.m_actionblocked = false;
                m_character.GetComponent<RigidbodyFirstPersonController>().enabled = true;
                duration = initialDuration;
                StateManager.Instance.GoToState("Play");
            }
        }
    }
}
