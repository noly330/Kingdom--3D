using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

[TaskCategory("Enemy")]
public class EnemyIdleAction : Action
{
    private EnemyAI _enemyAI;
    private Animator _animator;
    private CharacterBase _characterBase;
    public float idleTime = 5f;
    private float idleTimer = 0f;
    public override void OnAwake()
    {
        base.OnAwake();
        _enemyAI = GetComponent<EnemyAI>();
        _animator = GetComponent<Animator>();
        _characterBase = GetComponent<CharacterBase>();
    }

    public override void OnStart()
    {
        base.OnStart();
        idleTimer = idleTime;
        _enemyAI.scannerMode = ScannerMode.Forward;
        _animator.SetBool("Move", false);
    }

    public override TaskStatus OnUpdate()
    {
        base.OnUpdate();
       
        if(idleTimer <= 0f)
        {
            return TaskStatus.Success;
        }
        else
        {
            idleTimer -= Time.deltaTime;
            return TaskStatus.Running;
        }
    }
}
