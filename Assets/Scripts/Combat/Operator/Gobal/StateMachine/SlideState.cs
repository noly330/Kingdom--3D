using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlideState : ICombatState
{
    private PlayerMovementControl _playerMovementControl;
    private OperatorCombatStateMachine _combatStateMachine;
    
    private Animator _animator;

    bool _isEnd = true;
    public SlideState(OperatorCombatStateMachine combatStateMachine, PlayerMovementControl playerMovementControl)
    {
        _combatStateMachine = combatStateMachine;
        _playerMovementControl = playerMovementControl;
        _animator = playerMovementControl.animator;
    }
    public void OnEnter()
    {
        _playerMovementControl.transform.eulerAngles = Vector3.up * _playerMovementControl.targetRot;
        _animator.CrossFadeInFixedTime("Slide", 0, 0, 0);
    }

    public void OnEnterAgain()
    {

    }

    public void OnExit()
    {
        _isEnd = true;
    }

    public void OnUpdate()
    {
        if (_animator.GetCurrentAnimatorStateInfo(0).IsTag("Slide"))
        {
            _isEnd = false;
        }

        if(!_isEnd && _animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.5f)
        {
            _combatStateMachine.ReturnToDefaultState();
        }
    }
}
