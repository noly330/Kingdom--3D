using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillAttackState : ICombatState
{
    private OperatorCombatController _operatorCombatController;
    private OperatorCombatStateMachine _operatorCombatStateMachine;
    public SkillAttackState(OperatorCombatController operatorCombatController,OperatorCombatStateMachine operatorCombatStateMachine)
    {
        _operatorCombatController = operatorCombatController;
        _operatorCombatStateMachine = operatorCombatStateMachine;
    }
    public void OnEnter()
    {
        
    }

    public void OnEnterAgain()
    {
        
    }

    public void OnExit()
    {
        
    }

    public void OnUpdate()
    {
        
    }
}
