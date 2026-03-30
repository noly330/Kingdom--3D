using BehaviorDesigner.Runtime.Tasks;

[TaskCategory("Enemy")]
public class EnemyIsInFightCondition : Conditional
{
    private EnemyAI _enemyAI;

    public override void OnAwake()
    {
        base.OnAwake();
        _enemyAI = GetComponent<EnemyAI>();
    }

    public override TaskStatus OnUpdate()
    {
        return _enemyAI.enemyState == EnemyState.Fight ? TaskStatus.Success : TaskStatus.Failure;
    }
}
