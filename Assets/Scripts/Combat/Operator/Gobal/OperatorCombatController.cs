using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OperatorCombatController : CombatControllerBase
{
    [SerializeField] private CombatListSO _skillCombatList;
    public CombatListSO skillCombatList => _skillCombatList;
    [SerializeField] private CombatListSO _linkCombatList;
    public CombatListSO linkCombatList => _linkCombatList;
    [SerializeField] private CombatListSO _ultimateCombatList;
    public CombatListSO ultimateCombatList => _ultimateCombatList;

    public bool CanSkillAttack()
    {
        return TeamManager.Instance.teamCurrentEnergy >= 100;
    }
}
