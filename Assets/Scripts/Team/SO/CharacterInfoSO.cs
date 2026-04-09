using UnityEngine;
[CreateAssetMenu(menuName = "Team/CharacterInfo")]
public class CharacterInfoSO : ScriptableObject
{
    [Header("角色信息")]
    public Sprite headSprite;
    public string characterName;
    public string characterDescription;
}
