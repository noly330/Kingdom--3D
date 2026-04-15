using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCombatInputControllerBase : MonoBehaviour
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
}
