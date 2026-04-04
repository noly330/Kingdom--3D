using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

public class EnemyJustMoveForwardAction : Action
{
    private EnemyMovementController _enemyMovementController;
    private EnemyAI _enemyAI;
    private Animator _animator;

    public override void OnAwake()
    {
        _enemyMovementController = GetComponent<EnemyMovementController>();
        _enemyAI = GetComponent<EnemyAI>();
        _animator = GetComponent<Animator>();
    }

    public override void OnStart()
    {
        base.OnStart();
        _animator.SetBool("Move", true);
        _enemyAI.navMeshAgent.isStopped = false;
    }

    public override TaskStatus OnUpdate()
    {
        //LookAtTarget();
        _enemyAI.navMeshAgent.SetDestination(_enemyAI.target.transform.position);
        _enemyMovementController.SetApplyFightMovement(true);
        _enemyMovementController.SetFightAnimatorMovementValue(0f, 1f);
        return TaskStatus.Running;
    }

    private void LookAtTarget()
    {
        if (_enemyAI.target == null)
            return;
        transform.LookAt(_enemyAI.target);
    }

}
