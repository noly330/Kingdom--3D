using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;

public class SkillAttackState : ICombatState
{
    private OperatorCombatController _combatController;
    private CombatStateMachineBase _combatStateMachine;
    private CombatListSO _currentCombatList;
    private RunningEventIndex _runningEventIndex;
    private Animator _animator;
    public SkillAttackState(OperatorCombatController operatorCombatController, CombatStateMachineBase combatStateMachine)
    {
        _combatController = operatorCombatController;
        _currentCombatList = operatorCombatController.GetSkillComabtList(0);
        _animator = operatorCombatController.animator;
        _combatStateMachine = combatStateMachine;
        _runningEventIndex = new RunningEventIndex();
    }
    public void OnEnter()
    {
        _combatController.poiseLevel = ForceLevel.Mid;
        ExecuteSkillAttack();
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


    //TODO: 技能暂时就一段吧，以后有时间拓展多段技能

    private void ExecuteSkillAttack()
    {
        _runningEventIndex.Reset();
        _combatController.animator.CrossFadeInFixedTime(_currentCombatList.TryGetCombatName(0), 0.1f);
        _combatController.FindTarget();
        _combatController.LookTarget();

        //在这里减能量条
        TeamManager.Instance.teamCurrentEnergy -= 100f;
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
