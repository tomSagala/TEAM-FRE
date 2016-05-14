using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LifeBar : MonoBehaviour
{
    public GameObject hitPrefab;
    public float hitInitialX;
    public float hitInitialY;
    public float hitDistance;

    private Character m_character;
    private ArrayList m_hits = new ArrayList();


	void OnEnable ()
    {
        GenerateHitPoints(m_character.m_maxHealthPoints);
	}
	
	void Update ()
    {
        int currentHp = (int) Mathf.Ceil(m_character.GetHealthPoints());
        for (int i = 0; i < m_character.m_maxHealthPoints; ++i)
        {
            ((Hit)m_hits[i]).SetIsDamage(i >= currentHp);
        }
	}

    private void GenerateHitPoints(uint hp)
    {
        for (uint i = 0; i < hp; ++i)
        {
            Vector3 pos = new Vector3(hitInitialX + i * hitDistance, hitInitialY, 0.0f);
            GameObject justCreated = Instantiate(hitPrefab) as GameObject;
            justCreated.transform.parent = this.transform;
            justCreated.GetComponent<Image>().rectTransform.SetParent(this.GetComponent<RectTransform>());
            justCreated.GetComponent<Image>().rectTransform.anchoredPosition = this.GetComponent<RectTransform>().anchoredPosition;
            justCreated.GetComponent<Image>().rectTransform.localPosition = pos;
            m_hits.Add(justCreated.GetComponent<Hit>());
        }
    }

    public void SetCharacter(Character character)
    {
        m_character = character;
    }
    
}
