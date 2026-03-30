using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

[TaskCategory("Enemy")]
public class EnemyReturnToPoolAction : Action
{
    private CharacterBase _character;
    private bool _hasReturnedToPool;
    private Animator _animator;

    public override void OnAwake()
    {
        base.OnAwake();
        _character = GetComponent<CharacterBase>();
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
        _character.isDead = false;
        _animator.SetBool("IsDead", false);
        ObjectPool.instance.ReturnPool(_character.characterPoolType, _character.gameObject);
        return TaskStatus.Success;
    }
}
