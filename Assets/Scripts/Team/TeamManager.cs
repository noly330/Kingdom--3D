using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime;
using UnityEngine.AI;

public class TeamManager : MonoBehaviour
{
    public static TeamManager Instance;
    public List<Transform> teamMembers = new List<Transform>();
    private int currentIndex;
    private int nextIndex;

    private void Awake()
    {
        Instance = this;
        teamMembers.Clear();

        // 遍历 TeamManager 的子物体
        foreach (Transform child in transform)  // transform = TeamManager 的子物体
        {
            teamMembers.Add(child);
            Debug.Log($"找到队员: {child.name}");
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            SwitchCharacter();
        }
    }

    private void SwitchCharacter()
    {
        teamMembers[currentIndex].gameObject.tag = "Companion";
        nextIndex = (currentIndex + 1) % teamMembers.Count;
        teamMembers[nextIndex].gameObject.tag = "Player";

        SwitchPosition();
        OpenCompanionController();
        OpenPlayerController();

        EventCenter.Broadcast(new Events.SwitchMainCharacter()
        {
            NewIndex = nextIndex,
            OldIndex = currentIndex,
        });
        currentIndex = nextIndex;
    }

    private void OpenCompanionController()
    {
        // 原玩家角色 → 变成同伴
        teamMembers[currentIndex].GetComponent<PlayerCombatController>().enabled = false;
        teamMembers[currentIndex].GetComponent<PlayerMovementControl>().enabled = false;

        teamMembers[currentIndex].GetComponent<CompanionAI>().enabled = true;
        teamMembers[currentIndex].GetComponent<CompanionMovementAgent>().enabled = true;
        teamMembers[currentIndex].GetComponent<CompanionCombatAgent>().enabled = true;

        BehaviorTree behaviorTree = teamMembers[currentIndex].GetComponent<BehaviorTree>();

        NavMeshAgent agent = teamMembers[currentIndex].GetComponent<NavMeshAgent>();
        if (agent != null)
        {
            agent.enabled = true;
            agent.isStopped = false;
        }

        // 然后再启用 Behavior Tree
        behaviorTree.enabled = true;
        behaviorTree.EnableBehavior();
    }

    private void OpenPlayerController()
    {
        // 原同伴角色 → 变成玩家
        teamMembers[nextIndex].GetComponent<PlayerCombatController>().enabled = true;
        teamMembers[nextIndex].GetComponent<PlayerMovementControl>().enabled = true;

        teamMembers[nextIndex].GetComponent<CompanionAI>().enabled = false;
        teamMembers[nextIndex].GetComponent<CompanionMovementAgent>().enabled = false;
        teamMembers[nextIndex].GetComponent<CompanionCombatAgent>().enabled = false;

        BehaviorTree behaviorTree = teamMembers[nextIndex].GetComponent<BehaviorTree>();
        behaviorTree.DisableBehavior();
        behaviorTree.enabled = false;

        NavMeshAgent agent = teamMembers[nextIndex].GetComponent<NavMeshAgent>();
        agent.isStopped = true;
        agent.enabled = false;
        
    }

    private void SwitchPosition()
    {
        teamMembers[currentIndex].gameObject.SetActive(false);
        teamMembers[nextIndex].gameObject.SetActive(false);

        Vector3 tempPosition = teamMembers[currentIndex].position;
        teamMembers[currentIndex].position = teamMembers[nextIndex].position;
        teamMembers[nextIndex].position = tempPosition;

        teamMembers[currentIndex].gameObject.SetActive(true);
        teamMembers[nextIndex].gameObject.SetActive(true);
    }
    
}
