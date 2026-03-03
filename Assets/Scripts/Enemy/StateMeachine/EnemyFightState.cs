using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyFightState : IEnemyState
{

    private EnemyFSM enemyFSM;

    private NavMeshAgent agent;
    public EnemyFightState(EnemyFSM fSM)
    {
        this.enemyFSM = fSM;
    }
    public void OnEnter()
    {
        agent = enemyFSM.M_agent;

    }

    public void OnUpdate()
    {
        FightWithPlayer();
    }
    public void OnExit()
    {

    }

    void FightWithPlayer()
    {
        Collider[] colliderPlayers = Physics.OverlapSphere(enemyFSM.transform.position, 2f, enemyFSM.targetMask);
        if (colliderPlayers.Length == 0)
        {
            enemyFSM.TransitionState(StateType.Chase);
            return;
        }

        if (enemyFSM.canExecuteCombo)
        {
            agent.isStopped = true;
            enemyFSM.ExecuteCombo();
            agent.isStopped = false;
        }
    }

}
