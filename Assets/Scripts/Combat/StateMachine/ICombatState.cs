using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICombatState
{
    void OnEnter();
    void OnUpdate();
    void OnExit();
    //重复进入
    void OnEnterAgain();

}
