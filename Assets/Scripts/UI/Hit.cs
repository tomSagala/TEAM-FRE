using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Hit : MonoBehaviour
{
    public Sprite imageHealth;
    public Sprite imageDamage;

    private Image m_image;

    void Awake()
    {
        m_image = GetComponent<Image>();
        m_image.sprite = imageHealth;
    }

    public void SetIsDamage(bool isDamage)
    {
        m_image.sprite = isDamage ? imageDamage : imageHealth;
    }
}
