using UnityEngine;
using UnityEngine.Events;

public class CharacterBase : MonoBehaviour
{

    [Header("基础属性")]
    public float baseHealth;
    public float maxHealth;
    public float currentHealth;
    public float baseAttack;
    public float currentAttack;
    public float baseCritChance = 0.1f;
    public float currentCritChance;
    public float baseDefence;
    public float currentDefence;
    public float speed;

    [Header("角色状态")]
    public bool isDead;
    public bool isInvulnerable;
    public float invulnerableTime = 0f;
    public ForceLevel poise = ForceLevel.Basy;  //韧性（抗打击力）

    [Header("广播事件")]
    public UnityEvent OnHealthChangeEvent;
    public UnityEvent OnDeathEvent;

    private Animator animator;

    protected virtual void Awake()
    {
        animator = GetComponent<Animator>();
        currentHealth = maxHealth;
        currentAttack = baseAttack;
        currentCritChance = baseCritChance;
        currentDefence = baseDefence;
    }

    protected virtual void Start()
    {

    }

    private void Update()
    {
        CheckState();
    }

    private void CheckState()
    {
        if (currentHealth <= 0.01f && !isDead)
        {
            animator.SetBool("IsDead", true);
            isDead = true;
            OnDeathEvent?.Invoke();

        }
        if (invulnerableTime > 0f)
        {
            isInvulnerable = true;
            invulnerableTime -= Time.deltaTime;
        }
        else
        {
            isInvulnerable = false;
        }
    }

    /// <summary>
    /// 老的伤害计算
    /// </summary>
    /// <param name="comboInteractionConfig"></param>
    /// <returns></returns>
    public Damage TryGetDamage(ComboInteractionConfig comboInteractionConfig)
    {
        bool isCrit = Random.value < currentCritChance;
        float damage = currentAttack * comboInteractionConfig.damageMul * (isCrit ? 1.5f : 1f);

        return new Damage(damage,isCrit);
    }
    /// <summary>
    /// 新的伤害计算
    /// </summary>
    /// <param name="combatInteractionConfig"></param>
    /// <returns></returns>
    public Damage TryGetDamage(CombatInteractionConfig combatInteractionConfig)
    {
        bool isCrit = Random.value < currentCritChance;
        float damage = currentAttack * combatInteractionConfig.damageMul * (isCrit ? 1.5f : 1f);

        return new Damage(damage,isCrit);
    }

    public void BeHit(Damage newDamage)
    {
        OnBeHit(newDamage);
    }

    protected float finalDamage;
    protected virtual void OnBeHit(Damage newDamage)
    {
        finalDamage = Mathf.Max(1f, newDamage.damage - currentDefence);
        currentHealth -= finalDamage;
        if (currentHealth < 0) currentHealth = 0;
        OnHealthChangeEvent?.Invoke();
    }

}

public class Damage
{
    public Damage(float damage,bool isCrit)
    {
        this.damage = damage;
        this.isCrit = isCrit;
    }
    public float damage;
    public bool isCrit;
}