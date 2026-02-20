using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.ReorderableList.Element_Adder_Menu;
using UnityEditor.Timeline;
using UnityEngine;
using UnityEngine.AI;

public class EnemyPatrolState : IEnemyState
{
    private EnemyFSM enemyFSM;
    private NavMeshAgent agent;
    private CharacterBase currentCharacter;
    private float patrolTime;  //防止走不到一直走
    private Vector3 wayPoint;

    private Transform targetPlayer;

    public EnemyPatrolState(EnemyFSM enemyFSM)
    {
        this.enemyFSM = enemyFSM;
    }
    public void OnEnter()
    {
        agent = enemyFSM.M_agent;
        agent.isStopped = false;
        currentCharacter = enemyFSM.M_currentCharacter;
        GetNewWayPoint();



        agent.speed = currentCharacter.speed * 0.5f;

        agent.SetDestination(wayPoint);


        enemyFSM.animator.SetBool("IsWalk", true);

        patrolTime = 10f;
    }

    public void OnUpdate()
    {
        patrolTime -= Time.deltaTime;
        if (patrolTime <= 0f)
        {
            enemyFSM.TransitionState(StateType.Idle);
        }
        if (Vector3.Distance(wayPoint, enemyFSM.transform.position) <= agent.stoppingDistance)
        {
            enemyFSM.TransitionState(StateType.Idle);
        }

        FindPlayer();
    }
    public void OnExit()
    {
        agent.isStopped = true;
        enemyFSM.animator.SetBool("IsWalk", false);
    }

    void GetNewWayPoint()
    {
        float randomX = Random.Range(-enemyFSM.patrolRange, enemyFSM.patrolRange);
        float randomZ = Random.Range(-enemyFSM.patrolRange, enemyFSM.patrolRange);
        Vector3 randomPoint = new Vector3(enemyFSM.transform.position.x + randomX, enemyFSM.transform.position.y, enemyFSM.transform.position.z + randomZ);
        wayPoint = randomPoint;
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
