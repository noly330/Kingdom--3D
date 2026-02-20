using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyChaseState : IEnemyState
{
    private EnemyFSM enemyFSM;
    private Transform targetPlayer;
    private CharacterBase currentCharacter;
    private NavMeshAgent agent;
    public EnemyChaseState(EnemyFSM fSM)
    {
        this.enemyFSM = fSM;
    }
    public void OnEnter()
    {
        agent = enemyFSM.M_agent;
        agent.isStopped = false;
        currentCharacter = enemyFSM.M_currentCharacter;

        agent.speed = currentCharacter.speed * 1f;
        enemyFSM.animator.SetBool("IsChase", true);
    }
    public void OnUpdate()
    {
        FindPlayer();
        ChasePlayer();
        FightWithPlayer();
    }

    public void OnExit()
    {
        agent.isStopped = true;
        enemyFSM.animator.SetBool("IsChase", false);
    }

    void FindPlayer()
    {
        Collider[] colliderPlayers = Physics.OverlapSphere(enemyFSM.transform.position, enemyFSM.viewDistance, enemyFSM.targetMask);
        if (colliderPlayers.Length == 0)
        {
            targetPlayer = null;
            enemyFSM.TransitionState(StateType.Patrol);
            return;
        }
        float minDis = Mathf.Infinity;
        foreach (var collider in colliderPlayers)
        {
            float dis = Vector3.Distance(enemyFSM.transform.position, collider.transform.position);
            if (dis < minDis)
            {
                minDis = dis;
                targetPlayer = collider.transform;
            }
        }
    }

    void ChasePlayer()
    {
        if(targetPlayer == null)  return;
        agent.SetDestination(targetPlayer.transform.position);
    }

    void FightWithPlayer()
    {
        Collider[] colliderPlayers = Physics.OverlapSphere(enemyFSM.transform.position, 2f, enemyFSM.targetMask);
        if (colliderPlayers.Length == 0)
            return;
        enemyFSM.TransitionState(StateType.Fight);
    }
}
