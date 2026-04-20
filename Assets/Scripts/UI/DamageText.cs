using System.Collections;
using TMPro;
using UnityEngine;

public class DamageText : MonoBehaviour
{
    public ObjectPoolType poolType = ObjectPoolType.DamageText;
    public TextMeshProUGUI damageText;

    public float normalSize = 0.15f;
    public float critSize = 0.2f;
    public Color normalColor;
    public Color critColor;
    private Vector3 randomDirection = new Vector3(0, 1, 0);
    private Camera mainCamera;

    void Awake()
    {
        damageText = GetComponentInChildren<TextMeshProUGUI>();
    }

    private void OnEnable()
    {
        randomDirection = new Vector3(Random.Range(-1f, 1f), 0.2f, 0f);

        StartCoroutine(MoveText(2f));
    }

    private void LateUpdate()
    {
        if(mainCamera == null)
        
            mainCamera = Camera.main;

        damageText.transform.LookAt(damageText.transform.position + mainCamera.transform.forward);
        
    }

    public void SetDamageText(float damage,bool isCrit)
    {
        if (!isCrit)
        {
            damageText.fontSize = normalSize;
            damageText.text = damage.ToString();
            damageText.color = normalColor;
        }
        else
        {
            damageText.fontSize = critSize;
            damageText.text = damage.ToString();
            damageText.color = critColor;
        }
    }

    IEnumerator MoveText(float duration)
    {
        while (duration > 0)
        {
            damageText.transform.localPosition += randomDirection * 0.5f * Time.deltaTime;
            damageText.transform.localScale += Vector3.one * 0.2f * Time.deltaTime;
            duration -= Time.deltaTime;
            yield return null;
        }

        ObjectPool.instance.ReturnPool(ObjectPoolType.DamageText, gameObject);
    }

}
