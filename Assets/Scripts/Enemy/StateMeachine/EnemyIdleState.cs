using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyIdleState : IEnemyState
{
    private EnemyFSM enemyFSM;
    private float idleTime;
    private Transform targetPlayer;
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
        FindPlayer();
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
            targetPlayer = null;
            return;
        }

        foreach (var collider in colliderPlayers)
        {
            targetPlayer = collider.transform;
            Vector3 playerDir = targetPlayer.position - enemyFSM.transform.position;
            playerDir.y = 0;
            float angle = Vector3.Angle(enemyFSM.transform.forward, playerDir);

            if (angle < enemyFSM.viewAngle / 2)
            {
                if (!Physics.Linecast(
                enemyFSM.transform.position + Vector3.up, // 射线起点（略高于地面）
                targetPlayer.position + Vector3.up, // 射线终点（玩家胸口位置）
                enemyFSM.obstacleMask))
                {
                    enemyFSM.TransitionState(StateType.Chase);
                    return;
                }
            }

        }
    }
}
