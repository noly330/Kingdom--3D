

using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

[TaskCategory("Companion")]
public class CompanionIdle : Action
{
    private CompanionMovementAgent _companionMovementAgent;
    private CompanionAI _companionAI;
    private Animator _animator;
    
    public override void OnAwake()
    {
        base.OnAwake();
        _companionMovementAgent = GetComponent<CompanionMovementAgent>();
        _companionAI = GetComponent<CompanionAI>();
        _animator = GetComponent<Animator>();
    }

    public override void OnStart()
    {
        _animator.SetBool(AnimationID.HasInputID,false);
        _companionAI.navMeshAgent.isStopped = true;
    }

    public override TaskStatus OnUpdate()
    {
        return TaskStatus.Running;
    }
    
}
