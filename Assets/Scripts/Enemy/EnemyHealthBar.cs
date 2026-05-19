using System.Collections;
using UnityEngine;
using UnityEngine.UI;
public class EnemyStatusHUD : MonoBehaviour
{
    [SerializeField] private GameObject healthBar;
    [SerializeField] private Image healthFill;
    [SerializeField] private Image _defenseBreakStackFill;
    [SerializeField] private float smoothTime = 0.3f;
    [SerializeField] private float showTime = 2.5f;

    private CharacterBase character;
    private EnemyAbnormalityManager _enemyAbnormalityManager;
    private Camera mainCamera;
    private Coroutine healthCoroutine;
    private Coroutine showTimeCoroutine;

    private void Awake()
    {
        character = GetComponent<CharacterBase>();
        _enemyAbnormalityManager = GetComponent<EnemyAbnormalityManager>();
        mainCamera = Camera.main;
    }

    private void OnEnable()
    {
        UpdateDefenseBreakStackFill();
        UpdateHealthBar();
    }

    private void OnDisable()
    {
        if (healthCoroutine != null)
            StopCoroutine(healthCoroutine);
        if (showTimeCoroutine != null)
            StopCoroutine(showTimeCoroutine);
    }

    private void LateUpdate()
    {
        if (mainCamera == null)
            mainCamera = Camera.main;
        //用血条坐标，再用相机朝向影响，生成一个点，然后血条看向这个点
        healthBar.transform.LookAt(healthBar.transform.position - mainCamera.transform.forward);
    }

    //在角色自带事件，OnHealthChange里面更新血条
    public void UpdateHealthBar()
    {
        if (character == null)
            return;
        
        float target = character.currentHealth / character.maxHealth;
        if (character.currentHealth <= 0)
        {
            healthBar.SetActive(false);
            return;
        }

        if (healthCoroutine != null)
            StopCoroutine(healthCoroutine);
        healthCoroutine = StartCoroutine(HealthBarSmoothChange(target));

        if (showTimeCoroutine != null)
            StopCoroutine(showTimeCoroutine);
        showTimeCoroutine = StartCoroutine(ShowTime(showTime));
    }

    public void UpdateDefenseBreakStackFill()
    {
        if(_enemyAbnormalityManager == null)
            return;

        if(_enemyAbnormalityManager.breakStack == 0)
        {
            _defenseBreakStackFill.gameObject.SetActive(false);
            return;
        }
        _defenseBreakStackFill.gameObject.SetActive(true);
        _defenseBreakStackFill.fillAmount = _enemyAbnormalityManager.breakStack / 4f;
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
