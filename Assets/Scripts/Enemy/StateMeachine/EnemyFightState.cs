using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFightState : IEnemyState
{

    private EnemyFSM enemyFSM;

    private Transform targetPlayer;
    public EnemyFightState(EnemyFSM fSM)
    {
        this.enemyFSM = fSM;
    }
    public void OnEnter()
    {

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
        Collider[] colliderPlayers = Physics.OverlapSphere(enemyFSM.transform.position, 1.5f, enemyFSM.targetMask);
        if (colliderPlayers.Length == 0)
        {
            enemyFSM.TransitionState(StateType.Chase);
            return;
        }

        if (enemyFSM.canExecuteCombo)
        {
            enemyFSM.ExecuteCombo();
        }
    }

}
