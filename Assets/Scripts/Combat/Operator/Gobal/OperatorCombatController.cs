using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OperatorCombatController : CombatControllerBase
{
    [SerializeField] private CombatListSO _skillCombatList;
    public CombatListSO skillCombatList => _skillCombatList;
    [SerializeField] private CombatListSO _linkCombatList;
    public CombatListSO linkCombatList => _linkCombatList;
    [SerializeField] private CombatListSO _ultimateCombatList;
    public CombatListSO ultimateCombatList => _ultimateCombatList;

    public float linkEnergy = 15f;
    public float currentLinkEnergy = 15f;

    public bool CanSkillAttack()
    {
        return TeamManager.Instance.teamCurrentEnergy >= 100;
    }

    protected void LinkSkillCold()
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
