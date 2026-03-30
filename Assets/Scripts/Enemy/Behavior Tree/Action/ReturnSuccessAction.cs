
using BehaviorDesigner.Runtime.Tasks;


public class ReturnSuccess : Action
{
    public override TaskStatus OnUpdate()
    {
        return TaskStatus.Success;
    }
}
