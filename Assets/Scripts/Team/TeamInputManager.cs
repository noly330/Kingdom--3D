using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamInputManager : MonoBehaviour
{
    public static TeamInputManager Instance;
    private Queue<int> _linkAttackQueue = new Queue<int>();
    public Queue<int> GetlinkAttackQueue() => _linkAttackQueue;
    private HashSet<int> _queuedSkillIds = new HashSet<int>();
    private void Awake()
    {
        Instance = this;
    }
    private void Update()
    {
        if (GameInputManager.Instance.Fire1)
        {
            TeamManager.Instance.operatorCombatInputControllers[TeamManager.Instance.mainCharacterIndex].TryToNormalAttack();
        }

        if (GameInputManager.Instance.Skill1)
        {
            TeamManager.Instance.operatorCombatInputControllers[0]?.TryToSkillAttack();
        }
        if (GameInputManager.Instance.Skill2)
        {
            TeamManager.Instance.operatorCombatInputControllers[1]?.TryToSkillAttack();
        }
        if (GameInputManager.Instance.Skill3)
        {
            TeamManager.Instance.operatorCombatInputControllers[2]?.TryToSkillAttack();
        }
        if (GameInputManager.Instance.Skill4)
        {
            TeamManager.Instance.operatorCombatInputControllers[3]?.TryToSkillAttack();
        }
        if (GameInputManager.Instance.Link && _linkAttackQueue.Count > 0)
        {
            int index = _linkAttackQueue.Peek();
            //_queuedSkillIds.Remove(index);
            //在状态机里面出队
            TeamManager.Instance.operatorCombatInputControllers[index]?.TryToLinkAttack();
        }
    }
    /// <summary>
    /// 出队链接攻击，让状态机调用免得出bug（也就是打出来的时候再出队）
    /// </summary>
    public void DequeueLinkAttack()
    {
        int index = _linkAttackQueue.Dequeue();
        _queuedSkillIds.Remove(index);
    }

    /// <summary>
    /// 尝试入队连携攻击
    /// </summary>
    /// <param name="index"></param>
    public void TryEnqueueLinkAttack(int index)
    {
        if (!CanEnqueueLinkAttack(index)) return;
        EnequeueLinkAttack(index);
    }

    private void EnequeueLinkAttack(int index)
    {
        _queuedSkillIds.Add(index);
        _linkAttackQueue.Enqueue(index);
        EventCenter.Broadcast<Events.OnLinkSkillQueueChanged>(new Events.OnLinkSkillQueueChanged());
        
    }

    private bool CanEnqueueLinkAttack(int index)
    {
        if (_queuedSkillIds.Contains(index)) return false;
        return _linkAttackQueue.Count < 4;
    }
}
