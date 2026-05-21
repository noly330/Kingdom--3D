using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using UnityEngine;

public class FeiBiCombatController : OperatorCombatController
{
    private int _recieveBreakStack = 0;
    public int GetRecieveBreakStack() => _recieveBreakStack;
    public void ResetRecieveBreakStack() => _recieveBreakStack = 0;
    private void OnEnable()
    {
        EventCenter.AddListener<Events.OnPhysicalDefenseBreakConsumed>(OnPhysicalDefenseBreakConsumed);
    }
    private void OnDisable()
    {
        EventCenter.RemoveListener<Events.OnPhysicalDefenseBreakConsumed>(OnPhysicalDefenseBreakConsumed);
    }

    protected override void Update()
    {
        base.Update();
        LinkSkillEnemyRecover();
    }
    private void OnPhysicalDefenseBreakConsumed(Events.OnPhysicalDefenseBreakConsumed message)
    {
        if(currentLinkEnergy >= linkEnergy)
        {
            Debug.Log("飞比可以释放连携技了");
            TeamInputManager.Instance.TryEnqueueLinkAttack(TeamManager.Instance.GetSlotIndex(this.transform));
            _recieveBreakStack = Mathf.Max(_recieveBreakStack, message.breakStack);
        }
    }



}
