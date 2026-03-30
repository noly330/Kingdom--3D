
using BehaviorDesigner.Runtime.Tasks;

[TaskCategory("Enemy")]
public class EnemyIsCloseToPlayer : Conditional
{
    private EnemyAI _enemyAI;
    public float arriveDistance = 2f;

    public override void OnAwake()
    {
        base.OnAwake();
        _enemyAI = GetComponent<EnemyAI>();
    }

    public override TaskStatus OnUpdate()
    {
        return _enemyAI.GetDistanceToTarget() <= arriveDistance ? TaskStatus.Success : TaskStatus.Failure;
    }

}
