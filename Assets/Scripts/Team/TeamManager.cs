using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime;
using UnityEngine.AI;

public class TeamManager : MonoBehaviour
{
    public static TeamManager Instance;
    public List<Transform> teamMembers = new List<Transform>();
    public List<PlayerCharacter> playerCharacters = new List<PlayerCharacter>();
    public List<OperatorCombatInputController> operatorCombatInputControllers = new List<OperatorCombatInputController>();
    public List<OperatorCombatController> operatorCombatControllers = new List<OperatorCombatController>();
    public int mainCharacterIndex;  //当前主控干员索引
    public float teamMaxEnergy;
    public float teamCurrentEnergy;
    private int _currentIndex;
    private int _nextIndex;

    private void Awake()
    {
        Instance = this;
        teamMembers.Clear();

        // 遍历 TeamManager 的子物体
        foreach (Transform child in transform)  // transform = TeamManager 的子物体
        {
            teamMembers.Add(child);
            playerCharacters.Add(child.GetComponent<PlayerCharacter>());
            operatorCombatInputControllers.Add(child.GetComponent<OperatorCombatInputController>());
            operatorCombatControllers.Add(child.GetComponent<OperatorCombatController>());
            Debug.Log($"找到队员: {child.name}");
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            SwitchCharacter();
        }
        UpdateTeamEnergy();


    }

    #region 公共方法

    public int GetSlotIndex(Transform character)
    {
        return teamMembers.IndexOf(character);
    }

    #endregion

    private void UpdateTeamEnergy()
    {
        if (teamCurrentEnergy <= teamMaxEnergy)
        {
            teamCurrentEnergy += Time.deltaTime * 5f;
        }
        else
        {
            teamCurrentEnergy = teamMaxEnergy;
        }
    }

    #region 角色切换

    private void SwitchCharacter()
    {
        _currentIndex = mainCharacterIndex;

        teamMembers[_currentIndex].gameObject.tag = "Companion";
        _nextIndex = (_currentIndex + 1) % teamMembers.Count;
        teamMembers[_nextIndex].gameObject.tag = "Player";

        mainCharacterIndex = _nextIndex;

        SwitchPosition();
        OpenCompanionController();
        OpenPlayerController();

        EventCenter.Broadcast(new Events.SwitchMainCharacter()
        {
            NewIndex = _nextIndex,
            OldIndex = _currentIndex,
        });

    }

    private void OpenCompanionController()
    {
        //teamMembers[_currentIndex].GetComponent<PlayerCombatInputController>().enabled = false;
        teamMembers[_currentIndex].GetComponent<PlayerMovementControl>().enabled = false;

        teamMembers[_currentIndex].GetComponent<CompanionAI>().enabled = true;
        teamMembers[_currentIndex].GetComponent<CompanionMovementAgent>().enabled = true;

        BehaviorTree behaviorTree = teamMembers[_currentIndex].GetComponent<BehaviorTree>();

        NavMeshAgent agent = teamMembers[_currentIndex].GetComponent<NavMeshAgent>();
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
        //teamMembers[_nextIndex].GetComponent<PlayerCombatInputController>().enabled = true;
        teamMembers[_nextIndex].GetComponent<PlayerMovementControl>().enabled = true;

        teamMembers[_nextIndex].GetComponent<CompanionAI>().enabled = false;
        teamMembers[_nextIndex].GetComponent<CompanionMovementAgent>().enabled = false;

        BehaviorTree behaviorTree = teamMembers[_nextIndex].GetComponent<BehaviorTree>();
        behaviorTree.DisableBehavior();
        behaviorTree.enabled = false;

        NavMeshAgent agent = teamMembers[_nextIndex].GetComponent<NavMeshAgent>();
        agent.isStopped = true;
        agent.enabled = false;

    }

    private void SwitchPosition()
    {
        teamMembers[_currentIndex].gameObject.SetActive(false);
        teamMembers[_nextIndex].gameObject.SetActive(false);

        Vector3 tempPosition = teamMembers[_currentIndex].position;
        Vector3 tempRotation = teamMembers[_currentIndex].eulerAngles;

        teamMembers[_currentIndex].position = teamMembers[_nextIndex].position;
        teamMembers[_nextIndex].position = tempPosition;
        teamMembers[_nextIndex].eulerAngles = tempRotation;

        teamMembers[_currentIndex].gameObject.SetActive(true);
        teamMembers[_nextIndex].gameObject.SetActive(true);
    }

    #endregion


}
