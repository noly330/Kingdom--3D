using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

[TaskCategory("Enemy")]
public class EnemyReturnToPoolAction : Action
{
    private EnemyCharacter _enemyCharacter;
    private bool _hasReturnedToPool;
    private Animator _animator;

    public override void OnAwake()
    {
        base.OnAwake();
        _enemyCharacter = GetComponent<EnemyCharacter>();
        _animator = GetComponent<Animator>();
    }

    public override void OnStart()
    {
        _hasReturnedToPool = false;
    }

    public override TaskStatus OnUpdate()
    {
        if (_hasReturnedToPool)
            return TaskStatus.Success;

        _hasReturnedToPool = true;
        _enemyCharacter.isDead = false;
        _animator.SetBool("IsDead", false);
        ObjectPool.instance.ReturnPool(_enemyCharacter.characterPoolType, _enemyCharacter.gameObject);
        return TaskStatus.Success;
    }
}
