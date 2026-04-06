
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;
[TaskCategory("Companion")]
public class CompanionIsCloseToPlayer : Conditional
{
    private CompanionAI _companionAI;
    [SerializeField] private float _arriveDistance;

    public override void OnAwake()
    {
        base.OnAwake();
        _companionAI = GetComponent<CompanionAI>();
    }

    public override TaskStatus OnUpdate()
    {
        return _companionAI.GetDistanceToPlayer() <= _arriveDistance ? TaskStatus.Success : TaskStatus.Failure;
    }

}
