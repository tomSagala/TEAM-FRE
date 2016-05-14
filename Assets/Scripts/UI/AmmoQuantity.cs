using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class AmmoQuantity : MonoBehaviour
{
    // Reference to the player
    private Text m_text;

    void Awake()
    {
        m_text = GetComponent<Text>();
    }

    void Update()
    {
        // m_text.text = player...
    }

    public void SetPlayer()
    {

    }
}
