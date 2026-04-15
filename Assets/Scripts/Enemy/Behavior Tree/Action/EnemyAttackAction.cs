using BehaviorDesigner.Runtime.Tasks;

[TaskCategory("Enemy")]
public class EnemyAttackAction : Action
{
    private CombatControllerBase _combatController;
    private EnemyCombatInputControllerBase _enemyCombatInputController;
    private EnemyAI _enemyAI;

    public override void OnAwake()
    {
        base.OnAwake();
        _combatController = GetComponent<CombatControllerBase>();
        _enemyCombatInputController = GetComponent<EnemyCombatInputControllerBase>();
        _enemyAI = GetComponent<EnemyAI>();
    }

    public override void OnStart()
    {
        if (_enemyAI)
        {
            _enemyAI.navMeshAgent.isStopped = true;
        }
    }

    public override TaskStatus OnUpdate()
    {
        _enemyCombatInputController.TryToNormalAttack();
        return TaskStatus.Success;
    }
}
