using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;

public class TeamManager : MonoBehaviour
{
    public static TeamManager Instance;
        public List<Transform> teamMembers = new List<Transform>();
    private int currentIndex;

    private void Awake()
    {
        Instance = this;
        teamMembers.Clear();  // 先清空，避免重复

        // 遍历 TeamManager 的子物体
        foreach (Transform child in transform)  // transform = TeamManager 的子物体
        {
            teamMembers.Add(child);
            Debug.Log($"找到队员: {child.name}");
        }
    }

    private void OnEnable()
    {
        EventCenter.Addlistener<Events.SwitchMainCharacter>(OnSwitchMainCharacter);

    }

    private void OnDisable()
    {
        EventCenter.RemoveListener<Events.SwitchMainCharacter>(OnSwitchMainCharacter);
    }

    private void OnSwitchMainCharacter(Events.SwitchMainCharacter message)
    {

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Debug.Log("切换主角色");
            SwitchCharacter();
        }
    }

    private void SwitchCharacter()
    {
        teamMembers[currentIndex].gameObject.tag = "Companion";
        int nextIndex = (currentIndex + 1) % teamMembers.Count;
        teamMembers[nextIndex].gameObject.tag = "Player";

        EventCenter.Broadcast(new Events.SwitchMainCharacter()
        {
            NewIndex = nextIndex,
            OldIndex = currentIndex,
        });
        currentIndex = nextIndex;
    }
    

}
