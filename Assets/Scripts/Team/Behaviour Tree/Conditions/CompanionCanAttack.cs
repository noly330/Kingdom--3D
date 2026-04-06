using BehaviorDesigner.Runtime.Tasks;

[TaskCategory("Companion")]
public class CompanionCanAttack : Conditional
{
    private CombatControllerBase _combatController;

    public override void OnAwake()
    {
        base.OnAwake();
        _combatController = GetComponent<CombatControllerBase>();
    }

    public override TaskStatus OnUpdate()
    {
        return _combatController.canExecuteCombo ? TaskStatus.Success : TaskStatus.Failure;
    }
}
