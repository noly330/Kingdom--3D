using BehaviorDesigner.Runtime.Tasks;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.AI;

[TaskCategory("Companion")]
public class CompanionFollowPlayer : Action
{
    [SerializeField] private float _stopDistance = 3.5f;
    [SerializeField] private float _toChaseDistance = 7f;
    private CompanionMovementAgent _companionMovementAgent;
    private CompanionAI _companionAI;

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
        _animator.SetBool(AnimationID.HasInputID, true);
        _companionAI.navMeshAgent.isStopped = false;
    }

    public override TaskStatus OnUpdate()
    {
        if (_companionAI.GetDistanceToPlayer() <= _stopDistance)
        {
            return TaskStatus.Success;
        }

        _companionAI.navMeshAgent.SetDestination(_companionAI.playerTransform.position);  

        if (_companionAI.GetDistanceToPlayer() <= _toChaseDistance)
        {
            _animator.SetBool(AnimationID.IsRunID, false);
            _companionMovementAgent.SetAnimatorMovementValue(2f);
            return TaskStatus.Running;
        }
        else
        {
            _animator.SetBool(AnimationID.IsRunID, true);
            _companionMovementAgent.SetAnimatorMovementValue(3f);
            return TaskStatus.Running;
        }
    }

    public override void OnEnd()
    {
        _animator.SetBool(AnimationID.HasInputID, false);
        if (_companionAI != null &&
        _companionAI.navMeshAgent != null &&
        _companionAI.navMeshAgent.enabled &&
        _companionAI.navMeshAgent.isOnNavMesh)
        {
            _companionAI.navMeshAgent.isStopped = true;
        }
    }

}
