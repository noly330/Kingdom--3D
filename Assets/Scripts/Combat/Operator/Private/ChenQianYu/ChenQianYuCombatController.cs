using System;
using System.Collections;
using System.Collections.Generic;
using Events;
using UnityEngine;

public class ChenQianYuCombatController : OperatorCombatController
{

    //陈千语的连携技触发条件为：当团队给敌人挂上物理破防时
    
    void OnEnable()
    {
        
        EventCenter.AddListener<Events.OnPhysicalDefenseBreakApplied>(OnPhysicalDefenseBreakApplied);
    }

    void OnDisable()
    {
        EventCenter.RemoveListener<Events.OnPhysicalDefenseBreakApplied>(OnPhysicalDefenseBreakApplied);
    }

    protected override void Update()
    {
        base.Update();

        LinkSkillCold();
    }

    private void OnPhysicalDefenseBreakApplied(OnPhysicalDefenseBreakApplied message)
    {

        if (currentLinkEnergy >= linkEnergy)
        {
            Debug.Log("陈千语可以释放连携技了");
            TeamInputManager.Instance.TryEnqueueLinkAttack(TeamManager.Instance.GetSlotIndex(this.transform));
        }
    }

    
}
