﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityStandardAssets.ImageEffects;

public class Countdown : MonoBehaviour
{
    public bool blurs;
    public float duration;
    private Text m_text;
    private Camera m_camera;

    void OnEnable()
    {
        m_text = GetComponent<Text>();
        m_camera.GetComponent<Blur>().enabled = blurs;
    }

    public void SetBlurCamera(Camera camera)
    {
        m_camera = camera;
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
