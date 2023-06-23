using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CheatBtn : MonoBehaviour
{
    #region Scripts
    public RoundData roundData;
    public RoundManager roundManager;
    #endregion
    #region Flow
    void Awake()
    {
        Debug.Log("CheatBtn.Awake (start)");

        // Not ideal place
        roundData = GameObject.Find("Round Manager").GetComponent<RoundData>();
        roundManager = GameObject.Find("Round Manager").GetComponent<RoundManager>();

        Debug.Log("CheatBtn.Awake (end)");
    }


    void OnEnable()
    {
        Debug.Log("CheatBtn.OnEnable (start)");
        Debug.Log("CheatBtn.OnEnable (end)");
    }
    
    void Start()
    {
        Debug.Log("CheatBtn.Start (start)");
        Debug.Log("CheatBtn.Start (end)");
    }

    void Update()
    {

    }
    
    void OnDisable()
    {
        Debug.Log($"CheatBtn.OnDisable (start)");
        Debug.Log($"CheatBtn.OnDisable (end)");
    }

    void OnDestroy() 
    {
        Debug.Log($"CheatBtn.OnDestroy (start)");
        Debug.Log($"CheatBtn.OnDestroy (end)");
    }
    #endregion

    #region Button functions
    public void CheatMobAttackInterval()
    {
        Debug.Log($"CheatBtn.CheatMobAttackInterval (start)");

        if (roundManager.currentGameState == GameState.State.IsFlying)
        {
            Debug.Log($"In flying can't cheat");
        }
        else if (roundManager.currentGameState == GameState.State.IsBattling)
        {
            roundData.currentMob.currentAttackInterval = 1;
        }
        
        Debug.Log($"CheatBtn.CheatMobAttackInterval (end)");
    }
    #endregion
}