using UnityEditor.ShaderGraph.Serialization;
using UnityEngine;

public class HitState : ICombatState
{
    private CombatControllerBase _combatController;
    private CombatStateMachineBase _combatStateMachine;
    private Animator _animator;

    private bool _isEnd;
    public HitState(CombatControllerBase combatController, CombatStateMachineBase combatStateMachine)
    {
        _combatController = combatController;
        _combatStateMachine = combatStateMachine;
        _animator = _combatController.animator;
    }
    public void OnEnter()
    {
        
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
        if(_animator.GetCurrentAnimatorStateInfo(0).IsTag("Hurt") && _isEnd)
            _isEnd = false;
        if(!_animator.GetCurrentAnimatorStateInfo(0).IsTag("Hurt") && !_isEnd)
            _combatStateMachine.ReturnToDefaultState();
    }
}
