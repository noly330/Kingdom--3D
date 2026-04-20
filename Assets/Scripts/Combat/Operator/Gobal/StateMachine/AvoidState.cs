using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvoidState : ICombatState
{
    [Header("完美闪避相关")]
    public AudioClip perfectAvoidClip;
    public float perfectAvoidDuration = 0.8f;  // 完美闪避总时间
    public float meshRefreshRate = 0.2f;  // 残影生成间隔
    public float tScale = 0.4f;
    private float meshDestoryDelay = 1f;
    private SkinnedMeshRenderer[] skinnedMeshRenderers;
    private Material trailMaterial;
    public void OnEnter()
    {
        
    }

    public void OnEnterAgain()
    {
        
    }

    public void OnExit()
    {
        
    }

    public void OnUpdate()
    {
        
    }
}
