using System.Collections;
using System.Collections.Generic;
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
        Debug.Log("死亡");
        enemyFSM.animator.SetBool("IsDead", true);
        destoryTime = 3f;
    }
    public void OnUpdate()
    {
        if (enemyFSM == null) return;
        destoryTime -= Time.deltaTime;
        if (destoryTime <= 0f)
        {
            GameObject.Destroy(enemyFSM.gameObject);
            enemyFSM = null;
        }
    }
    public void OnExit()
    {
        if (enemyFSM == null) return;
        enemyFSM.animator.SetBool("IsDead", false);
    }
}
