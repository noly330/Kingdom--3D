using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "CharacterData_SO", menuName = "SO/Character/CharacterData_SO")]
public class CharacterData_SO : ScriptableObject
{
    [Header("基础属性")]
    public float maxHealth;
    public float currentHealth;
    public float baseAttack;

}
