using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

[TaskCategory("Enemy")]
public class EnemtChaseAction : Action
{
    private EnemyAI _enemyAI;
    private Animator _animator;
    private EnemyMovementController _enemyMovementController;
    [SerializeField] private float arriveDistance = 1.2f;

    public override void OnAwake()
    {
        _enemyAI = GetComponent<EnemyAI>();
        _animator = GetComponent<Animator>();
        _enemyMovementController = GetComponent<EnemyMovementController>();
    }

    public override void OnStart()
    {
        _enemyAI.scannerMode = ScannerMode.Nearest;
        _enemyAI.navMeshAgent.isStopped = false;
        _animator.SetBool("Move", true);
    }

    public override TaskStatus OnUpdate()
    {
        base.OnUpdate();

        if (_enemyAI.navMeshAgent.pathPending)
            return TaskStatus.Running;

        if (Vector3.Distance(transform.position, _enemyAI.target.transform.position) <= arriveDistance)
            return TaskStatus.Success;

        _enemyMovementController.SetApplyFightMovement(false);
        _enemyMovementController.SetAnimatorMovementValue(2f);
        _enemyAI.navMeshAgent.SetDestination(_enemyAI.target.transform.position);

        return TaskStatus.Running;
    }

}
