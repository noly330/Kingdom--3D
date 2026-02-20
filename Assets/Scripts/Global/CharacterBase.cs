using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterBase : MonoBehaviour
{

    [Header("基础属性")]
    public float maxHealth;
    public float currentHealth;
    public float baseAttack;
    public float currentAttack;
    public float defence;
    public float speed;

    [Header("角色状态")]
    public bool isDead;

    protected virtual void Awake()
    {

    }

    protected virtual void Start()
    {
        currentHealth = maxHealth;
        currentAttack = baseAttack;
        
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
    }

}
