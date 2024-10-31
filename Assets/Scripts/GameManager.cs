using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    #region Attributes
    
    private PoolManager PoolManager;
    private SpawnManager SpawnManager;

    [SerializeField] private PlayerController player;
    
    #endregion
    
    #region Unity API

    private void Awake()
    {
        Instance = this;
        
        PoolManager = PoolManager.instance;
        SpawnManager = SpawnManager.Instance;

        player = FindObjectOfType<PlayerController>();
    }

    #endregion

    #region Methods

    public PlayerController GetPlayer()
    {
        return player;
    }

    #endregion
    
}
