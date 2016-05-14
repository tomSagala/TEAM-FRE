using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Countdown : MonoBehaviour
{
    public float duration = 300.0f;

    private Text m_text;
 
     void Awake()
    {
        m_text = GetComponent<Text>();
    }
    
     void Update()
    {
        duration -= Time.deltaTime;
        //m_text.text = Mathf.Abs(duration);
        if (duration < 0)
        {
            //Application.LoadLevel("gameOver");
        }
    }
}
