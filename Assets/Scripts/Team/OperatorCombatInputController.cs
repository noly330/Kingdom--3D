using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OperatorCombatInputController : MonoBehaviour
{
    private CombatStateMachineBase _combatStateMachine;

    private void Awake()
    {
        _combatStateMachine = GetComponent<CombatStateMachineBase>();
    }

    public void TryToNormalAttack()
    {
        _combatStateMachine.TryTransitionTO(CombatStateType.NormalAttack);
    }

    public void TryToSkillAttack()
    {
        _combatStateMachine.TryTransitionTO(CombatStateType.Skill);
    }

    public void TryToLinkAttack()
    {
        _combatStateMachine.TryTransitionTO(CombatStateType.LinkSkill);
    }
}
