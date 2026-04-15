using UnityEngine;

[CreateAssetMenu(menuName = "SO/Combat/CombatConfig")]
public class CombatConfigSO : ScriptableObject
{
    [Header("基础配置")]
    public string combatName;
    public float coldTime;

    [Header("战斗交互配置")]
    public CombatInteractionConfig[] interactionConfigs;
    [Header("战斗检测配置")]
    public CombatDetectConfig[] detectConfigs;
}


[System.Serializable]

public class CombatInteractionConfig
{
    public string hitName;
    public float damageMul;
}

[System.Serializable]
public class CombatDetectConfig
{
    public float startTime;
    public Vector3 position;
    public Vector3 rotation;
    public Vector3 scale;
}
