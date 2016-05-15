using UnityEngine;
using System.Collections;

public abstract class Character : MonoBehaviour
{
    [SerializeField] public Sprite primaryAbilitySprite;
    [SerializeField] public Sprite secondaryAbilitySprite;
    [SerializeField] public uint m_maxHealthPoints = 5;
    [SerializeField] public float m_healthPoints = 5f;
    [SerializeField] protected string m_team;
    [SerializeField] protected float m_autoAttackDamage = 1f;
    [SerializeField] public float m_primaryAbilityCoolDown;
    [HideInInspector] public float m_primaryAbilityRemainingCoolDown;
    [SerializeField] public float m_secondaryAbilityCoolDown;
    [HideInInspector] public float m_secondaryAbilityRemainingCoolDown;
    [SerializeField] public float m_autoAttackPerSeconds = 1f;
    [HideInInspector] public float m_autoAttackRemainingCoolDown;
    [SerializeField] public float m_reloadDuration = 1f;
    [SerializeField] public int m_maxAmmo;
    [SerializeField] public int m_currentAmmo;
    [SerializeField] public bool m_isMelee;

    private float m_damageOverTimeTakenDPS = 0f;
    private float m_damageOverTimeTakenRemainingTime = 0f;

    protected bool m_autoAttackAvailable = true;
    protected bool m_primaryAbilityAvailable = true;
    protected bool m_secondaryAbilityAvailable = true;
    public bool m_actionblocked = false;
    private Transform m_spawnPoint;
    protected Coroutine reloadCouroutine;

	// Update is called once per frame
	protected void FixedUpdate () {
        if (m_damageOverTimeTakenRemainingTime > 0f)
        {
            TakeDamage(m_damageOverTimeTakenDPS * Time.fixedDeltaTime);
            m_damageOverTimeTakenRemainingTime -= Time.fixedDeltaTime;
        }

        if (!m_autoAttackAvailable)
        {
            m_autoAttackRemainingCoolDown -= Time.fixedDeltaTime;
            if (m_autoAttackRemainingCoolDown <= 0f)
            {
                AutoAttackReady();
            }
        }

        if (!m_primaryAbilityAvailable)
        {
            m_primaryAbilityRemainingCoolDown -= Time.fixedDeltaTime;
            if (m_primaryAbilityRemainingCoolDown <= 0f)
            {
                PrimaryReady();
            }
        }

        if (!m_secondaryAbilityAvailable)
        {
            m_secondaryAbilityRemainingCoolDown -= Time.fixedDeltaTime;
            if (m_secondaryAbilityRemainingCoolDown <= 0f)
            {
                secondaryReady();
            }
        }
    }

    public void SetSpawnPoint(Transform sp)
    {
        m_spawnPoint = sp;
    }

    public void SetTeam(string team)
    {
        m_team = team;
    }

    public string GetTeam()
    {
        return m_team;
    }

    public void SetHealthPoints(float hp)
    {
        m_healthPoints = hp;
    }

    public float GetHealthPoints()
    {
        return m_healthPoints;
    }

    [PunRPC]
    public virtual void TakeDamage(float damage)
    {
        if (reloadCouroutine != null)
        {
            StopCoroutine(reloadCouroutine);
        }

        m_healthPoints -= damage;

        if (m_healthPoints > m_maxHealthPoints)
            m_healthPoints = m_maxHealthPoints;

        if (INetwork.Instance.IsMaster() && m_healthPoints <= 0f)
        {
            // DIE
            if (m_team == TeamsEnum.BadLuckTeam)
            {
                FindObjectOfType<PlayState>().AddBadLuckDeath();
            }
            else if (m_team == TeamsEnum.GoodLuckTeam)
            {
                FindObjectOfType<PlayState>().AddLuckDeath();
            }

            INetwork.Instance.RPC(gameObject, "Die", PhotonTargets.All);
        }
    }

    [PunRPC]
    public virtual void TakeDamageOverTime(float dps, float duration)
    {
        m_damageOverTimeTakenDPS = dps;
        m_damageOverTimeTakenRemainingTime = duration;
    }

    public bool CanUseAutoAttack() { return m_autoAttackAvailable && !m_actionblocked && reloadCouroutine == null; }
    public bool CanUsePrimaryAbility() { return m_primaryAbilityAvailable && !m_actionblocked && reloadCouroutine == null; }
    public bool CanUseSecondaryAbility() { return m_secondaryAbilityAvailable && !m_actionblocked && reloadCouroutine == null; }

    [PunRPC]
    public virtual void Die() 
    {
        if (!INetwork.Instance.IsMine(gameObject))
            return;
        this.transform.position = m_spawnPoint.position;
        this.transform.rotation = m_spawnPoint.rotation;

        CatLadyCat[] cats = this.transform.GetComponentsInChildren<CatLadyCat>();

        foreach (CatLadyCat cat in cats)
        {
            INetwork.Instance.RPC(cat.gameObject, "DestroyProjectile", PhotonTargets.All);
        }
        
        StateManager.Instance.GoToState("Respawn");
        m_healthPoints = m_maxHealthPoints;
        m_damageOverTimeTakenRemainingTime = 0f;
    }

    public virtual void Attack() { }
    public virtual void UsePrimaryAbility() { }
    public virtual void UseSecondaryAbility() { }

    public virtual void AutoAttackReady() { m_autoAttackAvailable = true; }
    public virtual void PrimaryReady() { m_primaryAbilityAvailable = true; }
    public virtual void secondaryReady() { m_secondaryAbilityAvailable = true; }

    public virtual void Reload()
    {
        if (reloadCouroutine != null)
        {
            reloadCouroutine = StartCoroutine(ReloadCoroutine());
        }
    }

    public virtual IEnumerator ReloadCoroutine()
    {
        yield return new WaitForSeconds(m_reloadDuration);
        m_currentAmmo = m_maxAmmo;
    }

    [PunRPC]
    public virtual void Stun(float time)
    {
        m_actionblocked = true;
        GetComponent<RigidbodyFirstPersonController>().enabled = false;
        Timer.Instance.Request(time, () =>
        {
            Debug.Log("unstun");
            m_actionblocked = false;
            GetComponent<RigidbodyFirstPersonController>().enabled = true;
        });
    }
}
