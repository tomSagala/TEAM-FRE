using UnityEngine;
using System.Collections;

public class LifeBar : MonoBehaviour
{
    private Character m_character;

	void OnEnable ()
    {
        GenerateHitPoints(m_character.m_maxHealthPoints);
	}
	
	void Update ()
    {
	
	}

    private void GenerateHitPoints(float hp)
    {
        
    }

    public void SetCharacter(Character character)
    {
        m_character = character;
    }
    
}
