using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class CharacterBase : MonoBehaviour
{

    [Header("基础属性")]
    public float baseHealth;
    public float maxHealth;
    public float currentHealth;
    public float baseAttack;
    public float currentAttack;
    public float defence;
    public float speed;

    [Header("角色状态")]
    public bool isDead;
    public bool isInvulnerable;
    public float invulnerableTime = 0f;
    
    public UnityEvent OnHealthChangeEvent;

    private Animator animator;

    protected virtual void Awake()
    {
        animator = GetComponent<Animator>();
    }

    protected virtual void Start()
    {
        currentHealth = maxHealth;
        currentAttack = baseAttack;
        
    }

    private void Update()
    {
        CheckState();
    }

    private void CheckState()
    {
        if(invulnerableTime > 0.1f)
        {
            isInvulnerable = true;
            invulnerableTime -= Time.deltaTime;
        }
        else
        {
            isInvulnerable = false;
        }
    }
    public float TryGetDamage(ComboInteractionConfig comboInteractionConfig)
    {
        float damage = currentAttack * comboInteractionConfig.damageMul;

        return damage;
    }

    public void OnBeHit(float damage)
    {
        float finalDamage = Mathf.Max(1f,damage - defence);
        currentHealth -= finalDamage;
        if(currentHealth<0)  currentHealth = 0;
        OnHealthChangeEvent?.Invoke();
    }

}
