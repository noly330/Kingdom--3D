using System.Collections;
using UnityEngine;
using UnityEngine.UI;
public class EnemyHealthBar : MonoBehaviour
{
    public GameObject healthBar;
    public Image healthFill;
    public float smoothTime = 0.3f;
    public float showTime = 2.5f;

    private CharacterBase character;
    private Camera mainCamera;
    private Coroutine healthCoroutine;
    private Coroutine showTimeCoroutine;

    private void Awake() {
        character = GetComponentInParent<CharacterBase>();
        mainCamera = Camera.main;
    }

    private void LateUpdate() 
    {
        if(mainCamera == null)
            mainCamera = Camera.main;
        //用血条坐标，再用相机朝向影响，生成一个点，然后血条看向这个点
        healthBar.transform.LookAt(healthBar.transform.position - mainCamera.transform.forward);
    }

    public void UpdateHealthBar()
    {
        Debug.Log("更新健康条");
        float target = character.currentHealth / character.maxHealth;

        if (healthCoroutine != null)
            StopCoroutine(healthCoroutine);
        healthCoroutine = StartCoroutine(HealthBarSmoothChange(target));

        if (showTimeCoroutine != null)
            StopCoroutine(showTimeCoroutine);
        showTimeCoroutine = StartCoroutine(ShowTime(showTime));
    }

    private IEnumerator HealthBarSmoothChange(float target)
    {
        float startPercent = healthFill.fillAmount;
        float elapsed = 0f;
        
        while (elapsed < smoothTime)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / smoothTime;
            healthFill.fillAmount = Mathf.Lerp(startPercent, target, t);
            yield return null;
        }
        
        healthFill.fillAmount = target;
        healthCoroutine = null;
        
    }

    private IEnumerator ShowTime(float showTime)
    {
        healthBar.SetActive(true);
        while (showTime > 0)
        {
            showTime -= Time.deltaTime;
            yield return null;
        }
        healthBar.SetActive(false);
    }

}
