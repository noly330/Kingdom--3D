using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeiBiCombatStateMachine : CombatStateMachineBase
{
    private FeiBiCombatController _controller;
    private PlayerMovementControl _movementControl;

    protected override void Awake()
    {
        base.Awake();
        _controller = GetComponent<FeiBiCombatController>();
        _movementControl = GetComponent<PlayerMovementControl>();
    }

    protected override void InitStates()
    {
        base.InitStates();
        //TODO:未来干员越来越独特的话，需要在字典里给每个干员的CombatstateTyoe对应专属的接口
        states.Add(CombatStateType.Skill, new SkillAttackState(_controller, this));
        states.Add(CombatStateType.LinkSkill, new FeiBiLinkAttackState(_controller, this));
        states.Add(CombatStateType.Slide, new SlideState(_movementControl, this));
    }
}
