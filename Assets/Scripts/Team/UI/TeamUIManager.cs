using System;
using System.Collections;
using System.Collections.Generic;
using Events;
using UnityEngine;

public class TeamUIManager : MonoBehaviour
{
    public static TeamUIManager instance;

    [SerializeField] private MainStateCard mainStateCard;
    [SerializeField] private List<StateCard> stateCards = new List<StateCard>();


    private void Awake()
    {
        instance = this;
    }

    private void OnEnable()
    {
        EventCenter.AddListener<Events.CharacterStateChanged>(UpdateStateCard);
    }


    private void OnDisable()
    {
        EventCenter.RemoveListener<Events.CharacterStateChanged>(UpdateStateCard);
    }


    private void Start()
    {
        foreach(var item in stateCards)
        {
            item.gameObject.SetActive(false);
        }
        for (int i = 0; i < TeamManager.Instance.teamMembers.Count; i++)
        {
            stateCards[i].gameObject.SetActive(true);
            UpdateStateCard(i, TeamManager.Instance.teamMembers[i].GetComponent<PlayerCharacter>());
        }
    }

    private void UpdateStateCard(CharacterStateChanged message)
    {
        if(message.CharacterIndex == TeamManager.Instance.mainCharacterIndex)
        {
            mainStateCard.UpdateMainStateCard();
        }
        UpdateStateCard(message.CharacterIndex, TeamManager.Instance.teamMembers[message.CharacterIndex].GetComponent<PlayerCharacter>());
    }
    public void UpdateStateCard(int index, PlayerCharacter Character)
    {
        stateCards[index].UpdateStateCard(Character);
    }
}
