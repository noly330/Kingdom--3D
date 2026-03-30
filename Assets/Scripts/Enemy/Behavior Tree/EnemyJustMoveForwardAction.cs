using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

public class EnemyJustMoveForwardAction : Action
{
    private EnemyMovementController _enemyMovementController;
    private EnemyAI _enemyAI;

    public override void OnAwake()
    {
        _enemyMovementController = GetComponent<EnemyMovementController>();
        _enemyAI = GetComponent<EnemyAI>();
    }

    public override TaskStatus OnUpdate()
    {
        LookAtTarget();

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
