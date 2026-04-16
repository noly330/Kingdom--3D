using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OperatorCombatStateMachine : CombatStateMachineBase
{
    private OperatorCombatController _operatorCombatController;
    protected override void Awake()
    {
        base.Awake();
        _operatorCombatController = GetComponent<OperatorCombatController>();
    }

    protected override void InitStates()
    {
        base.InitStates();
        states.Add(CombatStateType.Skill, new SkillAttackState(_operatorCombatController, this));
        states.Add(CombatStateType.LinkSkill, new LinkAttackState(_operatorCombatController, this));
    }
}
