using UnityEngine;
using UnityEngine.UI;

public class StateCard : MonoBehaviour
{
    public Image _headSprite;
    public Image _healthFill;
    public Image _linkEnemyFill;

    private void OnEnable() {
        
    }

    private void OnDisable() {
        
    }

    public void UpdateStateCard(PlayerCharacter Character)
    {
        _headSprite.sprite = Character.characterInfo.headSprite;
        _healthFill.fillAmount = Character.currentHealth / Character.maxHealth;
        
    }

}
