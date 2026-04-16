

using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

[TaskCategory("Companion")]
public class CompanionAttack : Action
{    private OperatorCombatInputController _companionCombatController;
    private CompanionMovementAgent _companionMovementAgent;
    private CompanionAI _companionAI;
    private Animator _animator;

    public override void OnAwake()
    {
        base.OnAwake();
        _companionMovementAgent = GetComponent<CompanionMovementAgent>();
        _companionCombatController = GetComponent<OperatorCombatInputController>();
        _companionAI = GetComponent<CompanionAI>();
        _animator = GetComponent<Animator>();
    }

    public override void OnStart()
    {
        if(_companionAI != null)
        {
            _companionAI.navMeshAgent.isStopped = true;
        }
        _companionMovementAgent.SetAnimatorMovementValue(0);
        _animator.SetBool(AnimationID.HasInputID, false);
    }

    public override TaskStatus OnUpdate()
    {
        _companionCombatController.TryToNormalAttack();
        return TaskStatus.Success;
    }
}
