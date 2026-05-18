using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OperatorCombatController : CombatControllerBase
{
    [SerializeField] private CombatListSO[] _skillCombatLists;
    public CombatListSO GetSkillComabtList(int index) => _skillCombatLists[index];
    [SerializeField] private CombatListSO[] _linkCombatLists;
    public CombatListSO GetLinkCombatList(int index) => _linkCombatLists[index];
    [SerializeField] private CombatListSO _ultimateCombatList;
    public CombatListSO ultimateCombatList => _ultimateCombatList;

    public float linkEnergy = 15f;
    public float currentLinkEnergy = 15f;

    public bool CanSkillAttack()
    {
        return TeamManager.Instance.teamCurrentEnergy >= 100;
    }

    protected void LinkSkillEnemyRecover()
    {
        if (currentLinkEnergy < linkEnergy + 0.01f)
        {
            currentLinkEnergy += Time.deltaTime * 1f;
        }
        else
        {
            currentLinkEnergy = linkEnergy;
        }
    }

    // public void StartLinkTimeSlow(float slowScale, float realDuration)
    // {
    //     StartCoroutine(LinkTimeSlowCoroutine(slowScale, realDuration));
    // }

    // private IEnumerator LinkTimeSlowCoroutine(float slowScale, float realDuration)
    // {
    //     Time.timeScale = slowScale;
    //     yield return new WaitForSecondsRealtime(realDuration);
    //     Time.timeScale = 1f;
    // }
}
