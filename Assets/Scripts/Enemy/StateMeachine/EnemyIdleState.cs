using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyIdleState : IEnemyState
{
    private EnemyFSM enemyFSM;
    private float idleTime;
    private Transform targetCharacter;

    private float searchTimer = 0f;
    private float searchInterval = 0.3f; // 每秒检测3-4次
    public EnemyIdleState(EnemyFSM fSM)
    {
        this.enemyFSM = fSM;
    }
    public void OnEnter()
    {
        idleTime = 6f;
    }


    public void OnUpdate()
    {
        idleTime -= Time.deltaTime;
        if (idleTime <= 0f)
        {
            enemyFSM.TransitionState(StateType.Patrol);
        }

        // 优化搜索玩家，减少检测次数
        searchTimer -= Time.deltaTime;
        if (searchTimer <= 0f)
        {
            searchTimer = searchInterval;
            FindPlayer();
        }
    }
    public void OnExit()
    {
        idleTime = 0f;
    }


    void FindPlayer()
    {
        Collider[] colliderPlayers = Physics.OverlapSphere(enemyFSM.transform.position, enemyFSM.viewDistance, enemyFSM.targetMask);
        if (colliderPlayers.Length == 0)
        {
            targetCharacter = null;
            return;
        }

        foreach (var collider in colliderPlayers)
        {
            targetCharacter = collider.transform;
            Vector3 playerDir = targetCharacter.position - enemyFSM.transform.position;
            playerDir.y = 0;
            float angle = Vector3.Angle(enemyFSM.transform.forward, playerDir);

            if (angle < enemyFSM.viewAngle / 2)
            {
                if (!Physics.Linecast(
                enemyFSM.transform.position + Vector3.up, // 射线起点（略高于地面）
                targetCharacter.position + Vector3.up, // 射线终点（玩家胸口位置）
                enemyFSM.obstacleMask))
                {
                    enemyFSM.TransitionState(StateType.Chase);
                    return;
                }
            }

        }
    }
}
