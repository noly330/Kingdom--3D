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
        Debug.Log("可以释放连携技了");

        if (currentLinkEnergy >= linkEnergy)
        {
            TeamInputManager.Instance.TryEnqueueLinkAttack(TeamManager.Instance.GetSlotIndex(this.transform));
        }
    }

    private void LinkSkillCold()
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
}
