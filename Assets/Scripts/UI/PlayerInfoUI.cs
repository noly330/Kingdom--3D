using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInfoUI : MonoBehaviour
{
    //TODO:未来要做技能，技能能量条之类的
    public Image healthFill;
    public TextMeshProUGUI helathText;
    public CharacterBase character;
    private float smoothTime;

    public void OnHealthChange()
    {
        float targetPersent = character.currentHealth / character.maxHealth;

        helathText.text = character.currentHealth.ToString("F0") + "/" + character.maxHealth.ToString("F0");
        if(healthCoroutine !=null)
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
}
