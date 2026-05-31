using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class FeiBiLinkAttackState : ICombatState
{

    private FeiBiCombatController _combatController;
    private CombatStateMachineBase _combatStateMachine;

    private CombatListSO _currentCombatList;
    private RunningEventIndex _runningEventIndex;
    private Animator _animator;

    private int _currentCombatIndex;
    private int _nextCombatIndex;

    private float _skillCooldown = 0;
    public FeiBiLinkAttackState(FeiBiCombatController controller, CombatStateMachineBase stateMachine)
    {
        _combatController = controller;
        _combatStateMachine = stateMachine;
        _animator = _combatController.animator;
        _runningEventIndex = new RunningEventIndex();


    }

    public void OnEnter()
    {
        _currentCombatList = GetLinkCombatList(_combatController.GetRecieveBreakStack());
        _combatController.ResetRecieveBreakStack();

        Debug.Log(_currentCombatList.name);
        //_combatController.StartLinkTimeSlow(0.3f, 0.5f);


        _combatController.currentLinkEnergy = 0;  //连携技能量重置
        EventCenter.Broadcast(new Events.OnLinkSkillTriggered());  //触发事件

        TeamInputManager.Instance.DequeueLinkAttack();  //连携技出队
        EventCenter.Broadcast(new Events.OnLinkSkillQueueChanged());  //触发事件
        
        _skillCooldown = _currentCombatList.TryGetColdTime(0);
        ExecuteSkillAttack();

        _combatController.StartLinkSkillTimeSlow();

        PlayerCharacter playerCharacter = _combatController.GetComponent<PlayerCharacter>();
        LinkLeftNoticeCharacterUI.Instance.SetCharacterIcon(playerCharacter.characterInfo.linkHeadSprite);
        LinkLeftNoticeCharacterUI.Instance.Show();
    }

    public void OnEnterAgain()
    {

    }

    public void OnExit()
    {
        _nextCombatIndex = 0;
        _currentCombatIndex = 0;
        _combatController.poiseLevel = ForceLevel.Basy;
    }

    public void OnUpdate()
    {
        RunningEvent();
        TryExecuteSkillOnCooldownReady();

        if (!_animator.GetCurrentAnimatorStateInfo(0).IsTag("Skill") && !_animator.IsInTransition(0))
        {
            _combatStateMachine.ReturnToDefaultState();
        }
    }

    //针对多段连携技，按时间释放
    private void TryExecuteSkillOnCooldownReady()
    {
        if (_skillCooldown <= 0)
        {
            _currentCombatIndex = _nextCombatIndex;
            _skillCooldown = _currentCombatList.TryGetColdTime(_currentCombatIndex);
            ExecuteSkillAttack();
        }
        else
        {
            _skillCooldown -= Time.deltaTime;
        }
    }

    private void ExecuteSkillAttack()
    {
        // if (!_combatController.CanSkillAttack())
        //     return;

        if (_currentCombatIndex >= _currentCombatList.TryGetCombatListCount())
        {
            _combatStateMachine.ReturnToDefaultState();
            return;
        }

        _combatController.poiseLevel = ForceLevel.Mid;
        _runningEventIndex.Reset();
        bool hasCachedTarget = _combatController.TeleportNearCachedAttackTargetIfCompanion(_combatController.linkDistance);

        _combatController.animator.CrossFadeInFixedTime(_currentCombatList.TryGetCombatName(_currentCombatIndex), 0.1f);
        if (!hasCachedTarget)
            _combatController.FindTarget();
        _combatController.LookTarget();
        UpdateCombatIndex();


    }

    private void UpdateCombatIndex()
    {
        _nextCombatIndex = _nextCombatIndex + 1;
    }

    private void RunningEvent()
    {
        if (!_animator.GetCurrentAnimatorStateInfo(0).IsName(_currentCombatList.TryGetCombatName(_currentCombatIndex)) ||
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


                bool isHit = targetList.Length > 0;
                if (isHit)
                {
                    foreach (Collider taget in targetList)
                    {
                        IDamageable damageable = taget.GetComponent<IDamageable>();
                        if (damageable != null)
                        {
                            if (_combatController.CompareTag("Player"))
                                CombatControllerBase.CacheAttackTarget(taget.transform);
                            damageable.BeHit(_currentCombatList.TryGetInteractionConfig(0, _runningEventIndex.attackDetectionIndex), _combatController.characterBase);
                        }
                    }
                    //在这里加能量条
                    CombatRecoverEnergyConfig recoverEnergyConfig = _currentCombatList.TryGetRecoverEnergyConfig(_currentCombatIndex, _runningEventIndex.attackDetectionIndex);
                    if (recoverEnergyConfig != null)
                    {
                        TeamManager.Instance.teamCurrentEnergy += recoverEnergyConfig.energyRecover;
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
    private CombatListSO GetLinkCombatList(int defenderBreakStack)
    {
        if (defenderBreakStack == 1 || defenderBreakStack == 2)
        {
            return _combatController.GetLinkCombatList(0);
        }
        else if (defenderBreakStack == 3)
        {
            return _combatController.GetLinkCombatList(1);
        }
        else if (defenderBreakStack == 4)
        {
            return _combatController.GetLinkCombatList(2);
        }
        else
        {
            return null;
        }
    }
}
