using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ComboConfig", menuName = "SO/Combat/ComboConfig")]
public class ComboConfigSO : ScriptableObject
{
    [Header("基础数据")]
    public string comboName;
    public float coldTime;
    [Header("交互数据")]
    public ComboInteractionConfig[] comboInteractionConfig;

    [Header("攻击检测数据")]
    public AttackDetectionConfig[] attackDetectionConfig;  //可以搞多个检测

    [Header("打击感数据")]
    public AttackFeedbackConfig[] attackFeedbackConfig;

    [Header("特效数据")]
    public FXConfig[] fxConfig;

    [Header("音效数据")]
    public SFXConfig[] sfxConfig;

    [Header("自身位移补偿数据")]
    public SelfMoveOffsetConfig[] selfMoveOffsetConfig;

    [Header("目标位移补偿数据")]
    public TargetMoveOffsetConfig[] targetMoveOffsetConfig;

}

[System.Serializable]
public class ComboInteractionConfig
{
    public string hitName;  //攻击类型,可以用来播放对应动画
    public string hitAirName;
    //武器类型
    public Weapon weapon;
    //攻击力度
    public AttackForce attackForce;
    public float damageMul;
}

[System.Serializable]
public class AttackDetectionConfig
{
    public float startTime;
    public Vector3 position;
    public Vector3 rotation;
    public Vector3 scale;
}
[System.Serializable]
public class AttackFeedbackConfig
{
    //屏幕震动
    public float strength;
    public float frequency;
    public float duration;
}
[System.Serializable]
public class FXConfig
{
    public float startTime;
    public GameObject FXObject;
    public Vector3 position;
    public Vector3 rotation;
    public Vector3 scale;
}
[System.Serializable]
public class SFXConfig
{
    public float startTime;
    public AudioClip audioClip;
    public float volume;
    public float duration;
}

[System.Serializable]
public class SelfMoveOffsetConfig
{
    public float startTime;
    public AnimationCurve animationCurve;
    //方向
    public MoveOffsetDirection moveOffsetDirection;
    public float scale;
    public float duration;
}

[System.Serializable]
public class TargetMoveOffsetConfig
{
    public float startTime;
    public AnimationCurve animationCurve;
    //方向
    public MoveOffsetDirection moveOffsetDirection;
    public float scale;
    public float duration;
}
