using System;
using System.Collections;
using Events;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainStateCard : MonoBehaviour
{
    public Image healthFill;
    public TextMeshProUGUI helathText;
    public Image[] energyFill;
    private float energy3;
    private float smoothTime;


    private void OnEnable()
    {
        EventCenter.AddListener<Events.SwitchMainCharacter>(UpdateMainStateCard);
    }

    private void OnDisable()
    {
        EventCenter.RemoveListener<Events.SwitchMainCharacter>(UpdateMainStateCard);
    }
    
    private void Start()
    {
        energy3 = TeamManager.Instance.teamMaxEnergy / 3;
    }


    private void Update()
    {
        UpdateEnemyUI();
    }
    private void UpdateMainStateCard(SwitchMainCharacter character)
    {
        UpdateMainStateCard();
    }

    public void UpdateMainStateCard()
    {
        PlayerCharacter playerCharacter = TeamManager.Instance.GetPlayerCharacter(TeamManager.Instance.mainCharacterIndex);
        if(playerCharacter == null)
            return;
        
        float targetPersent = playerCharacter.currentHealth / playerCharacter.maxHealth;

        helathText.text = playerCharacter.currentHealth.ToString("F0") + "/" + playerCharacter.maxHealth.ToString("F0");
        if (healthCoroutine != null)
            StopCoroutine(healthCoroutine);
        StartCoroutine(HealthBarSmoothChange(targetPersent));
    }
    private Coroutine healthCoroutine;
    private IEnumerator HealthBarSmoothChange(float targetPersent)
    {
        float startPercent = healthFill.fillAmount;
        float elapsed = 0f;

        while (elapsed < smoothTime)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / smoothTime;
            healthFill.fillAmount = Mathf.Lerp(startPercent, targetPersent, t);
            yield return null;
        }

        healthFill.fillAmount = targetPersent;
        healthCoroutine = null;

    }

    private void UpdateEnemyUI()
    {
        energyFill[0].fillAmount = Mathf.Clamp01(TeamManager.Instance.teamCurrentEnergy / energy3);
        energyFill[1].fillAmount = Mathf.Clamp01((TeamManager.Instance.teamCurrentEnergy - energy3) / energy3);
        energyFill[2].fillAmount = Mathf.Clamp01((TeamManager.Instance.teamCurrentEnergy - 2 * energy3) / energy3);
    }
}
