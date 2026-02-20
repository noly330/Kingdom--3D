using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ComboList", menuName = "SO/Combat/ComboList")]
public class ComboListSO : ScriptableObject
{
    [SerializeField] private ComboConfigSO[] comboList;
    [SerializeField] private AttackType attackType;

    public AttackType TryGetAttackType => attackType;

    public int TryGetComboListCount()
    {
        return comboList.Length;
    }
    public string TryGetComboName(int comboIndex)
    {
        if(comboIndex >= comboList.Length)  return null;
        return comboList[comboIndex].comboName;
    }
    public float TryGetColdTime(int comboIndex)
    {
        if(comboIndex >= comboList.Length)  return 0;
        return comboList[comboIndex].coldTime;
    }
    public ComboInteractionConfig TryGetComboInteractionConfig(int comboIndex, int eventIndex)
    {
        if(comboIndex >= comboList.Length)  return null;
        if(eventIndex >= comboList[comboIndex].comboInteractionConfig.Length)  return null;
        return comboList[comboIndex].comboInteractionConfig[eventIndex];
    }

    //eventIndex是用来判定有几段攻击，根据列表的方式
    public AttackDetectionConfig TryGetAttackDetectionConfig(int comboIndex,int eventIndex)  
    {
        if(comboIndex >= comboList.Length)  return null;
        if(eventIndex >= comboList[comboIndex].attackDetectionConfig.Length)  return null;
        return comboList[comboIndex].attackDetectionConfig[eventIndex];
    }

    public AttackFeedbackConfig TryGetAttackFeedbackConfig(int comboIndex,int eventIndex)
    {
        if(comboIndex >= comboList.Length)  return null;
        if(eventIndex >= comboList[comboIndex].attackFeedbackConfig.Length)  return null;
        return comboList[comboIndex].attackFeedbackConfig[eventIndex];
    }
    public FXConfig TryGetFXConfig(int comboIndex,int eventIndex)
    {
        if(comboIndex >= comboList.Length)  return null;
        if(eventIndex >= comboList[comboIndex].fxConfig.Length)  return null;
        return comboList[comboIndex].fxConfig[eventIndex];
    }
    public SFXConfig TryGetSFXConfig(int comboIndex,int eventIndex)
    {
        if(comboIndex >= comboList.Length)  return null;
        if(eventIndex >= comboList[comboIndex].sfxConfig.Length)  return null;
        return comboList[comboIndex].sfxConfig[eventIndex];
    }
    public MoveOffsetConfig TryGetSelfMoveOffsetConfig(int comboIndex,int eventIndex)
    {
        if(comboIndex >= comboList.Length)  return null;
        if(eventIndex >= comboList[comboIndex].selfMoveOffsetConfig.Length)  return null;
        return comboList[comboIndex].selfMoveOffsetConfig[eventIndex];
    }
    public MoveOffsetConfig TryGetTargetMoveOffsetConfig(int comboIndex,int eventIndex)
    {
        if(comboIndex >= comboList.Length)  return null;
        if(eventIndex >= comboList[comboIndex].targetMoveOffsetConfig.Length)  return null;
        return comboList[comboIndex].targetMoveOffsetConfig[eventIndex];
    }
    
}