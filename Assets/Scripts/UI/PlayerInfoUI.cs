using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInfoUI : MonoBehaviour
{
    //TODO:未来要做技能，技能能量条之类的
    public CharacterBase character;
    public Image healthFill;
    public TextMeshProUGUI helathText;
    public Image[] energyFill;
    private float energy3;
    private float smoothTime;


    private void Awake()
    {
        energy3 = character.maxEnergy / 3;
    }

    private void Update()
    {
        UpdateEnemyUI();
    }

    public void OnHealthChange()
    {
        Debug.Log("触发血量变化");
        float targetPersent = character.currentHealth / character.maxHealth;

        helathText.text = character.currentHealth.ToString("F0") + "/" + character.maxHealth.ToString("F0");
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
            energyFill[0].fillAmount = Mathf.Clamp01 (character.currentEnergy / energy3);
            energyFill[1].fillAmount = Mathf.Clamp01((character.currentEnergy - energy3) / energy3);
            energyFill[2].fillAmount = Mathf.Clamp01((character.currentEnergy - 2 * energy3) / energy3);

    }
}
