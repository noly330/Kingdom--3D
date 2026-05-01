using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAbnormalityManager : MonoBehaviour
{
    public int breakstack;
    

    public void OnPhysicalDefenseBreakApplied()
    {
        if(breakstack <=4)
            breakstack++;
        EventCenter.Broadcast(new Events.OnPhysicalDefenseBreakApplied());
    }
}
