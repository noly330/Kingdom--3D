
using BehaviorDesigner.Runtime.Tasks;

public class ReturenFalitureAction : Action
{
    public override TaskStatus OnUpdate()
    {
        return TaskStatus.Failure;
    }
}
