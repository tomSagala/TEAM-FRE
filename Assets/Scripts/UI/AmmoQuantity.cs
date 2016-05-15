using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class AmmoQuantity : MonoBehaviour
{
    private Character m_character;
    private Text m_text;

    void Awake()
    {
        m_text = GetComponent<Text>();
    }

    void Update()
    {
        m_text.text = m_character.m_currentAmmo + "/" + m_character.m_maxAmmo;
    }

    public void SetCharacter(Character character)
    {
        m_character = character;
    }
}
