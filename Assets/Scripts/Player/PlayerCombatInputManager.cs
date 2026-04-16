using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombatInputController : MonoBehaviour
{
    private CombatStateMachineBase _combatStateMachine;

    private void Awake()
    {
        _combatStateMachine = GetComponent<CombatStateMachineBase>();
    }

    private void Update()
    {
        if (GameInputManager.Instance.Fire1)
        {
            _combatStateMachine.TryTransitionTO(CombatStateType.NormalAttack);
        }
        
    }
}
