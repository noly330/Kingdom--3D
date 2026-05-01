using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OperatorCombatStateMachine : CombatStateMachineBase
{
    private OperatorCombatController _operatorCombatController;
    private PlayerMovementControl _playerMovementControl;
    protected override void Awake()
    {
        base.Awake();
        _operatorCombatController = GetComponent<OperatorCombatController>();
        _playerMovementControl = GetComponent<PlayerMovementControl>();
    }

    protected override void InitStates()
    {
        base.InitStates();
        //TODO:未来干员越来越独特的话，需要在字典里给每个干员的CombatstateTyoe对应专属的接口
        states.Add(CombatStateType.Skill, new SkillAttackState(_operatorCombatController, this));
        states.Add(CombatStateType.LinkSkill, new LinkAttackState(_operatorCombatController, this));
        states.Add(CombatStateType.Slide, new SlideState(this, _playerMovementControl));
    }
}
