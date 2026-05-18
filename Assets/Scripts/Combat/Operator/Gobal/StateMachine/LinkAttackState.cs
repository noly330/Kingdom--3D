using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class LinkAttackState : ICombatState
{
    private OperatorCombatController _combatController;
    private CombatStateMachineBase _combatStateMachine;
    private CombatListSO _currentCombatList;
    private RunningEventIndex _runningEventIndex;
    private Animator _animator;
    public LinkAttackState(OperatorCombatController operatorCombatController, CombatStateMachineBase combatStateMachine)
    {
        _combatController = operatorCombatController;
        _currentCombatList = operatorCombatController.GetLinkCombatList(0);
        _animator = operatorCombatController.animator;
        _combatStateMachine = combatStateMachine;
        _runningEventIndex = new RunningEventIndex();
    }
    public void OnEnter()
    {
        //_combatController.StartLinkTimeSlow(0.3f, 0.5f);
        ExecuteSkillAttack();
        _combatController.currentLinkEnergy = 0;  //连携技能量重置
        EventCenter.Broadcast(new Events.OnLinkSkillTriggered());  //触发事件
    }

    public void OnEnterAgain()
    {

    }

    public void OnExit()
    {
        _combatController.poiseLevel = ForceLevel.Basy;
    }

    public void OnUpdate()
    {
        RunningEvent();

        if (!_animator.GetCurrentAnimatorStateInfo(0).IsTag("Skill") && !_animator.IsInTransition(0))
        {
            _combatStateMachine.ReturnToDefaultState();
        }
    }

    private void ExecuteSkillAttack()
    {
        // if (!_combatController.CanSkillAttack())
        //     return;
        
        _combatController.poiseLevel = ForceLevel.Mid;
        _runningEventIndex.Reset();
        _combatController.animator.CrossFadeInFixedTime(_currentCombatList.TryGetCombatName(0), 0.1f);
        _combatController.FindTarget();
        _combatController.LookTarget();
        
        TeamInputManager.Instance.DequeueLinkAttack();
        
    }

    private void RunningEvent()
    {
        if (!_animator.GetCurrentAnimatorStateInfo(0).IsName(_currentCombatList.TryGetCombatName(0)) ||
        _animator.IsInTransition(0)) return;

        //传递伤害
        CombatDetectConfig combatDetectConfig = _currentCombatList.TryGetDetectConfig(
            0, _runningEventIndex.attackDetectionIndex);

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
                        damageable.BeHit(_currentCombatList.TryGetInteractionConfig(0, _runningEventIndex.attackDetectionIndex), _combatController.characterBase);
                    }
                }

                _runningEventIndex.attackDetectionIndex++;
            }
        }

        //播放特效
        CombatVFXConfig combatVFXConfig = _currentCombatList.TryGetVFXConfig(0, _runningEventIndex.VFXIndex);
        if (combatVFXConfig != null)
        {
            if (_animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= combatVFXConfig.startTime)
            {
                _combatController.PlayVFX(combatVFXConfig);
                _runningEventIndex.VFXIndex++;
            }
        }

        //播放音效
        CombatSFXConfig combatSFXConfig = _currentCombatList.TryGetSFXConfig(0, _runningEventIndex.SFXIndex);
        if (combatSFXConfig != null)
        {
            if (_animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= combatSFXConfig.startTime)
            {
                _combatController.PlaySFX(combatSFXConfig);
                _runningEventIndex.SFXIndex++;
            }
        }
    }
}
