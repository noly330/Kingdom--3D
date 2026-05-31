using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinkSkillQueueUI : MonoBehaviour
{

    [SerializeField] private Transform  _linkSkillUITemplate;

    private void Start()
    {
        _linkSkillUITemplate.gameObject.SetActive(false);
        UpdateLinkSkillQueueUI();
    }
    private void OnEnable()
    {
        EventCenter.AddListener<Events.OnLinkSkillQueueChanged>(OnLinkSkillQueueChanged);
    }

    private void OnDisable()
    {
        EventCenter.RemoveListener<Events.OnLinkSkillQueueChanged>(OnLinkSkillQueueChanged);
    }
    private void OnLinkSkillQueueChanged(Events.OnLinkSkillQueueChanged e)
    {
        UpdateLinkSkillQueueUI();
    }


    private void UpdateLinkSkillQueueUI()
    {
        Debug.Log("更新连携攻击队列UI");
        Queue<int> linkSkillQueue = TeamInputManager.Instance.GetlinkAttackQueue();

        foreach(Transform child in transform)
        {
            if(child == _linkSkillUITemplate)
            {
                continue;
            }
            Destroy(child.gameObject);
        }
        Debug.Log($"当前连携攻击队列有{linkSkillQueue.Count}人");
        // if (linkSkillQueue.Count == 0)
        // {
        //     return;
        // }

        foreach(int id in linkSkillQueue)
        {
            GameObject linkSkillUI = Instantiate(_linkSkillUITemplate.gameObject, transform);
            Debug.Log($"创建连携攻击队列UI{id}");
            linkSkillUI.SetActive(true);
            LinkMemberHeadUI linkMemberHeadUI = linkSkillUI.GetComponent<LinkMemberHeadUI>();
            linkMemberHeadUI.UpdateHeadImage(TeamManager.Instance.teamMembers[id].GetComponent<PlayerCharacter>().characterInfo.headSprite);
            linkMemberHeadUI.PlayShowAnimation();
        }
    }
}
