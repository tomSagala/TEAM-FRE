using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PowerStatusManager : MonoBehaviour
{
    public Image primaryAbilityImage;
    public Image primaryAbilityCooldown;
    public Text primaryAbilityCount;

    public Image secondaryAbilityImage;
    public Image secondaryAbilityCooldown;
    public Text secondaryAbilityCount;

    public Sprite disabledImage;
    public Sprite cooldownImage;

    private bool m_OnDisable;

    private bool m_OnPrimaryCooldown;
    private bool m_OnSecondaryCooldown;

    private Character m_character;

    void Update()
    {
        if (m_character != null && !m_character.CanUsePrimaryAbility() && !m_character.m_actionblocked)
        {
            m_OnPrimaryCooldown = true;

            primaryAbilityCount.text = Mathf.Ceil(m_character.m_primaryAbilityRemainingCoolDown).ToString();

            Vector3 currentScale = primaryAbilityCooldown.rectTransform.localScale;
            float newScale = m_character.m_primaryAbilityRemainingCoolDown / m_character.m_primaryAbilityCoolDown;
            primaryAbilityCooldown.rectTransform.localScale = new Vector3(newScale, newScale, currentScale.z);

        }
        else if (m_character.CanUsePrimaryAbility() && m_OnPrimaryCooldown)
        {
            m_OnPrimaryCooldown = false;
            primaryAbilityCount.text = "";

        }

        if (m_character != null && !m_character.CanUseSecondaryAbility() && !m_character.m_actionblocked)
        {
            m_OnSecondaryCooldown = true;

            secondaryAbilityCount.text = Mathf.Ceil(m_character.m_secondaryAbilityRemainingCoolDown).ToString();

            Vector3 currentScale = secondaryAbilityCooldown.rectTransform.localScale;
            float newScale = m_character.m_secondaryAbilityRemainingCoolDown / m_character.m_secondaryAbilityCoolDown;
            secondaryAbilityCooldown.rectTransform.localScale = new Vector3(newScale, newScale, currentScale.z);
        }
        else if (m_character.CanUseSecondaryAbility() && m_OnSecondaryCooldown)
        {
            m_OnSecondaryCooldown = false;
            secondaryAbilityCount.text = "";
        }

        if (m_character.m_actionblocked && !m_OnDisable)
        {
            primaryAbilityCooldown.sprite = disabledImage;
            primaryAbilityCooldown.rectTransform.localScale = new Vector3(1.0f, 1.0f, 0.0f);

            secondaryAbilityCooldown.sprite = disabledImage;
            secondaryAbilityCooldown.rectTransform.localScale = new Vector3(1.0f, 1.0f, 0.0f);

            m_OnDisable = true;
        }
        else if (!m_character.m_actionblocked && m_OnDisable)
        {
            primaryAbilityCooldown.sprite = cooldownImage;
            primaryAbilityCooldown.rectTransform.localScale = new Vector3(0.0f, 0.0f, 0.0f);

            secondaryAbilityCooldown.sprite = cooldownImage;
            secondaryAbilityCooldown.rectTransform.localScale = new Vector3(0.0f, 0.0f, 0.0f);

            m_OnDisable = false;
        }
    }

    public void SetCharacter(Character character)
    {
        m_character = character;
        SetAbilityImages();
    }

    private void SetAbilityImages()
    {
        primaryAbilityImage.sprite = m_character.primaryAbilitySprite;
        secondaryAbilityImage.sprite = m_character.secondaryAbilitySprite;
    }
}
