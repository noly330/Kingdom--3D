using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCharacter : CharacterBase
{
    public EnemyHealthBar enemyHealthBar;
    public ObjectPoolType characterPoolType;  //在行为树里面用

    protected override void Awake()
    {
        base.Awake();
        enemyHealthBar = GetComponent<EnemyHealthBar>();
    }
    protected override void Start()
    {
        base.Start();
    }

    void OnEnable()
    {
        currentHealth = maxHealth;
        isDead = false;
        if (enemyHealthBar == null)
        {
            Debug.LogError("EnemyA1Character: enemyHealthBar is null");

        }
        else
        {
            enemyHealthBar.UpdateHealthBar();
        }

    }
    void OnDisable()
    {

    }

    protected override void OnBeHit(Damage newDamage)
    {
        base.OnBeHit(newDamage);
        GameObject obj = ObjectPool.instance.SpawnFromPool(ObjectPoolType.DamageText, transform.position - 1.2f * transform.forward + Vector3.up * 1f, transform.rotation);
        if (obj != null)
        {
            obj.GetComponent<DamageText>().SetDamageText(finalDamage, newDamage.isCrit);
        }
    }
}
