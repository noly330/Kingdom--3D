using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

[TaskCategory("Companion")]
public class CompanionChaseEnemy : Action
{
    [SerializeField] private float arriveDistance = 1.8f;
    private CompanionAI _companionAI;
    private CompanionMovementAgent _companionMovementAgent;
    private Animator _animator;

    public override void OnAwake()
    {
        base.OnAwake();
        _companionAI = GetComponent<CompanionAI>();
        _companionMovementAgent = GetComponent<CompanionMovementAgent>();
        _animator = GetComponent<Animator>();
    }

    public override void OnStart()
    {
        if(_companionAI != null)
        {
            _companionAI.navMeshAgent.isStopped = false;
        }
        _animator.SetBool(AnimationID.HasInputID, true);
        
    }

    public override TaskStatus OnUpdate()
    {
        if (_companionAI.navMeshAgent.pathPending)
            return TaskStatus.Running;
        
        if(_companionAI.GetDistanceToEnemy() <= arriveDistance)
        {
            return TaskStatus.Success;
        }
        else
        {
            _companionAI.navMeshAgent.SetDestination(_companionAI.enemyTransform.position);
            _companionMovementAgent.SetAnimatorMovementValue(3);
            return TaskStatus.Running;
        }

        
    }


    public override void OnEnd()
    {
        base.OnEnd();
        _companionMovementAgent.SetAnimatorMovementValue(0);
        _animator.SetBool(AnimationID.HasInputID, false);
    }

}
