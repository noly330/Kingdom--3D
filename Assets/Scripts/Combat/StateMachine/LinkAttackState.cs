using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinkAttackState : ICombatState
{
    private OperatorCombatController _operatorCombatController;
    private OperatorCombatStateMachine _combatStateMachine;
    public LinkAttackState(OperatorCombatController operatorCombatController, OperatorCombatStateMachine combatStateMachine)
    {
        _operatorCombatController = operatorCombatController;
        _combatStateMachine = combatStateMachine;
    }
    public void OnEnter()
    {
        throw new System.NotImplementedException();
    }

    public void OnEnterAgain()
    {
        throw new System.NotImplementedException();
    }

    public void OnExit()
    {
        throw new System.NotImplementedException();
    }

    public void OnUpdate()
    {
        throw new System.NotImplementedException();
    }
}
