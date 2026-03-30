using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyDeadState : IEnemyState 
{
    private EnemyFSM enemyFSM;

    private float destoryTime;
    public EnemyDeadState(EnemyFSM enemyFSM)
    {
        this.enemyFSM = enemyFSM;
    }
    public void OnEnter()
    {
        //死亡动画和isDead都在角色基类实现了
        destoryTime = 3f;
    }
    public void OnUpdate()
    {
        if (enemyFSM == null) return;
        destoryTime -= Time.deltaTime;
        if (destoryTime <= 0f)
        {
            enemyFSM.GetComponent<LootSpawner>().CreatLootItem();
            ObjectPool.instance.ReturnPool(enemyFSM.currentCharacter.characterPoolType, enemyFSM.gameObject);

        }
    }
    public void OnExit()
    {
        if (enemyFSM == null) return;
        enemyFSM.currentCharacter.isDead = false;
        enemyFSM.animator.SetBool("IsDead", false);
    }
}
