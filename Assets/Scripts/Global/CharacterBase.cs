using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class CharacterBase : MonoBehaviour
{

    [Header("基础属性")]
    public ObjectPoolType characterPoolType;  
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
    public float maxEnergy;
    public float currentEnergy;

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
    }

    protected virtual void Start()
    {
        currentHealth = maxHealth;
        currentAttack = baseAttack;
        currentEnergy = maxEnergy;
        currentCritChance = baseCritChance;
        currentDefence = baseDefence;

    }

    private void Update()
    {
        CheckState();
        if (currentEnergy < maxEnergy)
        {
            currentEnergy += Time.deltaTime * 5f;
        }
        else
            currentEnergy = maxEnergy;
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
    public Damage TryGetDamage(ComboInteractionConfig comboInteractionConfig)
    {
        bool isCrit = Random.value < currentCritChance;
        float damage = currentAttack * comboInteractionConfig.damageMul * (isCrit ? 1.5f : 1f);

        return new Damage()
        {
            damage = damage,
            isCrit = isCrit
        };
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
    public float damage;
    public bool isCrit;
}