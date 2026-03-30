using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

[TaskCategory("Enemy")]
public class EnemyEnterDeadAction : Action
{
    private EnemyMovementController _enemyMovementController;

    public override void OnAwake()
    {
        base.OnAwake();
        _enemyMovementController = GetComponent<EnemyMovementController>();
    }
    public override void OnStart()
    {

        if (_enemyMovementController != null)
        {
            _enemyMovementController.SetApplyFightMovement(false);
            _enemyMovementController.SetAnimatorMovementValue(0f);
        }
        Debug.Log("敌人死亡");
    }

    public override TaskStatus OnUpdate()
    {
        return TaskStatus.Success;
    }
}
