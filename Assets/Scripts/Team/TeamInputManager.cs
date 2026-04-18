using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamInputManager : MonoBehaviour
{
    public Queue<int> linkAttackQueue = new Queue<int>();
    private void Update()
    {
        if (GameInputManager.Instance.Fire1)
        {
            TeamManager.Instance.operatorCombatInputControllers[TeamManager.Instance.mainCharacterIndex].TryToNormalAttack();
        }

        if (GameInputManager.Instance.Skill1)
        {
            TeamManager.Instance.operatorCombatInputControllers[0].TryToSkillAttack();
        }
        if (GameInputManager.Instance.Skill2)
        {
            TeamManager.Instance.operatorCombatInputControllers[1].TryToSkillAttack();
        }
        if (GameInputManager.Instance.Skill3)
        {
            TeamManager.Instance.operatorCombatInputControllers[2].TryToSkillAttack();
        }
        if (GameInputManager.Instance.Skill4)
        {
            TeamManager.Instance.operatorCombatInputControllers[3].TryToSkillAttack();
        }
        if (GameInputManager.Instance.Link && linkAttackQueue.Count > 0)
        {
            int index = linkAttackQueue.Dequeue();
            TeamManager.Instance.operatorCombatInputControllers[index].TryToLinkAttack();
        }
    }
}
