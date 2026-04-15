using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/Combat/CombatList")]
public class CombatListSO : ScriptableObject
{
    [SerializeField] private CombatConfigSO[] _combatList;

    public int TryGetCombatListCount() => _combatList.Length;
    public string TryGetCombatName(int index)
    {
        if (index > _combatList.Length - 1)
        {
            return null;
        }
        return _combatList[index].combatName;
    }

    public float TryGetColdTime(int index)
    {
        if (index > _combatList.Length - 1)
        {
            return 0f;
        }
        return _combatList[index].coldTime;
    }

    public CombatInteractionConfig TryGetInteractionConfig(int index, int interactionIndex)
    {
        if (index > _combatList.Length - 1)  return null;
        if(interactionIndex > _combatList[index].interactionConfigs.Length - 1)  return null;
        return _combatList[index].interactionConfigs[interactionIndex];
    }

    public CombatDetectConfig TryGetDetectConfig(int index , int detectIndex)
    {
        if (index > _combatList.Length - 1)  return null;
        if(detectIndex > _combatList[index].detectConfigs.Length - 1)  return null;
        return _combatList[index].detectConfigs[detectIndex];
    }
}
