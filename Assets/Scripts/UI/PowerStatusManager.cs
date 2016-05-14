using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PowerStatusManager : MonoBehaviour
{
    public Image primaryAbilityFill;
    public Text primaryAbilityCount;
    public Image secondaryAbilityFill;
    public Text secondaryAbilityCount;

    private bool m_OnPrimaryDisable;
    private bool m_OnSecondaryDisable;

    private bool m_OnPrimaryCooldown;
    private bool m_OnSecondaryCooldown;

    private Character m_character;

    void Update()
    {
        if (m_character != null && !m_character.CanUsePrimaryAbility())
        {
            m_OnPrimaryCooldown = true;

            primaryAbilityCount.text = Mathf.Ceil(m_character.m_primaryAbilityRemainingCoolDown).ToString();

            Vector3 currentScale = primaryAbilityFill.rectTransform.localScale;
            float newScale = 1 - m_character.m_primaryAbilityRemainingCoolDown / m_character.m_primaryAbilityCoolDown;
            primaryAbilityFill.rectTransform.localScale = new Vector3(newScale, newScale, currentScale.z);

        }
        else if (m_character.CanUsePrimaryAbility() && m_OnPrimaryCooldown)
        {
            m_OnPrimaryCooldown = false;
            primaryAbilityCount.text = "";

        }

        if (m_character != null && !m_character.CanUseSecondaryAbility())
        {
            m_OnSecondaryCooldown = true;

            secondaryAbilityCount.text = Mathf.Ceil(m_character.m_secondaryAbilityRemainingCoolDown).ToString();

            Vector3 currentScale = secondaryAbilityFill.rectTransform.localScale;
            float newScale = 1 - m_character.m_secondaryAbilityRemainingCoolDown / m_character.m_secondaryAbilityCoolDown;
            secondaryAbilityFill.rectTransform.localScale = new Vector3(newScale, newScale, currentScale.z);
        }
        else if (m_character.CanUseSecondaryAbility() && m_OnSecondaryCooldown)
        {
            m_OnSecondaryCooldown = false;
            secondaryAbilityCount.text = "";
        }
    }


    public void SetCharacter(Character character)
    {
        m_character = character;
    }

}
