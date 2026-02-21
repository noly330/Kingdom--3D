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
        enemyHealthBar.UpdateHealthBar();
    }

    void OnEnable()
    {

    }
    void OnDisable()
    {

    }

}
