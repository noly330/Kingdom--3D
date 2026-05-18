using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LuoXiCombatController : OperatorCombatController
{
    private void OnEnable()
    {
        EventCenter.AddListener<Events.OnLinkSkillTriggered>(TryQueueLinkSkill);
    }

    private void OnDisable()
    {
        EventCenter.RemoveListener<Events.OnLinkSkillTriggered>(TryQueueLinkSkill);
    }

    protected override void Update()
    {
        base.Update();

        LinkSkillEnemyRecover();
    }

    private void TryQueueLinkSkill(Events.OnLinkSkillTriggered triggered)
    {
        if(currentLinkEnergy >= linkEnergy)
        {
            Debug.Log("洛茜可以释放连携技了");
            TeamInputManager.Instance.TryEnqueueLinkAttack(TeamManager.Instance.GetSlotIndex(this.transform));
        }
    }


    
}
