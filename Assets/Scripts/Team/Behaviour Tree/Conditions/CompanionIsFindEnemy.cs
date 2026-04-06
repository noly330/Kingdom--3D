
using BehaviorDesigner.Runtime.Tasks;

[TaskCategory("Companion")]
public class CompanionIsFindEnemy : Conditional
{
    private CompanionAI _companionAI;

    public override void OnAwake()
    {
        base.OnAwake();
        _companionAI = GetComponent<CompanionAI>();
    }

    public override TaskStatus OnUpdate()
    {
        return _companionAI.enemyTransform != null ? TaskStatus.Success : TaskStatus.Failure;
    }
}
