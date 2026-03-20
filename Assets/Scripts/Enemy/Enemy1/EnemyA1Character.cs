using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyA1Character : CharacterBase
{
    private EnemyHealthBar enemyHealthBar;

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
        enemyHealthBar.UpdateHealthBar();

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
