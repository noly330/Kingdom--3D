using BehaviorDesigner.Runtime.Tasks;

[TaskCategory("Enemy")]
public class EnemyDeadCondition : Conditional
{
    private CharacterBase characterBase;

    public override void OnAwake()
    {
        base.OnAwake();
        characterBase = GetComponent<CharacterBase>();
    }
    public override TaskStatus OnUpdate()
    {
        return characterBase.isDead ? TaskStatus.Success : TaskStatus.Failure; 
    }

}
