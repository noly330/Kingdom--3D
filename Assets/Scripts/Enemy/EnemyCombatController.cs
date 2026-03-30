using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCombatController : CombatControllerBase
{
    [SerializeField] private bool _attackCommand;

    public bool GetAttackCommand() => _attackCommand;

    protected override void Start()
    {
        base.Start();
    }

    public void AIBaseAttackInput()
    {
        ExecuteCombo();
    }

    /// <summary>
    /// 检查当前ai状态,可能多余以后删掉
    /// </summary>
    /// <returns></returns>
    private bool CheckAIState()
    {
        if(animator.GetCurrentAnimatorStateInfo(0).IsTag("Hit"))  return false;
        return true;
    }

    public void SetAttackCommand(bool attackCommand)
    {
        if (!CheckAIState())
        {
            ResetAttackCommand();
            return;
        }

        _attackCommand = true;
    }

    private void ResetAttackCommand()
    {
        _attackCommand = false;
    }
}
