using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

public class AIFreeMovementAction : Action
{
    private EnemyMovementController _enemyMovementController;
    private EnemyCombatController _enemyCombatController;
    private int _actionIndex = 0;  //动作索引
    private int _lastActionIndex;  //上一个动作索引
    private float _actiontime;
    public override void OnAwake()
    {
        base.OnAwake();
        _enemyMovementController = GetComponent<EnemyMovementController>();
        _enemyCombatController = GetComponent<EnemyCombatController>();
        _lastActionIndex = _actionIndex;
    }

    public override TaskStatus OnUpdate()
    {
        if (!_enemyCombatController.GetAttackCommand)
        {

            if(DistanceForTarget() >= 4f)
            {
                _enemyMovementController.SetApplyMovement(false);
                _enemyMovementController.SetAnimatorMovementValue(0f, 1f);
            }
            else if(DistanceForTarget() > 1.5f - 0.01f && DistanceForTarget() < 4f + 0.01f)
            {
                _enemyMovementController.SetApplyMovement(true);
                FreeMovement();
                UpdateFreeAction();
            }
            else
            {
                _enemyMovementController.SetAnimatorMovementValue(0f, 0f);
            }
            return TaskStatus.Running;  //运行节点
        }
        else
        {
            //退出当前状态
            Debug.Log("攻击");
        }
        return TaskStatus.Success;  //节点结束
    }

    private float DistanceForTarget() => Vector3.Distance(transform.position, EnemyManager.Instance.GetMainPlayer().position);

    private void FreeMovement()
    {
        switch(_actionIndex)
        {
            case 0:
                _enemyMovementController.SetAnimatorMovementValue(0f, 1f);
                break;
            case 1:
                _enemyMovementController.SetAnimatorMovementValue(0f, -1f);
                break;
            case 2:
                _enemyMovementController.SetAnimatorMovementValue(1f, 0f);
                break;
            case 3:
                _enemyMovementController.SetAnimatorMovementValue(-1f, 0f);
                break;
            case 4:
                _enemyMovementController.SetAnimatorMovementValue(0f, 0f);
                break;
        }
    }

    private void UpdateFreeAction()
    {
        if(_actiontime > 0f)
        {
            _actiontime -= Time.deltaTime;

        }
        else
        {
            UpdateActionIndex();
            _actiontime = Random.Range(0.5f, 2f);
        }
    }

    private void UpdateActionIndex()
    {
        _lastActionIndex = _actionIndex;
        _actionIndex = Random.Range(0, 5);
        if(_actionIndex == _lastActionIndex)
        {
            _actionIndex = Random.Range(0, 5);
        }
    }
}
