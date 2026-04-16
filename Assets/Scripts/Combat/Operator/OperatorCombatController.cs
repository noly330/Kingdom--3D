using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OperatorCombatController : CombatControllerBase
{
    [SerializeField] private CombatListSO _skillCombatList;
    [SerializeField] private CombatListSO _linkCombatList;
    [SerializeField] private CombatListSO _ultimateCombatList;
}
