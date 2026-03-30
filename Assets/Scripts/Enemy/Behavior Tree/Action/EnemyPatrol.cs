using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;
using UnityEngine.AI;

[TaskCategory("Enemy")]
public class EnemyPatrol : Action
{
    [SerializeField] private float patrolRadius = 8f;
    [SerializeField] private float arriveDistance = 0.5f;

    private EnemyAI enemyAI;
    private Vector3 targetPoint;
    private Animator _animator;
    private CharacterBase _characterBase;
    private EnemyMovementController _enemyMovementController;

    public override void OnAwake()
    {
        enemyAI = GetComponent<EnemyAI>();
        _animator = GetComponent<Animator>();
        _characterBase = GetComponent<CharacterBase>();
        _enemyMovementController = GetComponent<EnemyMovementController>();
    }

    public override void OnStart()
    {
        enemyAI.scannerMode = ScannerMode.Forward;
        _animator.SetBool("Move", true);

        Vector2 randomPoint = Random.insideUnitCircle * patrolRadius;
        Vector3 point = transform.position + new Vector3(randomPoint.x, 0f, randomPoint.y);

        if (NavMesh.SamplePosition(point, out NavMeshHit hit, patrolRadius, NavMesh.AllAreas))
        {
            targetPoint = hit.position;
            enemyAI.navMeshAgent.isStopped = false;
            enemyAI.navMeshAgent.SetDestination(targetPoint);
        }
    }

    public override TaskStatus OnUpdate()
    {
        base.OnUpdate();
                
        if (enemyAI.navMeshAgent.pathPending)
            return TaskStatus.Running;

        if (Vector3.Distance(transform.position, targetPoint) <= arriveDistance)
            return TaskStatus.Success;

        _enemyMovementController.SetApplyFightMovement(false);
        _enemyMovementController.SetAnimatorMovementValue(1f);

        return TaskStatus.Running;
    }

    public override void OnEnd()
    {
        enemyAI.navMeshAgent.isStopped = true;
        enemyAI.navMeshAgent.ResetPath();
    }
}

