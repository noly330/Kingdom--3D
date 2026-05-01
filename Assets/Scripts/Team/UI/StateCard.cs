using UnityEngine;
using UnityEngine.UI;

public class StateCard : MonoBehaviour
{
    public Image _headSprite;
    public Image _healthFill;
    public Image _linkEnemyFill;

    private PlayerCharacter _character;
    private OperatorCombatController _operatorCombatController;

    private void Update()
    {
        UpdateLinkEnemyFill();
    }
    public void InitStateCard(PlayerCharacter Character, OperatorCombatController operatorCombatController)
    {
        _character = Character;
        _operatorCombatController = operatorCombatController;
        UpdateStateCard(Character);
    }

    public void UpdateStateCard(PlayerCharacter Character)
    {
        _headSprite.sprite = Character.characterInfo.headSprite;
        _healthFill.fillAmount = Character.currentHealth / Character.maxHealth;

    }

    public void UpdateLinkEnemyFill()
    {
        _linkEnemyFill.fillAmount = _operatorCombatController.currentLinkEnergy / _operatorCombatController.linkEnergy;
    }

}
