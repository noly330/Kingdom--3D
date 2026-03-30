using BehaviorDesigner.Runtime.Tasks;

[TaskCategory("Enemy")]
public class EnemyIsFindTargetCondition : Conditional
{
    private EnemyAI _enemyAI;

    public override void OnStart()
    {
        _enemyAI = GetComponent<EnemyAI>();
    }

    public override TaskStatus OnUpdate()
    {
        return _enemyAI.target != null ? TaskStatus.Success : TaskStatus.Failure;
    }
}
