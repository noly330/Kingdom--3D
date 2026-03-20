using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCombatController : MonoBehaviour
{
    [SerializeField] private bool _attackCommand;
    public bool GetAttackCommand => _attackCommand;
}
