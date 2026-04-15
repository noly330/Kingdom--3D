using BehaviorDesigner.Runtime.Tasks;

[TaskCategory("Companion")]
public class CompanionCanAttack : Conditional
{
    private ComboControllerBase _combatController;

    public override void OnAwake()
    {
        base.OnAwake();
        _combatController = GetComponent<ComboControllerBase>();
    }

    public override TaskStatus OnUpdate()
    {
        return _combatController.canExecuteCombo ? TaskStatus.Success : TaskStatus.Failure;
    }
}
