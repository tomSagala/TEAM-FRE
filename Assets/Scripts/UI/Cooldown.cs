using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Cooldown : MonoBehaviour
{
    public Image fillCircle;
    public Text count;
    public float cooldownTime;
    public float m_duration;
    public bool m_cooldownActive;

    void Update()
    {
        if (m_cooldownActive)
        {
            m_duration -= Time.deltaTime;
            count.text = Mathf.Ceil(m_duration).ToString();

            Vector3 currentScale = fillCircle.rectTransform.localScale;
            float newScale = 1 - m_duration / cooldownTime;
            fillCircle.rectTransform.localScale = new Vector3(newScale, newScale, currentScale.z);

            if (m_duration < 0)
            {
                EndCooldown();
            }
        }
    }

    public void StartCooldown()
    {
        m_cooldownActive = true;
    }

    public void EndCooldown()
    {
        m_duration = cooldownTime;
        m_cooldownActive = false;
    }
}
