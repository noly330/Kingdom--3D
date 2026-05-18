using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 目前只实现物理异常，法术异常什么的以后有时间弄
/// </summary>
public class EnemyAbnormalityManager : MonoBehaviour
{
    public int breakStack;  //TODO:我知道不能暴露，但是为了方便调试，先暴露出来，毕竟这个项目只有我一个人在做
    public int shatterStacks = 0;  //碎甲层数
    private float _shatterTimer=0f;


    private void Update()
    {
        UpdateShatterTimer();
    }

    /// <summary>
    /// 叠加物理破防的方法
    /// </summary>
    public void OnPhysicalDefenseBreakApplied()
    {
        if(breakStack <=4)
            breakStack++;
        EventCenter.Broadcast(new Events.OnPhysicalDefenseBreakApplied());
    }

    public void ResetBreakStack()
    {
        EventCenter.Broadcast(new Events.OnPhysicalDefenseBreakConsumed() { breakStack = breakStack });
        breakStack = 0;
    }


    #region 碎甲相关
    public float GetShatterDamageMultiplier()
    {
        //TODO；以后碎甲的增上不能是线性函数，以后记得改
        if(shatterStacks == 0)
            return 1;

        return 1 + 0.12f +shatterStacks * 0.04f;
    }

    public void SetShatter(int stack)
    {
        shatterStacks = stack;
        _shatterTimer = 12f +stack * 4f;
    }

    private void UpdateShatterTimer()
    {
        _shatterTimer -= Time.deltaTime;
        if(_shatterTimer <= 0)
        {
            shatterStacks = 0;
            _shatterTimer = 0f;
        }
    }

    #endregion
}
