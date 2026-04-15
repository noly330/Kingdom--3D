using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

public class EnemyFreeMovementAction : Action
{
    private EnemyMovementController _enemyMovementController;
    private CombatControllerBase _combatController;
    private Animator _animator;
    private EnemyAI _enemyAI;
    private int _actionIndex = 0;  //动作索引
    private int _lastActionIndex;  //上一个动作索引
    private float _actiontime;
    public override void OnAwake()
    {
        base.OnAwake();
        _enemyMovementController = GetComponent<EnemyMovementController>();
        _combatController = GetComponent<CombatControllerBase>();
        _enemyAI = GetComponent<EnemyAI>();
        _animator = GetComponent<Animator>();
        _lastActionIndex = _actionIndex;
    }

    public override void OnStart()
    {
        base.OnStart();
        _animator.SetBool("Move", true);
        _enemyAI.navMeshAgent.isStopped = false;
    }


    public override TaskStatus OnUpdate()
    {

        //if(!_animator.GetCurrentAnimatorStateInfo(0).IsTag("Attack"))
            //LookAtTarget();
        _enemyAI.navMeshAgent.SetDestination(_enemyAI.target.transform.position);
        if (!_combatController.CanNormalAttack())
        {
            _enemyMovementController.SetApplyFightMovement(true);
            FreeMovement();
            UpdateFreeAction();

            return TaskStatus.Running;  //运行节点
        }
        else
        {
            //退出当前状态

        }
        return TaskStatus.Success;  //节点结束
    }

    private float DistanceForTarget()
    {
        if (_enemyAI.target == null)
            return 0f;
        return Vector3.Distance(transform.position, _enemyAI.target.position);
    }


    private void FreeMovement()
    {
        switch (_actionIndex)
        {
            case 0:
                _enemyMovementController.SetFightAnimatorMovementValue(0f, 1f);
                break;
            case 1:
                _enemyMovementController.SetFightAnimatorMovementValue(-1f, 0f);
                break;
            case 2:
                _enemyMovementController.SetFightAnimatorMovementValue(1f, 0f);
                break;
            case 3:
                _enemyMovementController.SetFightAnimatorMovementValue(0f, -1f);
                break;
                
        }
    }

    private void UpdateFreeAction()
    {
        if (_actiontime > 0f)
        {
            _actiontime -= Time.deltaTime;

        }
        else
        {
            UpdateActionIndex();
            _actiontime = Random.Range(1.5f, 3f);
        }
    }

    private int _maxActionIndex = 4;
    private void UpdateActionIndex()
    {
        if(_enemyAI.GetDistanceToTarget() > 0.7f* _enemyAI.GetScanRadius())
        {
            _maxActionIndex = 3;
        }
        else
        {
            _maxActionIndex = 4;
        }
        _lastActionIndex = _actionIndex;
        _actionIndex = Random.Range(0, _maxActionIndex);
        if (_actionIndex == _lastActionIndex)
        {
            _actionIndex = Random.Range(0, _maxActionIndex);
        }
    }

    private void LookAtTarget()
    {
        if (_enemyAI.target == null)
            return;
        transform.LookAt(_enemyAI.target);
    }
}
