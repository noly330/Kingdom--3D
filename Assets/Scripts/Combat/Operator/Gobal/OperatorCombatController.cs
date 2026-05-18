using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OperatorCombatController : CombatControllerBase
{
    [SerializeField] private CombatListSO[] _skillCombatLists;
    public CombatListSO GetSkillComabtList(int index) => _skillCombatLists[index];
    [SerializeField] private CombatListSO[] _linkCombatLists;
    public CombatListSO GetLinkCombatList(int index) => _linkCombatLists[index];
    [SerializeField] private CombatListSO _ultimateCombatList;
    public CombatListSO ultimateCombatList => _ultimateCombatList;

    public float linkEnergy = 15f;
    public float currentLinkEnergy = 15f;

    public bool CanSkillAttack()
    {
        return TeamManager.Instance.teamCurrentEnergy >= 100;
    }

    protected void LinkSkillEnemyRecover()
    {
        if (currentLinkEnergy < linkEnergy + 0.01f)
        {
            currentLinkEnergy += Time.deltaTime * 1f;
        }
        else
        {
            currentLinkEnergy = linkEnergy;
        }
    }

    public override void BeHit(CombatInteractionConfig interactionConfig, CharacterBase attacker)
    {
        if (_combatStateMachine.GetCurrentStateType() == CombatStateType.Avoid)
        {
            return;
        }
        // 完美闪避触发条件只放在受击入口：角色处于Slide动画标签时被命中，改为进入Avoid状态，不结算本次受击。
        if (_combatStateMachine.GetCurrentStateType() == CombatStateType.Slide)
        {
            _combatStateMachine.ForceTransitionTo(CombatStateType.Avoid);
            return;
        }

        base.BeHit(interactionConfig, attacker);
    }

    
}
