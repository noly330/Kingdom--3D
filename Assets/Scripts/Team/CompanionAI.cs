using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompanionAI : MonoBehaviour
{
    private TargetScanner _targetScanner;
    public Transform playerTransform;



    private void OnEnable()
    {
        EventCenter.Addlistener<Events.SwitchMainCharacter>(FindPlayerTransform);
    }

    private void OnDisable()
    {
        EventCenter.RemoveListener<Events.SwitchMainCharacter>(FindPlayerTransform);
    }

    public void FindPlayerTransform(Events.SwitchMainCharacter message)
    {
        playerTransform = GameObject.FindWithTag("Player").transform;
        Debug.Log("查找玩家成功：" + playerTransform.name);
    }
}
