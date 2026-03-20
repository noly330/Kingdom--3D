using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    private Transform _mainPlayer;
    public static EnemyManager Instance;
    private void Awake()
    {
        Instance = this;
        _mainPlayer = GameObject.FindGameObjectWithTag("Player").transform;
    }

    public Transform GetMainPlayer() => _mainPlayer;
    
    
}
