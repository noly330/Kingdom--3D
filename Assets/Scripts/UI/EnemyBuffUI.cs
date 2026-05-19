using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EnemyBuffUI : MonoBehaviour
{
    [SerializeField] private EnemyAbnormalityManager _enemyAbnormalityManager;
    [SerializeField] private GameObject _shatterBuffUI;
    [SerializeField] private TextMeshProUGUI _shatterStackText;

    private void Start()
    {
        _shatterBuffUI.SetActive(false);
    }


    public void UpdateEnemyBuffUI()
    {
        if(_enemyAbnormalityManager.shatterStacks == 0)
        {
            _shatterBuffUI.SetActive(false);
        }
        else
        {
            _shatterBuffUI.SetActive(true);
            _shatterStackText.text = _enemyAbnormalityManager.shatterStacks.ToString();
        }
    }
}
