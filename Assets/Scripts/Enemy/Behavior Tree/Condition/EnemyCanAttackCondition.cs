using BehaviorDesigner.Runtime.Tasks;

[TaskCategory("Enemy")]
public class EnemyCanAttackCondition : Conditional
{
    private CombatControllerBase _combatController;

    public override void OnAwake()
    {
        base.OnAwake();
        _combatController = GetComponent<CombatControllerBase>();
    }

    public override TaskStatus OnUpdate()
    {
        return _combatController.CanNormalAttack() ? TaskStatus.Success : TaskStatus.Failure;
    }
}
