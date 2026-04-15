using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using Unity.VisualScripting;
using Unity.Mathematics;

public class NormalAttackState : ICombatState
{
    private CombatControllerBase _combatController;
    private CombatListSO _currentCombatListSO;
    private CombatStateMachineBase _combatStateMachine;
    private RunningEventIndex _runningEventIndex;
    private Animator _animator;
    private int _currentCombatIndex;
    private int _nextCombatIndex;

    private bool _isEnd = true;
    public NormalAttackState(CombatControllerBase combatControllerBase, CombatStateMachineBase combatStateMachine)
    {
        _combatController = combatControllerBase;
        _animator = combatControllerBase.animator;
        _currentCombatListSO = combatControllerBase.normalCombatList;
        _combatStateMachine = combatStateMachine;
        _runningEventIndex = new RunningEventIndex();
    }
    public void OnEnter()
    {
        ExecuteCombat();
    }

    public void OnExit()
    {
        _isEnd = true;
    }
    public void OnEnterAgain()
    {
        ExecuteCombat();
    }

    public void OnUpdate()
    {
        RunningEvent();

        if (_animator.GetCurrentAnimatorStateInfo(0).IsTag("Attack"))
            _isEnd = false;
        if (!_animator.GetCurrentAnimatorStateInfo(0).IsTag("Attack") && !_isEnd)
        {
            Debug.Log($"普攻结束");
            _combatStateMachine.ReturnToDefaultState();
        }
    }


    private void ExecuteCombat()
    {
        if (!_combatController.CanNormalAttack())
            return;
        _runningEventIndex.Reset();

        _combatController.FindTarget();
        _combatController.LookTarget();
        //先检测当前需不需要重制普攻索引
        if (_combatController.ResetNormalAttackIndex())
            _nextCombatIndex = 0;

        _currentCombatIndex = _nextCombatIndex;
        _combatController.animator.CrossFadeInFixedTime(_currentCombatListSO.TryGetCombatName(_currentCombatIndex), 0.155f, 0, 0);

        //TODO:以后要制作保留平a的机制，这个更新索引以后可能要挪到出伤之后再更新
        UpdateCombatIndex();
        _combatController.TriggerNormalAttackCold(_currentCombatListSO.TryGetColdTime(_currentCombatIndex));
        _combatController.TriggerResetNormalAttackIndexCold(_currentCombatListSO.TryGetColdTime(_currentCombatIndex));

    }

    private void UpdateCombatIndex()
    {
        _nextCombatIndex = (_nextCombatIndex + 1) % _currentCombatListSO.TryGetCombatListCount();
    }

    /// <summary>
    /// 根据动画状态执行战斗事件
    /// </summary>
    private void RunningEvent()
    {
        if (!_animator.GetCurrentAnimatorStateInfo(0).IsName(_currentCombatListSO.TryGetCombatName(_currentCombatIndex)) ||
        _animator.IsInTransition(0)) return;

        CombatDetectConfig combatDetectConfig = _currentCombatListSO.TryGetDetectConfig(
            _currentCombatIndex, _runningEventIndex.attackDetectionIndex);

        if (combatDetectConfig != null)
        {
            if (_animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= combatDetectConfig.startTime)
            {
                Vector3 boxPosition = _combatController.transform.forward * combatDetectConfig.position.z +
                                      _combatController.transform.up * combatDetectConfig.position.y +
                                      _combatController.transform.right * combatDetectConfig.position.x;

                Collider[] targetList = Physics.OverlapBox(_combatController.transform.position + boxPosition,
                combatDetectConfig.scale, quaternion.identity, _combatController.targetMask);

                //TODO:遍历敌人
                foreach (Collider taget in targetList)
                {
                    IDamageable damageable = taget.GetComponent<IDamageable>();
                    if (damageable != null)
                    {
                        damageable.BeHit(_currentCombatListSO.TryGetInteractionConfig(_currentCombatIndex, _runningEventIndex.attackDetectionIndex), _combatController.characterBase);
                    }
                }

                _runningEventIndex.attackDetectionIndex++;
            }
        }

    }

}
